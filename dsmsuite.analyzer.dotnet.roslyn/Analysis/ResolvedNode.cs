using dsmsuite.analyzer.dotnet.roslyn.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public class ResolvedNode : INode
    {
        public ResolvedNode()
        {

        }
        public int Id => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string Fullname => throw new NotImplementedException();

        public NodeType NodeType => throw new NotImplementedException();

        public string Filename => throw new NotImplementedException();

        public int Startline => throw new NotImplementedException();

        public int Endline => throw new NotImplementedException();

        public int CyclomaticComplexity => throw new NotImplementedException();
    }
}
