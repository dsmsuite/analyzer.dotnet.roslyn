using dsmsuite.analyzer.dotnet.roslyn.Data;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;
using System.Xml.Linq;

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

        public int? RegisterNode(ISymbol symbol, NodeType nodeType, SyntaxNode syntaxNode, int cyclomaticComplexity)
        {
            _nodeIndex++;
            Node node = new Node(_nodeIndex, symbol, syntaxNode, nodeType, cyclomaticComplexity);
            _nodes[symbol] = node;

            RegisterNodeType(nodeType);
            RegisterFilename(node.Filename);

            return _nodeIndex;
        }

        public int? RegisterEdge(ISymbol source, ISymbol target, EdgeType edgeType)
        {
            if (!_nodes.ContainsKey(source))
            {
                Console.WriteLine($"Edge source not found: {source.Name}");
            }
            else if (!_nodes.ContainsKey(target))
            {
                Console.WriteLine($"Edge target not found: {target.Name}");
            }
            else
            {
                _edgeIndex++;
                Node sourceNode = _nodes[source];
                Node targetNode = _nodes[target];
                _edges.Add(new Edge(_edgeIndex, sourceNode, targetNode, edgeType));

                RegisterEdgeType(edgeType);
            }

            return null;
        }

        private void ResolveParents()
        {
            foreach (Node node in _nodes.Values)
            {
                 if (node.ContainingSymbol != null)
                {
                    if (_nodes.ContainsKey(node.ContainingSymbol))
                    {
                        node.Parent = _nodes[node.ContainingSymbol];
                    }
                }
            }
        }

        public void Save(IGraphRepository graphRepository)
        {
            ResolveParents();

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
                if (filenameId != null && nodeTypeId != null)
                {
                    graphRepository.SaveNode(node.Id, node.Fullname, nodeTypeId.Value, parentId, filenameId.Value, node.Startline, node.Endline, node.CyclomaticComplexity);
                }
            }

            foreach (Edge edge in _edges)
            {
                int? edgeTypeId = _edgeTypeIds[edge.EdgeType];
                if (edgeTypeId != null)
                {
                    graphRepository.SaveEdge(edge.Id, edge.Source.Id, edge.Target.Id, edgeTypeId.Value, 1);
                }
            }
        }
    }
}
