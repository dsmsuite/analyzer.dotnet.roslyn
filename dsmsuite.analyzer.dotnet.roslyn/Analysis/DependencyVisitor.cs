using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using dsmsuite.analyzer.dotnet.roslyn.Graph;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public class DependencyVisitor : CSharpSyntaxWalker
    {
        private readonly SemanticModel _semanticModel;
        private readonly IGraphBuilder _graphBuilder;
        private readonly string _filename;

        public DependencyVisitor(SemanticModel semanticModel, IGraphBuilder graphBuilder, string filename)
        {
            _semanticModel = semanticModel;
            _graphBuilder = graphBuilder;
            _filename = filename;
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            INamedTypeSymbol? symbol = _semanticModel.GetDeclaredSymbol(node);
            if (symbol == null) return;

            var graphNode = new GraphNode
            {
                Name = symbol.Name,
                Type = "Class",
                LinesOfCode = node.GetLocation().GetLineSpan().EndLinePosition.Line - node.GetLocation().GetLineSpan().StartLinePosition.Line + 1
            };
            _graphBuilder.AddNode(graphNode);

            base.VisitClassDeclaration(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            IMethodSymbol? symbol = _semanticModel.GetDeclaredSymbol(node);
            if (symbol == null) return;

            var graphNode = new GraphNode
            {
                Name = symbol.Name,
                Type = "Method",
                LinesOfCode = node.GetLocation().GetLineSpan().EndLinePosition.Line - node.GetLocation().GetLineSpan().StartLinePosition.Line + 1
            };
            _graphBuilder.AddNode(graphNode);

            base.VisitMethodDeclaration(node);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            var symbol = _semanticModel.GetSymbolInfo(node).Symbol as IMethodSymbol;
            if (symbol == null) return;

            // Example edge creation
            var edge = new GraphEdge
            {
                SourceId = -1, // You'd resolve real node IDs in practice
                TargetId = -1,
                Type = "Calls",
                Strength = 1
            };
            _graphBuilder.AddEdge(edge);

            base.VisitInvocationExpression(node);
        }
    }
}
