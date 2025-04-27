using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public interface IDependencyVisitorCallback
    {
        int? RegisterNode(ISymbol symbol, NodeType nodeType, SyntaxNode syntaxNode, int? cyclomaticComplexity = null);
        int? RegisterEdge(ISymbol source, ISymbol target, EdgeType edgeType);
    }
}
