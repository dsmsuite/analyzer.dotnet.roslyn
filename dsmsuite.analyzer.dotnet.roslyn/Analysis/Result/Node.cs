using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration
{
    public class Node : INode
    {
        private int _id;
        private ISymbol _symbol;
        private ISymbol? _parentSymbol;
        private SyntaxNode _syntaxNode;
        private NodeType _nodeType;
        private int _cyclomaticComplexity;
        private readonly List<INode> _children;
        private INode? _parent;
        private string? _comment;

        public Node(int id, ISymbol symbol, ISymbol? parentSymbol, SyntaxNode syntaxNode, NodeType nodeType, int cyclomaticComplexity, string? comment)
        {
            _id = id;
            _symbol = symbol;
            _parentSymbol = parentSymbol;
            _syntaxNode = syntaxNode;
            _nodeType = nodeType;
            _cyclomaticComplexity = cyclomaticComplexity;
            _children = new List<INode>();
            _parent = null;
            _comment = comment;
        }

        public ISymbol? ParentSymbol => _parentSymbol;

        public int Id => _id;
        public string Name => _symbol.Name;
        public NodeType NodeType => _nodeType;

        public string Filename => _syntaxNode.SyntaxTree?.FilePath ?? "";
        public int Startline => _syntaxNode.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
        public int Endline => _syntaxNode.GetLocation().GetLineSpan().EndLinePosition.Line + 1;

        public int LinesOfCode => Endline - Startline + 1;
        public int CyclomaticComplexity => _cyclomaticComplexity;
        public string? Comment => _comment;

        public List<INode> Children => _children;
        public INode? Parent => _parent;

        public string Fullname
        {
            get
            {
                string fullname = Name;
                INode? parent = Parent;
                while (parent != null)
                {
                    if (parent.Name.Length > 0)
                    {
                        fullname = parent.Name + "." + fullname;
                    }
                    parent = parent.Parent;
                }
                return fullname;
            }
        }

        public void AddChildNode(INode child)
        {
            _children.Add(child);
            Node? c = child as Node;
            if (c != null)
            {
                c._parent = this;
            }
        }
    }
}
