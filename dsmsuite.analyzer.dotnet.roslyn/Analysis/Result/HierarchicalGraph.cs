using dsmsuite.analyzer.dotnet.roslyn.Analysis.Reporting;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using dsmsuite.analyzer.dotnet.roslyn.Util;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

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
        public IEnumerable<INode> Nodes => _nodes.Values;
        public IEnumerable<IEdge> Edges => _edges;

        public void AddNode(SyntaxNode syntaxNode,
                            ISymbol? symbol,
                            ISymbol? parentSymbol,
                            NodeType nodeType,
                            int cyclomaticComplexity = 0,
                            [CallerFilePath] string sourceFile = "",
                            [CallerMemberName] string method = "",
                            [CallerLineNumber] int lineNumber = 0)
        {
            Result result = Result.Failed;

            if (symbol != null)
            {
                if (IsSymbolInSourceCode(symbol))
                {
                    if (!IsNodeRegistered(symbol))
                    {
                        _nodeCount++;
                        _nodes[symbol] = new Node(_nodeCount, symbol, parentSymbol, syntaxNode, nodeType, cyclomaticComplexity); ;
                    }
                    result = Result.Success;
                }
                else
                {
                    result = Result.Ignored;
                }
            }

            RegisterResult($"Parse node={nodeType}", syntaxNode, result, sourceFile, method, lineNumber);
        }

        public void AddEdge(SyntaxNode node,
                            ISymbol? sourceSymbol,
                            ISymbol? targetSymbol,
                            EdgeType edgeType,
                            [CallerFilePath] string sourceFile = "",
                            [CallerMemberName] string method = "",
                            [CallerLineNumber] int lineNumber = 0)
        {
            Result result = Result.Failed;

            if (sourceSymbol != null && targetSymbol != null)
            {
                if (IsSymbolInSourceCode(sourceSymbol) && IsSymbolInSourceCode(targetSymbol))
                {
                    _edgeCount++;
                    _unresolvedEdges.Add(new UnresolvedEdge(_edgeCount, sourceSymbol, targetSymbol, node, edgeType));
                    result = Result.Success;
                }
                else
                {
                    result = Result.Ignored;
                }
            }

            RegisterResult($"Parse edge={edgeType}", node, result, sourceFile, method, lineNumber);
        }

        public void Build()
        {
            _nodeHierarchy.Clear();
            _edges.Clear();

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
                                   Result result,
                                   [CallerFilePath] string sourceFile = "",
                                   [CallerMemberName] string method = "",
                                   [CallerLineNumber] int lineNumber = 0)
        {
            string syntaxNodeFilename = syntaxNode.SyntaxTree?.FilePath ?? "";
            int syntaxNodeline = syntaxNode.GetLocation().GetLineSpan().StartLinePosition.Line;

            _reporter.ReportResult(actionDescription, syntaxNodeFilename, syntaxNodeline, result, sourceFile, method, lineNumber);
        }

        private bool IsSymbolInSourceCode(ISymbol symbol)
        {
            return symbol.Locations.All(loc => loc.IsInSource);
        }
    }
}
