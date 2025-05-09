using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration
{
    public class Edge : IEdge
    {
        private int _id;
        private ISymbol _source;
        private ISymbol _target;
        private EdgeType _edgeType;

        public Edge(int id, ISymbol source, ISymbol target, EdgeType edgeType)
        {
            _id = id;
            _source = source;
            _target = target;
            _edgeType = edgeType;
        }

        public int Id => _id;
        public ISymbol SourceSymbol => _source;
        public ISymbol TargetSymbol => _target;
        public EdgeType EdgeType => _edgeType;

        public INode? Source { get; set; }
        public INode? Target { get; set;  }
    }
}
