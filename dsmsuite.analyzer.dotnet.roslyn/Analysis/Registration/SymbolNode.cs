using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration
{
    public class RegisteredSymbolNode
    {
        private readonly List<RegisteredSymbolNode> _children;
        private RegisteredSymbolNode? _parent;

        public RegisteredSymbolNode(int id, ISymbol symbol, ISymbol? parentSymbol, SyntaxNode syntaxNode, NodeType nodeType, int cyclomaticComplexity)
        {
            _children = new List<RegisteredSymbolNode>();
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

        public void InsertChildAtEnd(RegisteredSymbolNode child)
        {
            _children.Add(child);
            if (child != null)
            {
                child._parent = this;
            }
        }
    }
}
