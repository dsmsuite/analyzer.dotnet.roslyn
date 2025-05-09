using dsmsuite.analyzer.dotnet.roslyn.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Reporting
{
    public interface IResultReporter
    {
        void ReportResult(string actionDescription,
            string syntaxNodeFilename,
            int syntaxNodeline,
            bool success,
            [CallerFilePath] string sourceFile = "",
            [CallerMemberName] string method = "",
            [CallerLineNumber] int lineNumber = 0);
    }
}
