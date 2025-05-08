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
        private readonly Dictionary<ISymbol, Node> _nodes = [];
        private readonly List<Edge> _edges = [];
        private readonly Dictionary<NodeType, int> _nodeTypeIds = [];
        private readonly Dictionary<EdgeType, int> _edgeTypeIds = [];

        public IDictionary<NodeType, int> NodeTypes => _nodeTypeIds;
        public IDictionary<EdgeType, int> EdgeTypes => _edgeTypeIds;
        public IEnumerable<INode> Nodes => _nodes.Values;

        public bool IsNodeRegistered(ISymbol symbol)
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

        public void RegisterResult(string actionDescription,
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

        public int? RegisterNode(ISymbol symbol, ISymbol? parent, NodeType nodeType, SyntaxNode syntaxNode, int cyclomaticComplexity)
        {
            _nodeIndex++;
            Node node = new Node(_nodeIndex, symbol, parent, syntaxNode, nodeType, cyclomaticComplexity);
            _nodes[symbol] = node;

            RegisterNodeType(nodeType);
            RegisterFilename(node.Filename);

            return _nodeIndex;
        }

        public int? RegisterEdge(ISymbol source, ISymbol target, EdgeType edgeType)
        {
            _edgeIndex++;

            _edges.Add(new Edge(_edgeIndex, source, target, edgeType));

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

            foreach (Node node in _nodes.Values)
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

            foreach (Edge edge in _edges)
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

                    Node sourceNode = _nodes[edge.Source];
                    Node targetNode = _nodes[edge.Target];
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
