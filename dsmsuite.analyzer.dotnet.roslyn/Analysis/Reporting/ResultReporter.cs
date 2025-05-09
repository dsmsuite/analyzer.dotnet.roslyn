using dsmsuite.analyzer.dotnet.roslyn.Util;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Reporting
{
    public class ResultReporter : IResultReporter
    {
        public void ReportResult(string actionDescription,
                            string syntaxNodeFilename,
                            int syntaxNodeline,
                            bool success,
                            string sourceFile,
                            string method,
                            int lineNumber)
        {
            Logger.LogResult(actionDescription, syntaxNodeFilename, syntaxNodeline, success, sourceFile, method, lineNumber);
        }
    }
}
