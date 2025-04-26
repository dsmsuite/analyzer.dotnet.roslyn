using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dsmsuite.analyzer.dotnet.roslyn.Graph
{
    public class InMemoryGraphBuilder : IGraphBuilder
    {
        private readonly Dictionary<int,GraphNode> _nodes = new();
        private readonly Dictionary<int, GraphEdge> _edges = new();

        public void AddNode(GraphNode node)
        {
            Console.WriteLine($"Add node id={node.Id} name={node.Name} type={node.Type} loc={node.LinesOfCode} cc={node.CyclomaticComplexity} to graph");
            _nodes[node.Id] =node;
        }

        public void AddEdge(GraphEdge edge)
        {
            //GraphNode source = _nodes[edge.SourceId];
            //GraphNode target = _nodes[edge.TargetId];

            //Console.WriteLine($"Add edge id={edge.Id} source={source.Name} target={target.Name} type={edge.Type}  strength={edge.Strength}  to graph");
            //_edges[edge.Id] = edge;
        }

        public IEnumerable<GraphNode> GetAllNodes() => _nodes.Values;
        public IEnumerable<GraphEdge> GetAllEdges() => _edges.Values;
    }
}
