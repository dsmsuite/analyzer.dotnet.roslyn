using dsmsuite.analyzer.dotnet.roslyn.Data;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Storage
{
    public class ResultStorage
    {
        private readonly IGraphRepository _repository;

        private int _filenameIndex = 0;
        private int _nodeTypeIndex = 0;
        private int _edgeTypeIndex = 0;

        private readonly Dictionary<string, int> _filenameIds = [];
        private readonly Dictionary<NodeType, int> _nodeTypeIds = [];
        private readonly Dictionary<EdgeType, int> _edgeTypeIds = [];

        public ResultStorage(IGraphRepository repository)
        {
            _repository = repository;
        }

        public void Persist(IGraph graph)
        {
            _repository.Create();

            //foreach (var nodeType in graph.NodeTypes)
            //{
            //    _repository.SaveNodeType(nodeType.Id, nodeType.Name);
            //}
            //foreach (var edgeType in graph.EdgeTypes)
            //{
            //    _repository.SaveEdgeType(edgeType.Id, edgeType.Name);
            //}
            //foreach (var filename in graph.Filenames)
            //{
            //    _repository.SaveSourceFilename(filename.Id, filename.Name);
            //}
            //foreach (var node in graph.Nodes)
            //{
            //    _repository.SaveNode(node.Id, node.Name, node.NodeTypeId, node.ParentId, node.FilenameId, node.Begin, node.End, node.CyclomaticComplexity);
            //}
            //foreach (IEdge edge in graph.Edges)
            //{
            //    _repository.SaveEdge(edge.Id, edge.Source?.Id, edge.Target?.Id, RegisterEdgeType(edge.EdgeType), 1);
            //}
        }

        private int RegisterFilename(string filename)
        {
            if (_filenameIds.ContainsKey(filename))
            {
                return _filenameIds[filename];
            }
            else
            {
                _filenameIndex++;
                _filenameIds[filename] = _filenameIndex;
                _repository.SaveSourceFilename(_filenameIndex, filename);
                return _filenameIndex;
            }
        }

        private int RegisterNodeType(NodeType nodeType)
        {
            if (_nodeTypeIds.ContainsKey(nodeType))
            {
                return _nodeTypeIds[nodeType];
            }
            else
            {
                _nodeTypeIndex++;
                _nodeTypeIds[nodeType] = _nodeTypeIndex;
                _repository.SaveNodeType(_nodeTypeIndex, nodeType.ToString());
                return _nodeTypeIndex;
            }
        }

        private int RegisterEdgeType(EdgeType edgeType)
        {
            if (_edgeTypeIds.ContainsKey(edgeType))
            {
                return _edgeTypeIds[edgeType];
            }
            else
            {
                _edgeTypeIndex++;
                _edgeTypeIds[edgeType] = _edgeTypeIndex;
                _repository.SaveEdgeType(_edgeTypeIndex, edgeType.ToString());
                return _edgeTypeIndex;
            }
        }
    }
}
