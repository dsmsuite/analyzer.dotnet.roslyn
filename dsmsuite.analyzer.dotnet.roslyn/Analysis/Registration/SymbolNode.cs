using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration
{
    public class SymbolNode
    {
        private readonly List<SymbolNode> _children;
        private SymbolNode? _parent;

        public SymbolNode(int id, ISymbol symbol, ISymbol? parentSymbol, SyntaxNode syntaxNode, NodeType nodeType, int cyclomaticComplexity)
        {
            _children = new List<SymbolNode>();
            _parent = null;

            Id = id;
            Symbol = symbol;
            ParentSymbol = parentSymbol;
            SyntaxNode = syntaxNode;
            NodeType = nodeType;
            CyclomaticComplexity = cyclomaticComplexity;
        }

        public int Id;
        public ISymbol Symbol;
        public ISymbol? ParentSymbol;
        public SyntaxNode SyntaxNode;
        public NodeType NodeType;
        public int CyclomaticComplexity;

        public void InsertChildAtEnd(SymbolNode child)
        {
            _children.Add(child);
            if (child != null)
            {
                child._parent = this;
            }
        }
    }
}
