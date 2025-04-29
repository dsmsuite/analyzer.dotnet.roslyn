using dsmsuite.analyzer.dotnet.roslyn.Analysis;

namespace dsmsuite.analyzer.dotnet.roslyn.Graph
{
    public interface IEdge
    {
        EdgeType EdgeType { get; }
        int Id { get; }
        INode Source { get; }
        INode Target { get; }
    }
}