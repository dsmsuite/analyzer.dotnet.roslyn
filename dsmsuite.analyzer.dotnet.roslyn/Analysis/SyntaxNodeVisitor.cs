using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using System.Runtime.InteropServices;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public class SyntaxNodeVisitor : CSharpSyntaxWalker
    {
        private readonly SemanticModel _semanticModel;
        private readonly ICodeAnalysisResult _codeAnalysisResult;

        public SyntaxNodeVisitor(SemanticModel semanticModel, ICodeAnalysisResult codeAnalysisResult)
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
                _codeAnalysisResult.RegisterNode(classSymbol, null, NodeType.Class, node);

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

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            base.VisitMethodDeclaration(node);

            IMethodSymbol? methodSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (methodSymbol != null)
            {
                INamedTypeSymbol containingType = methodSymbol.ContainingType;

                int cyclomaticComplexity = CalculateCyclomaticComplexity(node);

                _codeAnalysisResult.RegisterNode(methodSymbol, containingType, NodeType.Method, node, cyclomaticComplexity);

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

                    if (methodSymbol.IsOverride && methodSymbol.OverriddenMethod != null)
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
                INamedTypeSymbol containingType = propertySymbol.ContainingType;
                _codeAnalysisResult.RegisterNode(propertySymbol, containingType, NodeType.Property, node);
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
                    INamedTypeSymbol containingType = fieldSymbol.ContainingType;
                    _codeAnalysisResult.RegisterNode(fieldSymbol, containingType, NodeType.Field, node);
                }
            }
        }

        public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            base.VisitEventFieldDeclaration(node);

            foreach (VariableDeclaratorSyntax eventField in node.Declaration.Variables)
            {
                IEventSymbol? eventFieldSymbol = _semanticModel.GetDeclaredSymbol(eventField) as IEventSymbol;

                if (eventFieldSymbol != null)
                {
                    INamedTypeSymbol containingType = eventFieldSymbol.ContainingType;
                    _codeAnalysisResult.RegisterNode(eventFieldSymbol, containingType, NodeType.Event, node);
                }
            }
        }

        public override void VisitEventDeclaration(EventDeclarationSyntax node)
        {
            base.VisitEventDeclaration(node);

            IEventSymbol? eventSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (eventSymbol != null)
            {
                INamedTypeSymbol containingType = eventSymbol.ContainingType;
                _codeAnalysisResult.RegisterNode(eventSymbol, containingType, NodeType.Event, node);
            }
        }

        public override void VisitGenericName(GenericNameSyntax node)
        {
            // Get the symbol information for the generic type  
            var symbolInfo = _semanticModel.GetSymbolInfo(node);
            var symbol = symbolInfo.Symbol;

            if (symbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType)
            {
                Console.WriteLine($"Generic Type: {namedTypeSymbol.Name}");

                // Analyze type arguments  
                foreach (var typeArgument in namedTypeSymbol.TypeArguments)
                {
                    Console.WriteLine($"Type Argument: {typeArgument.Name}");
                }
            }

            base.VisitGenericName(node);
        }

        public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            // Get the type being instantiated  
            var typeInfo = _semanticModel.GetTypeInfo(node);
            var namedTypeSymbol = typeInfo.Type as INamedTypeSymbol;

            if (namedTypeSymbol != null && namedTypeSymbol.IsGenericType)
            {
                Console.WriteLine($"Instantiated Generic Type: {namedTypeSymbol.Name}");

                // Analyze type arguments  
                foreach (var typeArgument in namedTypeSymbol.TypeArguments)
                {
                    Console.WriteLine($"Type Argument: {typeArgument.Name}");
                }
            }

            base.VisitObjectCreationExpression(node);
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            base.VisitEnumDeclaration(node);

            INamedTypeSymbol? enumSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (enumSymbol != null)
            {
                INamedTypeSymbol containingType = enumSymbol.ContainingType;
                _codeAnalysisResult.RegisterNode(enumSymbol, containingType, NodeType.Enum, node);

                foreach (EnumMemberDeclarationSyntax enumMemberNode in node.Members)
                {
                    IFieldSymbol? enumValueSymbol = _semanticModel.GetDeclaredSymbol(enumMemberNode);

                    if (enumValueSymbol != null)
                    {
                        _codeAnalysisResult.RegisterNode(enumValueSymbol, enumSymbol, NodeType.EnumValue, node);
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
                _codeAnalysisResult.RegisterNode(structSymbol, null, NodeType.Struct, node);
            }
        }


        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            base.VisitVariableDeclarator(node);

            ILocalSymbol? variableSymbol = _semanticModel.GetDeclaredSymbol(node) as ILocalSymbol;
            if (variableSymbol != null)
            {
                INamedTypeSymbol containingType = variableSymbol.ContainingType;
                _codeAnalysisResult.RegisterNode(variableSymbol, containingType, NodeType.Variable, node);
            }
        }

        public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            base.VisitAssignmentExpression(node);

            ISymbol? left = _semanticModel.GetSymbolInfo(node.Left).Symbol as IEventSymbol;
            ISymbol? handler = _semanticModel.GetSymbolInfo(node.Right).Symbol;
            if (left != null && handler != null)
            {
                if (node.IsKind(SyntaxKind.AddAssignmentExpression))
                {
                    _codeAnalysisResult.RegisterEdge(handler, left, EdgeType.Subscribes);
                }

                if (node.IsKind(SyntaxKind.SubtractAssignmentExpression))
                {
                    _codeAnalysisResult.RegisterEdge(handler, left, EdgeType.Unsubscribes);
                }
            }
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            base.VisitInvocationExpression(node);

            IMethodSymbol? calledMethodSymbol = _semanticModel.GetSymbolInfo(node).Symbol as IMethodSymbol;
            if (calledMethodSymbol != null)
            {
                IMethodSymbol? containingMethod = _semanticModel.GetEnclosingSymbol(node.SpanStart) as IMethodSymbol;

                if (containingMethod != null)
                {
                    if (calledMethodSymbol.MethodKind == MethodKind.DelegateInvoke)
                    {
                        var expr = node.Expression as MemberAccessExpressionSyntax;
                        IEventSymbol? eventSymbol = _semanticModel.GetSymbolInfo(expr?.Expression).Symbol as IEventSymbol;
                            if (eventSymbol != null)
                            {
                                _codeAnalysisResult.RegisterEdge(containingMethod, eventSymbol, EdgeType.Triggers);
                            }
                    }
                    else
                    {
                        _codeAnalysisResult.RegisterEdge(containingMethod, calledMethodSymbol, EdgeType.Call);
                    }
                }
            }
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            base.VisitIdentifierName(node);
            // Get the symbol for the identifier  
            var symbolInfo = _semanticModel.GetSymbolInfo(node);
            var symbol = symbolInfo.Symbol;

            var containingMethod = _semanticModel.GetEnclosingSymbol(node.SpanStart) as IMethodSymbol;
            if (containingMethod != null)
            {
                if (symbol is ILocalSymbol)
                {
                    _codeAnalysisResult.RegisterEdge(containingMethod, symbol, EdgeType.VariableUsage);
                }
                else if (symbol is IParameterSymbol)
                {
                    _codeAnalysisResult.RegisterEdge(containingMethod, symbol, EdgeType.ParameterUsage);
                }
                else if (symbol is IFieldSymbol)
                {
                    _codeAnalysisResult.RegisterEdge(containingMethod, symbol, EdgeType.FieldUsage);
                }
            }
            else
            {
                //Console.WriteLine($"Edge caller for {calledMethodSymbol.Name} not found");
            }
        }

        private int CalculateCyclomaticComplexity(MethodDeclarationSyntax node)
        {
            int cyclomaticComplexity = 1;

            var cfg = ControlFlowGraph.Create(node, _semanticModel);
            if (cfg != null)
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


            return cyclomaticComplexity;
        }

        private bool IsVoid(ITypeSymbol typeSymbol)
        {
            return typeSymbol.SpecialType == SpecialType.System_Void;
        }
    }
}
