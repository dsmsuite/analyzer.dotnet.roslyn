using dsmsuite.analyzer.dotnet.roslyn.Analysis.Reporting;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration
{
    public class ResultCollector : IResultCollector
    {
        private readonly IResultReporter _reporter;

        private int _nodeIndex = 0;
        private int _edgeIndex = 0;

        private readonly Dictionary<ISymbol, SymbolNode> _symbolNodes = [];
        private readonly List<SymbolEdge> _symbolEdges = [];

        public ResultCollector(IResultReporter reporter)
        {
            _reporter = reporter;
        }

        public bool RegisterNodeIfNotNull(SyntaxNode node,
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
                if (!IsSymbolNodeRegistered(nodeSymbol))
                {
                    _nodeIndex++;
                    _symbolNodes[nodeSymbol] = new SymbolNode(_nodeIndex, nodeSymbol, parent, node, nodeType, cyclomaticComplexity); ;
                }
                success = true;
            }

            RegisterResult($"Parse node={nodeType}", node, success, sourceFile, method, lineNumber);

            return success;
        }

        public bool RegisterEdgeIfNotNull(SyntaxNode node,
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
                _symbolEdges.Add(new SymbolEdge(_edgeIndex, edgeSource, edgeTarget, edgeType));
                success = true;
            }

            RegisterResult($"Parse edge={edgeType}", node, success, sourceFile, method, lineNumber);

            return success;
        }

        private bool IsSymbolNodeRegistered(ISymbol symbol)
        {
            return _symbolNodes.ContainsKey(symbol);
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


        public IGraph? BuildHierarchicalGraph()
        {
            List<SymbolNode> rootSymbolNodes = new List<SymbolNode>();

            foreach (SymbolNode symbolNode in _symbolNodes.Values)
            {

                SymbolNode parent = null;

                if (symbolNode.ParentSymbol != null && _symbolNodes.ContainsKey(symbolNode.ParentSymbol))
                {
                    parent = _symbolNodes[symbolNode.ParentSymbol];
                    parent.InsertChildAtEnd(symbolNode);
                }
                else
                {
                    rootSymbolNodes.Add(symbolNode);
                }
            }

            foreach (SymbolEdge edge in _symbolEdges)
            {
                if (!_symbolNodes.ContainsKey(edge.SourceSymbol))
                {
                    //Console.WriteLine($"Edge source not found: {source.Name}");
                }
                else if (!_symbolNodes.ContainsKey(edge.TargetSymbol))
                {
                    //Console.WriteLine($"Edge target not found: {target.Name}");
                }
                else
                {
                    edge.SourceNode = _symbolNodes[edge.SourceSymbol];
                    edge.TargetNode = _symbolNodes[edge.TargetSymbol];
                }
            }

            return null;
        }
    }
}
