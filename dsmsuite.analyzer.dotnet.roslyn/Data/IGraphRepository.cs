using dsmsuite.analyzer.dotnet.roslyn.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.Data
{
    public interface IGraphRepository
    {
        void SaveNodes(IEnumerable<GraphNode> nodes);
        void SaveEdges(IEnumerable<GraphEdge> edges);
    }
}
