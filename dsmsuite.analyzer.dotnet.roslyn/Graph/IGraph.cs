namespace dsmsuite.analyzer.dotnet.roslyn.Graph
{
    public interface IGraph
    {
        IEnumerable<IEdge> Edges { get; }
        IEnumerable<INode> NodeHierarchy { get; }
    }
}
