using dsmsuite.analyzer.dotnet.roslyn.Data;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Analyzer
{
    public interface ICodeAnalyzer
    {
        Task AnalyzeAsync();
    }
}
