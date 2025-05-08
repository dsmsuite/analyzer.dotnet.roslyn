using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public class RegisteredSymbolEdge
    {
        private int _id;
        private ISymbol _source;
        private ISymbol _target;
        private EdgeType _edgeType;

        public RegisteredSymbolEdge(int id, ISymbol source, ISymbol target, EdgeType edgeType)
        {
            _id = id;
            _source = source;
            _target = target;
            _edgeType = edgeType;
        }

        public int Id => _id;
        public ISymbol Source => _source;
        public ISymbol Target => _target;
        public EdgeType EdgeType => _edgeType;
    }
}
