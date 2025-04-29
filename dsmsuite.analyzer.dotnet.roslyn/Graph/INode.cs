using Microsoft.CodeAnalysis;

namespace dsmsuite.analyzer.dotnet.roslyn.Graph
{
    public interface INode
    {
        int Id { get; }
        INode? Parent { get; }
        string Name { get; }
        string Fullname { get; }
        NodeType NodeType { get; }
        string Filename { get; }
        int Startline { get; }
        int Endline { get; }
        int CyclomaticComplexity { get; }
    }
}