using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration
{
    public class RegisteredSymbolEdge
    {
        public RegisteredSymbolEdge(int id, ISymbol source, ISymbol target, EdgeType edgeType)
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

        public RegisteredSymbolNode SourceNode { get; set; }
        public RegisteredSymbolNode TargetNode { get; set; }
    }
}
