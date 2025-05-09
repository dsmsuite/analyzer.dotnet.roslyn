using dsmsuite.analyzer.dotnet.roslyn.Analysis.Reporting;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration
{
    public class CodeAnalysisResult : ICodeAnalysisResult
    {
        private readonly IResultReporter _reporter;

        private int _nodeIndex = 0;
        private int _edgeIndex = 0;

        private readonly List<UnresolvedEdge> _unresolvedEdges = [];

        private readonly Dictionary<ISymbol, Node> _nodes = [];
        private readonly List<INode> _nodeHierarchy = [];
        private readonly List<Edge> _edges = [];

        public CodeAnalysisResult(IResultReporter reporter)
        {
            _reporter = reporter;
        }

        public IEnumerable<INode> NodeHierarchy => _nodeHierarchy;
        public IEnumerable<IEdge> Edges => _edges;

        public bool RegisterNode(SyntaxNode node,
                           ISymbol? nodeSymbol,
                           ISymbol? parent,
                           NodeType nodeType,
                           int cyclomaticComplexity = 0,
                           [CallerFilePath] string sourceFile = "",
                           [CallerMemberName] string method = "",
                           [CallerLineNumber] int lineNumber = 0)
        {
            bool success = false;
            if (nodeSymbol != null)
            {
                if (!IsNodeRegistered(nodeSymbol))
                {
                    _nodeIndex++;
                    _nodes[nodeSymbol] = new Node(_nodeIndex, nodeSymbol, parent, node, nodeType, cyclomaticComplexity); ;
                }
                success = true;
            }

            RegisterResult($"Parse node={nodeType}", node, success, sourceFile, method, lineNumber);

            return success;
        }

        public bool RegisterEdge(SyntaxNode node,
                                   ISymbol? edgeSource,
                                   ISymbol? edgeTarget,
                                   EdgeType edgeType,
                                   [CallerFilePath] string sourceFile = "",
                                   [CallerMemberName] string method = "",
                                   [CallerLineNumber] int lineNumber = 0)
        {
            bool success = false;

            if (edgeSource != null && edgeTarget != null)
            {
                _edgeIndex++;
                _unresolvedEdges.Add(new UnresolvedEdge(_edgeIndex, edgeSource, edgeTarget, edgeType));
                success = true;
            }

            RegisterResult($"Parse edge={edgeType}", node, success, sourceFile, method, lineNumber);

            return success;
        }

        public void BuildHierarchicalGraph()
        {
            _nodeHierarchy.Clear();

            foreach (Node node in _nodes.Values)
            {
                Node? parentNode = null;

                if (node.ParentSymbol != null && _nodes.ContainsKey(node.ParentSymbol))
                {
                    parentNode = _nodes[node.ParentSymbol];
                    parentNode.InsertChildAtEnd(node);
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
                    _edges.Add(new Edge(unresolvedEdge.Id, sourceNode, targetNode, unresolvedEdge.EdgeType));
                }
            }
        }

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
