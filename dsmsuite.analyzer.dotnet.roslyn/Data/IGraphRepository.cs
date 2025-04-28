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
        void Create();
        void SaveNodeType(int id, string name);
        void SaveEdgeType(int id, string name);
        void SaveSourceFilename(int id, string filename);
        void SaveNodes(IEnumerable<GraphNode> nodes);
        void SaveEdges(IEnumerable<GraphEdge> edges);
        void SaveNode(int id, string name, int nodeTypeId, int? parentId, int filenameId, int begin, int end, int loc, int? cyclomaticComplexity);
        void SaveEdge(int id, int sourceId, int targetId, int edgeTYpe, int strength);
    }
}
