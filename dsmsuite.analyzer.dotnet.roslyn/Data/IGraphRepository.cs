
using dsmsuite.analyzer.dotnet.roslyn.Graph;

namespace dsmsuite.analyzer.dotnet.roslyn.Data
{
    public interface IGraphRepository
    {
        void Save(IHierarchicalGraph hierarchicalGraph);
    }
}
