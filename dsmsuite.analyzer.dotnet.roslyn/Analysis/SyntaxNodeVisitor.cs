using dsmsuite.analyzer.dotnet.roslyn.Graph;
using dsmsuite.analyzer.dotnet.roslyn.Util;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
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

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            bool success = false;

            base.VisitNamespaceDeclaration(node);

            ISymbol? namespaceSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (namespaceSymbol != null)
            {
                _codeAnalysisResult.RegisterNode(namespaceSymbol, null, NodeType.Namespace, node);
                success = true;
            }

            _codeAnalysisResult.RegisterResult(node, success); // OK:  Failed=0/275
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            bool success = false;

            base.VisitClassDeclaration(node);

            INamedTypeSymbol? classSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (classSymbol != null)
            {
                _codeAnalysisResult.RegisterNode(classSymbol, null, NodeType.Class, node);
                success = true;

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

            _codeAnalysisResult.RegisterResult(node, success); // OK:  Failed=0/233
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            bool success = false;

            base.VisitInterfaceDeclaration(node);

            INamedTypeSymbol? interfaceSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (interfaceSymbol != null)
            {
                _codeAnalysisResult.RegisterNode(interfaceSymbol, null, NodeType.Interface, node);
                success = true;

                foreach (INamedTypeSymbol interfaceType in interfaceSymbol.Interfaces)
                {
                    _codeAnalysisResult.RegisterEdge(interfaceSymbol, interfaceType, EdgeType.Implements);
                }
            }

            _codeAnalysisResult.RegisterResult(node, success); // OK Failed=0/52
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            bool success = false;

            base.VisitMethodDeclaration(node);

            IMethodSymbol? methodSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (methodSymbol != null)
            {
                INamedTypeSymbol containingType = methodSymbol.ContainingType;
                int cyclomaticComplexity = CalculateCyclomaticComplexity(node);
                _codeAnalysisResult.RegisterNode(methodSymbol, containingType, NodeType.Method, node, cyclomaticComplexity);
                success = true;

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

            _codeAnalysisResult.RegisterResult(node, success); // OK: Failed=0/1202
        }


        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            bool success = false;

            base.VisitPropertyDeclaration(node);

            IPropertySymbol? propertySymbol = _semanticModel.GetDeclaredSymbol(node);
            if (propertySymbol != null)
            {
                INamedTypeSymbol containingType = propertySymbol.ContainingType;
                _codeAnalysisResult.RegisterNode(propertySymbol, containingType, NodeType.Property, node);
                success = true;
            }

            _codeAnalysisResult.RegisterResult(node, success); // OK: Failed=0/517
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            int variableCount = node.Declaration.Variables.Count;
            int successCount = 0;

            base.VisitFieldDeclaration(node);

            // Loop over all variables (multiple variables can be declared in one FieldDeclaration) e.g. 'private int x, y;'
            foreach (VariableDeclaratorSyntax variableNode in node.Declaration.Variables)
            {
                IFieldSymbol? fieldSymbol = _semanticModel.GetDeclaredSymbol(variableNode) as IFieldSymbol;
                if (fieldSymbol != null)
                {
                    INamedTypeSymbol containingType = fieldSymbol.ContainingType;
                    _codeAnalysisResult.RegisterNode(fieldSymbol, containingType, NodeType.Field, node);
                    successCount++;
                }
            }

            _codeAnalysisResult.RegisterResult(node, variableCount == successCount); // OK: Failed=0/59
        }

        public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            int variableCount = node.Declaration.Variables.Count;
            int successCount = 0;

            base.VisitEventFieldDeclaration(node);

            foreach (VariableDeclaratorSyntax eventField in node.Declaration.Variables)
            {
                IEventSymbol? eventFieldSymbol = _semanticModel.GetDeclaredSymbol(eventField) as IEventSymbol;

                if (eventFieldSymbol != null)
                {
                    INamedTypeSymbol containingType = eventFieldSymbol.ContainingType;
                    _codeAnalysisResult.RegisterNode(eventFieldSymbol, containingType, NodeType.Event, node);
                    successCount++;
                }
            }

            _codeAnalysisResult.RegisterResult(node, variableCount == successCount); // OK: Failed=0/32
        }

        public override void VisitEventDeclaration(EventDeclarationSyntax node)
        {
            bool success = false;

            base.VisitEventDeclaration(node);

            IEventSymbol? eventSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (eventSymbol != null)
            {
                INamedTypeSymbol containingType = eventSymbol.ContainingType;
                _codeAnalysisResult.RegisterNode(eventSymbol, containingType, NodeType.Event, node);
                success = true;
            }

            _codeAnalysisResult.RegisterResult(node, success); // TODO: Implements smaple code to analyze
        }

        public override void VisitGenericName(GenericNameSyntax node)
        {
            bool success = false;

            base.VisitGenericName(node);

            // Get the symbol information for the generic type  
            var symbolInfo = _semanticModel.GetSymbolInfo(node);
            var symbol = symbolInfo.Symbol;

            if (symbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType)
            {
                Logger.LogInfo($"Generic Type: {namedTypeSymbol.Name}"); // TODO: Implement Line = 168 Count = 64

                // Analyze type arguments  
                foreach (var typeArgument in namedTypeSymbol.TypeArguments)
                {
                    Logger.LogInfo($"Type Argument: {typeArgument.Name}"); // TODDO: Implement Line=173 Count=759
                }
            }

            _codeAnalysisResult.RegisterResult(node, success); // TOD0: Failed=645/645
        }

        public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            bool success = false;

            base.VisitObjectCreationExpression(node);

            // Get the type being instantiated  
            var typeInfo = _semanticModel.GetTypeInfo(node);
            var namedTypeSymbol = typeInfo.Type as INamedTypeSymbol;

            if (namedTypeSymbol != null && namedTypeSymbol.IsGenericType)
            {
                Logger.LogInfo($"Instantiated Generic Type: {namedTypeSymbol.Name}");

                // Analyze type arguments  
                foreach (var typeArgument in namedTypeSymbol.TypeArguments)
                {
                    Logger.LogInfo($"Type Argument: {typeArgument.Name}");
                }
            }
            _codeAnalysisResult.RegisterResult(node, success); // TODO: Failed=525/525

        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            bool success = false;
            int enumValueCount = node.Members.Count;
            int successCount = 0;

            base.VisitEnumDeclaration(node);

            INamedTypeSymbol? enumSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (enumSymbol != null)
            {
                INamedTypeSymbol containingType = enumSymbol.ContainingType;
                _codeAnalysisResult.RegisterNode(enumSymbol, containingType, NodeType.Enum, node);
                success = true;

                foreach (EnumMemberDeclarationSyntax enumMemberNode in node.Members)
                {
                    IFieldSymbol? enumValueSymbol = _semanticModel.GetDeclaredSymbol(enumMemberNode);

                    if (enumValueSymbol != null)
                    {
                        _codeAnalysisResult.RegisterNode(enumValueSymbol, enumSymbol, NodeType.EnumValue, node);
                        successCount++;
                    }
                }
            }

            _codeAnalysisResult.RegisterResult(node, success && enumValueCount == successCount); // OK: Failed=0/16
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            bool success = false;

            base.VisitStructDeclaration(node);

            INamedTypeSymbol? structSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (structSymbol != null)
            {
                _codeAnalysisResult.RegisterNode(structSymbol, null, NodeType.Struct, node);
                success = true;
            }

            _codeAnalysisResult.RegisterResult(node, success); // OK:  Failed=0/1
        }


        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            bool success = false;

            base.VisitVariableDeclarator(node);

            ISymbol? variableSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (variableSymbol != null)
            {
                INamedTypeSymbol containingType = variableSymbol.ContainingType;
                _codeAnalysisResult.RegisterNode(variableSymbol, containingType, NodeType.Variable, node);
                success = true;
            }
            else
            {
                // Temp
                var s = _semanticModel.GetDeclaredSymbol(node);
                if (s != null)
                {
                }
            }

            _codeAnalysisResult.RegisterResult(node, success); // TODO: Failed=622/2149
        }

        public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            bool success = false;

            base.VisitAssignmentExpression(node);

            IEventSymbol? eventSymbol = _semanticModel.GetSymbolInfo(node.Left).Symbol as IEventSymbol;
            ISymbol? eventHandlerSymbol = _semanticModel.GetSymbolInfo(node.Right).Symbol;
            if (eventSymbol != null)
            {
                if (eventHandlerSymbol != null)
                {
                    if (node.IsKind(SyntaxKind.AddAssignmentExpression))
                    {
                        _codeAnalysisResult.RegisterEdge(eventHandlerSymbol, eventSymbol, EdgeType.Subscribes);
                        success = true;
                    }
                    else if (node.IsKind(SyntaxKind.SubtractAssignmentExpression))
                    {
                        _codeAnalysisResult.RegisterEdge(eventHandlerSymbol, eventSymbol, EdgeType.Unsubscribes);
                        success = true;
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }
            else
            {
                success = true; // Assignement not relevant
            }

            _codeAnalysisResult.RegisterResult(node, success); // TODO: Failed=19/1169
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            bool success = false;

            base.VisitInvocationExpression(node);

            IMethodSymbol? calledMethodSymbol = _semanticModel.GetSymbolInfo(node).Symbol as IMethodSymbol;
            if (calledMethodSymbol != null)
            {
                IMethodSymbol? callingMethodSymbol = _semanticModel.GetEnclosingSymbol(node.SpanStart) as IMethodSymbol;

                if (callingMethodSymbol != null)
                {
                    if (calledMethodSymbol.MethodKind == MethodKind.DelegateInvoke)
                    {
                        IEventSymbol? eventSymbol = _semanticModel.GetSymbolInfo(node.Expression).Symbol as IEventSymbol;
                        if (eventSymbol != null)
                        {
                            _codeAnalysisResult.RegisterEdge(callingMethodSymbol, eventSymbol, EdgeType.Triggers);
                            success = true;
                        }
                    }
                    else
                    {
                        _codeAnalysisResult.RegisterEdge(callingMethodSymbol, calledMethodSymbol, EdgeType.Call);
                        success = true;
                    }
                }
                else
                {
                    var s = _semanticModel.GetEnclosingSymbol(node.SpanStart);
                    if (s != null) { }
                }
            }

            _codeAnalysisResult.RegisterResult(node, success); // TODO: Failed=55/4780
        }

        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            //var symbol = _semanticModel.GetSymbolInfo(node).Symbol;
            //if (symbol != null && _membersToTrack.Contains(symbol))
            //{
            //    Console.WriteLine($"Member {symbol.Name} accessed at {node.GetLocation().GetLineSpan()}");
            //}

            base.VisitMemberAccessExpression(node);
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            bool success = false;

            base.VisitIdentifierName(node);

            ISymbol? IdentifierNameSymbol = _semanticModel.GetSymbolInfo(node).Symbol;

            if (IdentifierNameSymbol != null)
            {
                IMethodSymbol callingMethodSymbol = _semanticModel.GetEnclosingSymbol(node.SpanStart) as IMethodSymbol;
                if (callingMethodSymbol != null)
                {
                    if (callingMethodSymbol is ILocalSymbol)
                    {
                        _codeAnalysisResult.RegisterEdge(callingMethodSymbol, IdentifierNameSymbol, EdgeType.VariableUsage);

                    }
                    else if (callingMethodSymbol is IParameterSymbol)
                    {
                        _codeAnalysisResult.RegisterEdge(callingMethodSymbol, IdentifierNameSymbol, EdgeType.ParameterUsage);
                        success = true;
                    }
                    else if (callingMethodSymbol is IFieldSymbol)
                    {
                        _codeAnalysisResult.RegisterEdge(callingMethodSymbol, IdentifierNameSymbol, EdgeType.FieldUsage);
                        success = true;
                    }
                    else
                    {

                    }
                }
                else
                {
                    ISymbol symbol = _semanticModel.GetEnclosingSymbol(node.SpanStart);
                    if (symbol.IsImplicitlyDeclared)
                    {
                        success = true;
                    }
                    else
                    {

                    }
                }
            }

            _codeAnalysisResult.RegisterResult(node, success); // TODO: Failed=25837/32693
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
