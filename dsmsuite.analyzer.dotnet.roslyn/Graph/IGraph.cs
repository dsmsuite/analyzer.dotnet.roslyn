using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.Graph
{
    public interface IGraph
    {
        IEnumerable<IEdge> Edges { get; }
        IEnumerable<INode> NodeHierarchy { get; }
    }
}
