using dsmsuite.analyzer.dotnet.roslyn.Data;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public interface ICodeAnalyzer
    {
        Task AnalyzeAsync(string InputPath, IGraphRepository graphRepository);
    }
}
