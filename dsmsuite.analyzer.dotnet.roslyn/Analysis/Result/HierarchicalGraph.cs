using dsmsuite.analyzer.dotnet.roslyn.Analysis.Reporting;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration
{
    public class HierarchicalGraph : IHierarchicalGraphBuilder, IHierarchicalGraph
    {
        private readonly IResultReporter _reporter;

        private int _nodeCount = 0;
        private int _edgeCount = 0;

        private readonly List<UnresolvedEdge> _unresolvedEdges = [];

        private readonly Dictionary<ISymbol, Node> _nodes = [];
        private readonly List<INode> _nodeHierarchy = [];
        private readonly List<Edge> _edges = [];

        public HierarchicalGraph(IResultReporter reporter)
        {
            _reporter = reporter;
        }

        public IEnumerable<INode> NodeHierarchy => _nodeHierarchy;
        public IEnumerable<IEdge> Edges => _edges;

        public bool AddNode(SyntaxNode node,
                            ISymbol? symbol,
                            ISymbol? parentSymbol,
                            NodeType nodeType,
                            int cyclomaticComplexity = 0,
                            [CallerFilePath] string sourceFile = "",
                            [CallerMemberName] string method = "",
                            [CallerLineNumber] int lineNumber = 0)
        {
            bool success = false;

            if (symbol != null)
            {
                if (!IsNodeRegistered(symbol))
                {
                    _nodeCount++;
                    _nodes[symbol] = new Node(_nodeCount, symbol, parentSymbol, node, nodeType, cyclomaticComplexity); ;
                }
                success = true;
            }

            RegisterResult($"Parse node={nodeType}", node, success, sourceFile, method, lineNumber);

            return success;
        }

        public bool AddEdge(SyntaxNode node,
                            ISymbol? sourceSymbol,
                            ISymbol? targetSymbol,
                            EdgeType edgeType,
                            [CallerFilePath] string sourceFile = "",
                            [CallerMemberName] string method = "",
                            [CallerLineNumber] int lineNumber = 0)
        {
            bool success = false;

            if (sourceSymbol != null && targetSymbol != null)
            {
                _edgeCount++;
                _unresolvedEdges.Add(new UnresolvedEdge(_edgeCount, sourceSymbol, targetSymbol, node, edgeType));
                success = true;
            }

            RegisterResult($"Parse edge={edgeType}", node, success, sourceFile, method, lineNumber);

            return success;
        }

        public void Build()
        {
            _nodeHierarchy.Clear();

            foreach (Node node in _nodes.Values)
            {
                Node? parentNode = null;

                if (node.ParentSymbol != null && _nodes.ContainsKey(node.ParentSymbol))
                {
                    parentNode = _nodes[node.ParentSymbol];
                    parentNode.AddChildNode(node);
                }
                else
                {
                    _nodeHierarchy.Add(node);
                }
            }

            foreach (UnresolvedEdge unresolvedEdge in _unresolvedEdges)
            {
                if (_nodes.ContainsKey(unresolvedEdge.SourceSymbol) && _nodes.ContainsKey(unresolvedEdge.TargetSymbol))
                {
                    Node sourceNode = _nodes[unresolvedEdge.SourceSymbol];
                    Node targetNode = _nodes[unresolvedEdge.TargetSymbol];
                    _edges.Add(new Edge(unresolvedEdge.Id, sourceNode, targetNode, unresolvedEdge.SyntaxNode, unresolvedEdge.EdgeType));
                }
            }
        }

        public int EdgeCount => _edgeCount;
        public int NodeCount => _nodeCount;

        private bool IsNodeRegistered(ISymbol symbol)
        {
            return _nodes.ContainsKey(symbol);
        }

        private void RegisterResult(string actionDescription,
                                   SyntaxNode syntaxNode,
                                   bool success,
                                   [CallerFilePath] string sourceFile = "",
                                   [CallerMemberName] string method = "",
                                   [CallerLineNumber] int lineNumber = 0)
        {
            string syntaxNodeFilename = syntaxNode.SyntaxTree?.FilePath ?? "";
            int syntaxNodeline = syntaxNode.GetLocation().GetLineSpan().StartLinePosition.Line;

            _reporter.ReportResult(actionDescription, syntaxNodeFilename, syntaxNodeline, success, sourceFile, method, lineNumber);
        }
    }
}
