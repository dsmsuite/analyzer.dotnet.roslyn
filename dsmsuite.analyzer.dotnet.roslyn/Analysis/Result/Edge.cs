using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration
{
    public class Edge : IEdge
    {
        public Edge(int id, INode source, INode target, EdgeType edgeType)
        {
            Id = id;
            Source = source;
            Target = target;
            EdgeType = edgeType;
        }

        public int Id { get; }
        public INode Source { get; }
        public INode Target { get; }
        public EdgeType EdgeType { get; }
    }
}
