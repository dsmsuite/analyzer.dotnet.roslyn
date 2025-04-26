using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.Graph
{
    public interface IGraphBuilder
    {
        void AddNode(GraphNode node);
        void AddEdge(GraphEdge edge);
        IEnumerable<GraphNode> GetAllNodes();
        IEnumerable<GraphEdge> GetAllEdges();
    }
}
