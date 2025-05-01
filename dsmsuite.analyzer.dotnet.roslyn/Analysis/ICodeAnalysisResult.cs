using dsmsuite.analyzer.dotnet.roslyn.Data;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public interface ICodeAnalysisResult
    {
        void RegisterResult(SyntaxNode syntaxNode,
                             bool success,
                             [CallerFilePath] string sourceFile = "",
                             [CallerMemberName] string method = "",
                             [CallerLineNumber] int lineNumber = 0);

        int? RegisterNode(ISymbol symbol, ISymbol? parent, NodeType nodeType, SyntaxNode syntaxNode, int cyclomaticComplexity = 0);
        int? RegisterEdge(ISymbol source, ISymbol target, EdgeType edgeType);

        void Save(IGraphRepository graphRepository);
    }
}
