using dsmsuite.analyzer.dotnet.roslyn.Data;
using Microsoft.CodeAnalysis;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public interface ICodeAnalysisResult
    {
        int? RegisterNode(ISymbol symbol, NodeType nodeType, SyntaxNode syntaxNode, int cyclomaticComplexity = 0);
        int? RegisterEdge(ISymbol source, ISymbol target, EdgeType edgeType);

        void Save(IGraphRepository graphRepository);
    }
}
