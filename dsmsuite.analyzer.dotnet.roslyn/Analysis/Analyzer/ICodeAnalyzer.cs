namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Analyzer
{
    public interface ICodeAnalyzer
    {
        Task AnalyzeAsync();
    }
}
