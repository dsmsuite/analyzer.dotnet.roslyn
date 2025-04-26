using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.Graph
{
    public class InMemoryGraphBuilder : IGraphBuilder
    {
        private readonly List<GraphNode> _nodes = new();
        private readonly List<GraphEdge> _edges = new();

        public void AddNode(GraphNode node) => _nodes.Add(node);
        public void AddEdge(GraphEdge edge) => _edges.Add(edge);
        public IEnumerable<GraphNode> GetAllNodes() => _nodes;
        public IEnumerable<GraphEdge> GetAllEdges() => _edges;
    }
}
