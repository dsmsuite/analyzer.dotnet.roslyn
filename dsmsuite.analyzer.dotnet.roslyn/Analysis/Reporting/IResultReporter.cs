using dsmsuite.analyzer.dotnet.roslyn.Util;
using System.Runtime.CompilerServices;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Reporting
{
    public interface IResultReporter
    {
        void ReportResult(string actionDescription,
            string syntaxNodeFilename,
            int syntaxNodeline,
            Result result,
            [CallerFilePath] string sourceFile = "",
            [CallerMemberName] string method = "",
            [CallerLineNumber] int lineNumber = 0);
    }
}
