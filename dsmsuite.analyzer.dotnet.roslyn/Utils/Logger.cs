using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace dsmsuite.analyzer.dotnet.roslyn.Util
{
    /// <summary>
    /// Provides logging to be used for diagnostic purposes
    /// </summary>
    public class Logger
    {
        private static Assembly _assembly;
        private static string _logPath;
        private static readonly Dictionary<Action, int> _actionTotalCount;
        private static readonly Dictionary<Action, int> _actionFailedCount;

        public class Action
        {
            public Action(string description, string file, string method, int line)
            {
                Description = description;
                File = file;
                Method = method;
                Line = line;
            }

            public string File { get; }
            public string Description { get; }
            public string Method { get; }
            public int Line { get; }

            public override int GetHashCode()
            {
                return HashCode.Combine(Description, File, Method, Line);
            }

            public override bool Equals(object obj)
            {
                return obj is Action other &&
                       Description == other.Description &&
                       File == other.File &&
                       Method == other.Method &&
                       Line == other.Line;
            }
        }

        public static DirectoryInfo LogDirectory { get; private set; }

        static Logger()
        {
            _actionTotalCount = new Dictionary<Action, int>();
            _actionFailedCount = new Dictionary<Action, int>();
        }

        public static void Init(Assembly assembly, bool logInCurrentDirectory)
        {
            _assembly = assembly;
            _logPath = logInCurrentDirectory ? Directory.GetCurrentDirectory() : @"C:\Temp\DsmSuiteLogging\";

            LogLevel = LogLevel.None;
        }

        public static LogLevel LogLevel { get; set; }



        public static void LogResourceUsage()
        {
            Process currentProcess = Process.GetCurrentProcess();
            const long million = 1000000;
            long peakPagedMemMb = currentProcess.PeakPagedMemorySize64 / million;
            long peakVirtualMemMb = currentProcess.PeakVirtualMemorySize64 / million;
            long peakWorkingSetMb = currentProcess.PeakWorkingSet64 / million;
            LogUserMessage($"Peak physical memory usage {peakWorkingSetMb:0.000}MB");
            LogUserMessage($"Peak paged memory usage    {peakPagedMemMb:0.000}MB");
            LogUserMessage($"Peak virtual memory usage  {peakVirtualMemMb:0.000}MB");
        }

        public static void LogAssemblyInfo()
        {
            string name = _assembly.GetName().Name;
            string version = _assembly.GetName().Version.ToString();
            DateTime buildDate = new FileInfo(_assembly.Location).LastWriteTime;
            LogUserMessage(name + " version =" + version + " build=" + buildDate);
        }

        public static void LogUserMessage(string message,
            [CallerFilePath] string sourceFile = "",
            [CallerMemberName] string method = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Console.WriteLine(message);
            LogToFile(LogLevel.User, "userMessages.log", sourceFile, method, lineNumber, "user", message);
        }

        public static void LogError(string message,
            [CallerFilePath] string sourceFile = "",
            [CallerMemberName] string method = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogToFile(LogLevel.Error, "errorMessages.log", sourceFile, method, lineNumber, "error", message);
        }

        public static void LogException(string message, Exception e,
            [CallerFilePath] string sourceFile = "",
            [CallerMemberName] string method = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogToFile(LogLevel.Error, "exceptions.log", sourceFile, method, lineNumber, "exception", message + "\n" + e.StackTrace + "\n");
        }

        public static void LogWarning(string message,
            [CallerFilePath] string sourceFile = "",
            [CallerMemberName] string method = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogToFile(LogLevel.Warning, "warningMessages.log", sourceFile, method, lineNumber, "warning", message);
        }

        public static void LogInfo(string message,
            [CallerFilePath] string sourceFile = "",
            [CallerMemberName] string method = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogToFile(LogLevel.Info, "infoMessages.log", sourceFile, method, lineNumber, "info", message);
        }

        public static void LogResult(string actionDescription,
            string syntaxNodeFilename,
            int syntaxNodeline,
            bool success,
            [CallerFilePath] string sourceFile = "",
            [CallerMemberName] string method = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Action action = new Action(actionDescription, sourceFile, method, lineNumber);
            IncrementActionCount(_actionTotalCount, action);

            if (!success)
            {
                IncrementActionCount(_actionFailedCount, action);

                LogError($"Action={actionDescription} failed in file={syntaxNodeFilename} line={syntaxNodeline}", sourceFile, method, lineNumber);
            }
        }

        private static void IncrementActionCount(Dictionary<Action, int> locationLogCount, Action logLocation)
        {
            if (locationLogCount.ContainsKey(logLocation))
            {
                locationLogCount[logLocation] = locationLogCount[logLocation] + 1;
            }
            else
            {
                locationLogCount[logLocation] = 1;
            }
        }

        private static int GetLocationCount(Dictionary<Action, int> locationLogCount, Action logLocation)
        {
            return locationLogCount.ContainsKey(logLocation) ? locationLogCount[logLocation] : 0;
        }

        public static void Flush()
        {
            LogSummary();
        }

        private static void LogToFile(LogLevel logLevel, string logFilename, string sourceFile, string method, int lineNumber, string catagory, string message)
        {
            if (LogLevel >= logLevel)
            {
                string path = GetLogFullPath(logFilename);
                FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(FormatLine(sourceFile, method, lineNumber, catagory, message));
                }
            }
        }

        private static void LogSummary()
        {
            string path = GetLogFullPath("summary.log");
            FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(fs))
            {
                foreach (Action action in _actionTotalCount.Keys)
                {
                    int failed = GetLocationCount(_actionFailedCount, action);
                    int total = GetLocationCount(_actionTotalCount, action);
                    writer.WriteLine($"Action={action.Description} File={action.File} Method={action.Method} Line={action.Line} Failed={failed}/{total}");
                }
            }
        }

        private static DirectoryInfo CreateLogDirectory()
        {
            DateTime now = DateTime.Now;
            string timestamp = $"{now.Year:0000}-{now.Month:00}-{now.Day:00}-{now.Hour:00}-{now.Minute:00}-{now.Second:00}";
            string assemblyName = _assembly.GetName().Name;
            return Directory.CreateDirectory($@"{_logPath}\{assemblyName}_{timestamp}\");
        }

        private static string GetLogFullPath(string logFilename)
        {
            if (LogDirectory == null)
            {
                LogDirectory = CreateLogDirectory();
            }

            return Path.GetFullPath(Path.Combine(LogDirectory.FullName, logFilename));
        }

        private static string FormatLine(string sourceFile, string method, int lineNumber, string category, string text)
        {
            return StripPath(sourceFile) + " " + method + "() line=" + lineNumber + " " + category + "=" + text;
        }

        private static string StripPath(string sourceFile)
        {
            char[] separators = { '\\' };
            string[] parts = sourceFile.Split(separators);
            return parts[parts.Length - 1];
        }
    }
}
