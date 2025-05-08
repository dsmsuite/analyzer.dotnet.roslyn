namespace dsmsuite.analyzer.dotnet.roslyn.Graph
{
    public interface IEdge
    {
        int Id { get; }
        EdgeType EdgeType { get; }
        INode? Source { get; }
        INode? Target { get; }
    }
}