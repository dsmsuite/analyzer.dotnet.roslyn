using dsmsuite.analyzer.dotnet.roslyn.Graph;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public class Edge : IEdge
    {
        private int _id;
        private Node _source;
        private Node _target;
        private EdgeType _edgeType;

        public Edge(int id, Node source, Node target, EdgeType edgeType)
        {
            _id = id;
            _source = source;
            _target = target;
            _edgeType = edgeType;
        }

        public int Id => _id;
        public INode Source => _source;
        public INode Target => _target;
        public EdgeType EdgeType => _edgeType;
    }
}
