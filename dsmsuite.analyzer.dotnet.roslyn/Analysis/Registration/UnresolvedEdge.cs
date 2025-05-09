using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration
{
    public class UnresolvedEdge
    {
        public UnresolvedEdge(int id, ISymbol sourceSymbol, ISymbol targetSymbol, EdgeType edgeType)
        {
            Id = id;
            SourceSymbol = sourceSymbol;
            TargetSymbol = targetSymbol;
            EdgeType = edgeType;
        }

        public int Id { get; }
        public ISymbol SourceSymbol { get; }
        public ISymbol TargetSymbol { get; }
        public EdgeType EdgeType { get; }
    }
}
