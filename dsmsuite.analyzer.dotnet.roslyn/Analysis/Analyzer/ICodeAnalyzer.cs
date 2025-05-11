using dsmsuite.analyzer.dotnet.roslyn.Graph;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Analyzer
{
    public interface ICodeAnalyzer
    {
        Task AnalyzeAsync();

        void BuildGraph();

        IHierarchicalGraph Graph { get; }
    }
}
