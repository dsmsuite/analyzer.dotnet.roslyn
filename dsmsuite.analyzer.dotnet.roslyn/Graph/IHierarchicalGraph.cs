namespace dsmsuite.analyzer.dotnet.roslyn.Graph
{
    public interface IHierarchicalGraph
    {
        IEnumerable<IEdge> Edges { get; }
        IEnumerable<INode> NodeHierarchy { get; }
        IEnumerable<INode> Nodes { get; }

        int EdgeCount { get; }
        int NodeCount { get; }
    }
}
