using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration
{
    public class SymbolEdge
    {
        public SymbolEdge(int id, ISymbol source, ISymbol target, EdgeType edgeType)
        {
            Id = id;
            SourceSymbol = source;
            TargetSymbol = target;
            EdgeType = edgeType;
        }

        public int Id { get; }
        public ISymbol SourceSymbol { get; }
        public ISymbol TargetSymbol { get; }
        public EdgeType EdgeType { get; }

        public SymbolNode SourceNode { get; set; }
        public SymbolNode TargetNode { get; set; }
    }
}
