using dsmsuite.analyzer.dotnet.roslyn.Data;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public interface ICodeAnalysisResult
    {
        void Save(IGraphRepository graphRepository);

        public bool RegisterNodeIfNotNull(SyntaxNode node,
                   ISymbol? nodeSymbol,
                   ISymbol? parent,
                   NodeType nodeType,
                   int cyclomaticComplexity = 0,
                   [CallerFilePath] string sourceFile = "",
                   [CallerMemberName] string method = "",
                   [CallerLineNumber] int lineNumber = 0);

        public bool RegisterEdgeIfNotNull(SyntaxNode node,
                           ISymbol? edgeSource,
                           ISymbol? edgeTarget,
                           EdgeType edgeType,
                           [CallerFilePath] string sourceFile = "",
                           [CallerMemberName] string method = "",
                           [CallerLineNumber] int lineNumber = 0);
    }
}
