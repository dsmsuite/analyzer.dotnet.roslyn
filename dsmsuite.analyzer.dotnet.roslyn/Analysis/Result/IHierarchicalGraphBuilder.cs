using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration
{
    public interface IHierarchicalGraphBuilder
    {
        void AddNode(SyntaxNode node,
                   ISymbol? nodeSymbol,
                   ISymbol? parent,
                   NodeType nodeType,
                   int cyclomaticComplexity = 0,
                   [CallerFilePath] string sourceFile = "",
                   [CallerMemberName] string method = "",
                   [CallerLineNumber] int lineNumber = 0);

        void AddEdge(SyntaxNode node,
                           ISymbol? edgeSource,
                           ISymbol? edgeTarget,
                           EdgeType edgeType,
                           [CallerFilePath] string sourceFile = "",
                           [CallerMemberName] string method = "",
                           [CallerLineNumber] int lineNumber = 0);

        void Build();
    }
}
