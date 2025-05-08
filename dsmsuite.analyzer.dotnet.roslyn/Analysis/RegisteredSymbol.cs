using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public class RegisteredSymbol
    {
        private int _id;
        private ISymbol _symbol;
        private ISymbol _parent;
        private SyntaxNode _syntaxNode;
        private NodeType _nodeType;
        private int _cyclomaticComplexity;

        public RegisteredSymbol(int id, ISymbol symbol, ISymbol? parent, SyntaxNode syntaxNode, NodeType nodeType, int cyclomaticComplexity)
        {
            _id = id;
            _symbol = symbol;
            _parent = parent;
            _syntaxNode = syntaxNode;
            _nodeType = nodeType;
            _cyclomaticComplexity = cyclomaticComplexity;
        }

        public ISymbol? Parent => _parent;
        public int Id => _id;
        public string Name => _symbol.Name;
        public string Fullname => _symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        public NodeType NodeType => _nodeType;
        public string Filename => _syntaxNode.SyntaxTree?.FilePath ?? "";
        public int Startline => _syntaxNode.GetLocation().GetLineSpan().StartLinePosition.Line;
        public int Endline => _syntaxNode.GetLocation().GetLineSpan().EndLinePosition.Line;
        public int LinesOfCode => Endline - Startline + 1;
        public int CyclomaticComplexity => _cyclomaticComplexity;
    }
}
