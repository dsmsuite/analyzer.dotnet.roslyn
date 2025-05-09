namespace dsmsuite.analyzer.dotnet.roslyn.Graph
{
    public interface INode
    {
        int Id { get; }
        string Name { get; }
        NodeType NodeType { get; }
        string Filename { get; }
        int Startline { get; }
        int Endline { get; }
        int CyclomaticComplexity { get; }

        List<INode> Children { get; }
        INode? Parent { get; }
        string Fullname { get; }
    }
}