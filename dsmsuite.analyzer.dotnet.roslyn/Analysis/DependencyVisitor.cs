using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    // Roslyn visitors(like CSharpSyntaxWalker) are recursive.
    // -If you don't call base.VisitXXX(node), you are responsible for visiting any children manually.
    // -If you do call base.VisitXXX(node), Roslyn will automatically visit all the child nodes for you.
    // This is usually the easiest way to make sure you don't miss anything deeper inside.
    public class DependencyVisitor : CSharpSyntaxWalker
    {
        private readonly SemanticModel _semanticModel;
        private readonly ICodeAnalysisResult _codeAnalysisResult;

        public DependencyVisitor(SemanticModel semanticModel, ICodeAnalysisResult codeAnalysisResult)
        {
            _semanticModel = semanticModel;
            _codeAnalysisResult = codeAnalysisResult;
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            base.VisitClassDeclaration(node);

            INamedTypeSymbol? classSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (classSymbol != null)
            {
                _codeAnalysisResult.RegisterNode(classSymbol, NodeType.Class, node);

                if (classSymbol.BaseType != null && classSymbol.BaseType.SpecialType != SpecialType.System_Object)
                {
                    INamedTypeSymbol? baseClassSymbol = classSymbol.BaseType;
                    _codeAnalysisResult.RegisterEdge(classSymbol, baseClassSymbol, EdgeType.Inheritance);
                }

                foreach (INamedTypeSymbol interfaceType in classSymbol.Interfaces)
                {
                    _codeAnalysisResult.RegisterEdge(classSymbol, interfaceType, EdgeType.Implements);
                }
            }
        }

        private bool IsVoid(ITypeSymbol typeSymbol)
        {
            return typeSymbol.SpecialType == SpecialType.System_Void;
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            base.VisitMethodDeclaration(node);

            IMethodSymbol? methodSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (methodSymbol != null)
            {
                int? cyclomaticComplexity = CalculateCyclomaticComplexity(node);

                _codeAnalysisResult.RegisterNode(methodSymbol, NodeType.Method, node, cyclomaticComplexity);


                if (methodSymbol != null)
                {
                    if (!IsVoid(methodSymbol.ReturnType))
                    {
                        _codeAnalysisResult.RegisterEdge(methodSymbol, methodSymbol.ReturnType, EdgeType.Returns);
                    }

                    foreach (IParameterSymbol parameter in methodSymbol.Parameters)
                    {
                        _codeAnalysisResult.RegisterEdge(methodSymbol, parameter.Type, EdgeType.Parameter);
                    }

                    if (methodSymbol.IsOverride)
                    {
                        _codeAnalysisResult.RegisterEdge(methodSymbol, methodSymbol.OverriddenMethod, EdgeType.Overrride);
                    }
                }
            }
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            base.VisitPropertyDeclaration(node);

            IPropertySymbol? propertySymbol = _semanticModel.GetDeclaredSymbol(node);

            if (propertySymbol != null)
            {
                _codeAnalysisResult.RegisterNode(propertySymbol, NodeType.Property, node);
            }
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            base.VisitFieldDeclaration(node);

            // Loop over all variables (multiple variables can be declared in one FieldDeclaration) e.g. 'private int x, y;'
            foreach (VariableDeclaratorSyntax variableNode in node.Declaration.Variables)
            {
                IFieldSymbol? fieldSymbol = _semanticModel.GetDeclaredSymbol(variableNode) as IFieldSymbol;
                if (fieldSymbol != null)
                {
                    _codeAnalysisResult.RegisterNode(fieldSymbol, NodeType.Field, node);
                }
            }
        }

        public override void VisitEventDeclaration(EventDeclarationSyntax node)
        {
            base.VisitEventDeclaration(node);

            IEventSymbol? eventSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (eventSymbol != null)
            {
                _codeAnalysisResult.RegisterNode(eventSymbol, NodeType.Event, node);
            }
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            base.VisitEnumDeclaration(node);

            INamedTypeSymbol? enumSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (enumSymbol != null)
            {
                _codeAnalysisResult.RegisterNode(enumSymbol, NodeType.Enum, node);

                foreach (EnumMemberDeclarationSyntax enumMemberNode in node.Members)
                {
                    IFieldSymbol? enumValueSymbol = _semanticModel.GetDeclaredSymbol(enumMemberNode);

                    if (enumValueSymbol != null)
                    {
                        _codeAnalysisResult.RegisterNode(enumValueSymbol, NodeType.EnumValue, node);
                    }
                }
            }
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            base.VisitStructDeclaration(node);

            INamedTypeSymbol? structSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (structSymbol != null)
            {
                _codeAnalysisResult.RegisterNode(structSymbol, NodeType.Struct, node);

            }
        }


        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            base.VisitVariableDeclarator(node);

            ILocalSymbol? variableSymbol = _semanticModel.GetDeclaredSymbol(node) as ILocalSymbol;
            if (variableSymbol != null)
            {
                _codeAnalysisResult.RegisterNode(variableSymbol, NodeType.Variable, node);
            }
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            base.VisitInvocationExpression(node);

            var symbolInfo = _semanticModel.GetSymbolInfo(node);

            if (symbolInfo.Symbol is IMethodSymbol calledMethodSymbol)
            {
                // This is your target method
                var targetMethod = calledMethodSymbol;

                // You can get its containing type and method name
                var targetTypeName = targetMethod.ContainingType.ToDisplayString();
                var targetMethodName = targetMethod.Name;

                // Now you have the target!
            }

            var containingMethodSyntax = node.FirstAncestorOrSelf<MethodDeclarationSyntax>();
            if (containingMethodSyntax != null)
            {
                var sourceMethodSymbol = _semanticModel.GetDeclaredSymbol(containingMethodSyntax);

                if (sourceMethodSymbol != null)
                {
                    var sourceTypeName = sourceMethodSymbol.ContainingType.ToDisplayString();
                    var sourceMethodName = sourceMethodSymbol.Name;

                    // Now you have the source!
                }
            }

            //var symbol = _semanticModel.GetSymbolInfo(node).Symbol as IMethodSymbol;
            //if (symbol == null) return;

            //// Example edge creation
            //var edge = new GraphEdge
            //{
            //    SourceId = -1, // You'd resolve real node IDs in practice
            //    TargetId = -1,
            //    Type = "Calls",
            //    Strength = 1
            //};
            //_graphBuilder.AddEdge(edge);

            //base.VisitInvocationExpression(node);
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            base.VisitIdentifierName(node);

            var symbol = _semanticModel.GetSymbolInfo(node).Symbol;

            if (symbol is ILocalSymbol localSymbol)
            {
                Console.WriteLine($"Local Variable Usage: {localSymbol.Name} in method {GetEnclosingMethodName(node)}");
            }

            if (symbol is IFieldSymbol fieldSymbol && fieldSymbol.ContainingType?.TypeKind == TypeKind.Enum)
            {
                Console.WriteLine($"Enum Field Usage: {fieldSymbol.Name} in method {GetEnclosingMethodName(node)}");
            }

            if (symbol is IFieldSymbol fieldSymbol2 && fieldSymbol2.ContainingType?.TypeKind == TypeKind.Struct)
            {
                Console.WriteLine($"Struct Field Usage (Field): {fieldSymbol2.Name} in method {GetEnclosingMethodName(node)}");
            }
            else if (symbol is IPropertySymbol propertySymbol && propertySymbol.ContainingType?.TypeKind == TypeKind.Struct)
            {
                Console.WriteLine($"Struct Field Usage (Property): {propertySymbol.Name} in method {GetEnclosingMethodName(node)}");
            }
        }

        private string GetEnclosingMethodName(SyntaxNode node)
        {
            var methodSyntax = node.FirstAncestorOrSelf<MethodDeclarationSyntax>();
            if (methodSyntax != null)
            {
                var methodSymbol = _semanticModel.GetDeclaredSymbol(methodSyntax);
                return methodSymbol?.Name ?? "<unknown method>";
            }

            return "<outside method>";
        }

        private int? CalculateCyclomaticComplexity(MethodDeclarationSyntax node)
        {
            int? cyclomaticComplexity = null;

            if (node != null)
            {
                // No body to analyze (abstract method, interface, etc.)
                if (node.Body != null || node.ExpressionBody != null)
                {
                    IOperation? operation = _semanticModel.GetOperation(node) ?? _semanticModel.GetOperation(node.Body) ?? _semanticModel.GetOperation(node.ExpressionBody);
                    IBlockOperation? blockOperation = operation as IBlockOperation;
                    if (blockOperation != null)
                    {
                        ControlFlowGraph cfg = ControlFlowGraph.Create(blockOperation);

                        if (cfg == null)
                        {
                            int nodes = cfg.Blocks.Length;
                            int edges = 0;

                            foreach (var block in cfg.Blocks)
                            {
                                if (block.ConditionalSuccessor?.Destination != null)
                                    edges++;

                                if (block.FallThroughSuccessor?.Destination != null)
                                    edges++;
                            }

                            int p = 1; // assume 1 connected component for a method
                            cyclomaticComplexity = edges - nodes + (2 * p);
                        }
                    }
                }
            }

            return cyclomaticComplexity;
        }
    }
}
