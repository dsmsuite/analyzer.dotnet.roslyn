using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration
{
    public class Edge : IEdge
    {
        private SyntaxNode _syntaxNode;
        public Edge(int id, INode source, INode target, SyntaxNode syntaxNode, EdgeType edgeType)
        {
            Id = id;
            Source = source;
            Target = target;
            EdgeType = edgeType;

            _syntaxNode = syntaxNode;
        }

        public int Id { get; }
        public INode Source { get; }
        public INode Target { get; }
        public EdgeType EdgeType { get; }

        public string Filename => _syntaxNode.SyntaxTree?.FilePath ?? "";
        public int Line => _syntaxNode.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
    }
}
