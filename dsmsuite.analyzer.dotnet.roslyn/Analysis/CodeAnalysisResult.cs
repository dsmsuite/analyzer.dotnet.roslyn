using dsmsuite.analyzer.dotnet.roslyn.Data;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using dsmsuite.analyzer.dotnet.roslyn.Util;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public class CodeAnalysisResult : ICodeAnalysisResult
    {
        private int _filenameIndex = 0;
        private int _nodeIndex = 0;
        private int _edgeIndex = 0;
        private int _nodeTypeIndex = 0;
        private int _edgeTypeIndex = 0;
        private readonly Dictionary<string, int> _filenameIds = [];
        private readonly Dictionary<ISymbol, RegisteredSymbol> _nodes = [];
        private readonly List<RegisteredSymbolEdge> _edges = [];
        private readonly Dictionary<NodeType, int> _nodeTypeIds = [];
        private readonly Dictionary<EdgeType, int> _edgeTypeIds = [];

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
            string actionDescription = $"Parse node={nodeType}";
            if (nodeSymbol != null)
            {
                if (!IsNodeRegistered(nodeSymbol))
                {
                    RegisterNode(nodeSymbol, parent, nodeType, node, cyclomaticComplexity);
                }
                success = true;
            }

            RegisterResult(actionDescription, node, success, sourceFile, method, lineNumber);
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
            string actionDescription = $"Parse edge={edgeType}";

            if (edgeSource != null && edgeTarget != null)
            {
                RegisterEdge(edgeSource, edgeTarget, edgeType);
                success = true;
            }

            RegisterResult(actionDescription, node, success, sourceFile, method, lineNumber);

            return success;
        }

        private bool IsNodeRegistered(ISymbol symbol)
        {
            return _nodes.ContainsKey(symbol);
        }

        private void RegisterFilename(string filename)
        {
            if (!_filenameIds.ContainsKey(filename))
            {
                _filenameIndex++;
                _filenameIds[filename] = _filenameIndex;
            }
        }

        private void RegisterNodeType(NodeType nodeType)
        {
            if (!_nodeTypeIds.ContainsKey(nodeType))
            {
                _nodeTypeIndex++;
                _nodeTypeIds[nodeType] = _nodeTypeIndex;
            }
        }

        private void RegisterEdgeType(EdgeType edgeType)
        {
            if (!_edgeTypeIds.ContainsKey(edgeType))
            {
                _edgeTypeIndex++;
                _edgeTypeIds[edgeType] = _edgeTypeIndex;
            }
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

            Logger.LogResult(actionDescription, syntaxNodeFilename, syntaxNodeline, success, sourceFile, method, lineNumber);
        }

        private int? RegisterNode(ISymbol symbol, ISymbol? parent, NodeType nodeType, SyntaxNode syntaxNode, int cyclomaticComplexity)
        {
            _nodeIndex++;
            RegisteredSymbol node = new RegisteredSymbol(_nodeIndex, symbol, parent, syntaxNode, nodeType, cyclomaticComplexity);
            _nodes[symbol] = node;

            RegisterNodeType(nodeType);
            RegisterFilename(node.Filename);

            return _nodeIndex;
        }

        private int? RegisterEdge(ISymbol source, ISymbol target, EdgeType edgeType)
        {
            _edgeIndex++;

            _edges.Add(new RegisteredSymbolEdge(_edgeIndex, source, target, edgeType));

            RegisterEdgeType(edgeType);

            return null;
        }

        public void Save(IGraphRepository graphRepository)
        {
            graphRepository.Create();

            foreach (KeyValuePair<NodeType, int> keyValuePair in _nodeTypeIds)
            {
                graphRepository.SaveNodeType(keyValuePair.Value, keyValuePair.Key.ToString());
            }

            foreach (KeyValuePair<EdgeType, int> keyValuePair in _edgeTypeIds)
            {
                graphRepository.SaveEdgeType(keyValuePair.Value, keyValuePair.Key.ToString());
            }

            foreach (KeyValuePair<string, int> keyValuePair in _filenameIds)
            {
                graphRepository.SaveSourceFilename(keyValuePair.Value, keyValuePair.Key);
            }

            foreach (RegisteredSymbol node in _nodes.Values)
            {
                int? filenameId = _filenameIds[node.Filename];
                int? nodeTypeId = _nodeTypeIds[node.NodeType];
                int? parentId = null;

                if (node.Parent != null)
                {
                    if (_nodes.ContainsKey(node.Parent))
                    {
                        parentId = _nodes[node.Parent].Id;
                    }
                }

                if (filenameId != null && nodeTypeId != null)
                {
                    graphRepository.SaveNode(node.Id, node.Name, nodeTypeId.Value, parentId, filenameId.Value, node.Startline, node.Endline, node.CyclomaticComplexity);
                }
            }

            foreach (RegisteredSymbolEdge edge in _edges)
            {
                if (!_nodes.ContainsKey(edge.Source))
                {
                    //Console.WriteLine($"Edge source not found: {source.Name}");
                }
                else if (!_nodes.ContainsKey(edge.Target))
                {
                    //Console.WriteLine($"Edge target not found: {target.Name}");
                }
                else
                {

                    RegisteredSymbol sourceNode = _nodes[edge.Source];
                    RegisteredSymbol targetNode = _nodes[edge.Target];
                    int? edgeTypeId = _edgeTypeIds[edge.EdgeType];
                    if (edgeTypeId != null)
                    {
                        graphRepository.SaveEdge(edge.Id, sourceNode.Id, targetNode.Id, edgeTypeId.Value, 1);
                    }
                }
            }
        }
    }
}
