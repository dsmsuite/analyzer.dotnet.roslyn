
namespace dsmsuite.analyzer.dotnet.roslyn.Data
{
    public interface IGraphRepository
    {
        void Create();
        void SaveNodeType(int id, string name);
        void SaveEdgeType(int id, string name);
        void SaveSourceFilename(int id, string filename);
        void SaveNode(int id, string name, int nodeTypeId, int? parentId, int filenameId, int begin, int end, int cyclomaticComplexity);
        void SaveEdge(int id, int sourceId, int targetId, int edgeTypeId, int strength);
    }
}
