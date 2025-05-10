namespace dsmsuite.analyzer.dotnet.roslyn.Graph
{
    public interface IEdge
    {
        int Id { get; }
        INode Source { get; }
        INode Target { get; }
        EdgeType EdgeType { get; }

        string Filename { get; }
        int Line { get; }
    }
}