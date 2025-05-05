namespace dsmsuite.analyzer.dotnet.roslyn.Graph
{
    public interface IEdge
    {
        EdgeType EdgeType { get; }
        int Id { get; }
    }
}