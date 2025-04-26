using dsmsuite.analyzer.dotnet.roslyn.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public interface ICodeAnalyzer
    {
        Task AnalyzeAsync(string solutionPath, IGraphBuilder graphBuilder);
    }
}
