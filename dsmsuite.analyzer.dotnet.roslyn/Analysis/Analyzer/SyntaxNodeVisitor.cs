using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;

public class SyntaxNodeVisitor : CSharpSyntaxWalker
{
    private readonly SemanticModel _semanticModel;
    private readonly IHierarchicalGraphBuilder _hierarchicalGraphBuilder;

    public SyntaxNodeVisitor(SemanticModel semanticModel, IHierarchicalGraphBuilder hierarchicalGraphBuilder)
        : base(SyntaxWalkerDepth.Token)
    {
        _semanticModel = semanticModel;
        _hierarchicalGraphBuilder = hierarchicalGraphBuilder;
    }

    //Visit namespaces
    public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
        INamespaceSymbol? namespaceSymbol = _semanticModel.GetDeclaredSymbol(node);

        do
        {
            if (namespaceSymbol != null)
            {
                if (!namespaceSymbol.IsGlobalNamespace)
                {
                    _hierarchicalGraphBuilder.AddNode(node, namespaceSymbol, namespaceSymbol.ContainingNamespace, NodeType.Namespace);
                }
                namespaceSymbol = namespaceSymbol.ContainingNamespace;
            }
        } while (namespaceSymbol != null);

        base.VisitNamespaceDeclaration(node);
    }

    /// <summary>
    /// ileScopedNamespaceDeclarationSyntax is a type introduced in C# 10 that represents file-scoped namespaces in the Roslyn syntax model. 
    /// File-scoped namespaces are a new syntax feature that simplifies namespace declarations by allowing you to declare
    /// a namespace for an entire file without using nested braces.
    /// </summary>
    /// TODO: Implement
    //public override void VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node)
    //{
    //    RegisterSymbol(node, NodeType.Namespace);
    //    base.VisitFileScopedNamespaceDeclaration(node);
    //}

    //// Type Declarations
    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        INamedTypeSymbol? classSymbol = _semanticModel.GetDeclaredSymbol(node);
        INamespaceSymbol? parentSymbol = classSymbol?.ContainingNamespace;
        _hierarchicalGraphBuilder.AddNode(node, classSymbol, parentSymbol, NodeType.Class);

        if (classSymbol != null)
        {
            FindBaseType(node, classSymbol);
            FindInterfaceTypes(node, classSymbol);
        }

        base.VisitClassDeclaration(node);
    }

    public override void VisitStructDeclaration(StructDeclarationSyntax node)
    {
        INamedTypeSymbol? structSymbol = _semanticModel.GetDeclaredSymbol(node);
        INamespaceSymbol? parentSymbol = structSymbol?.ContainingNamespace;
        _hierarchicalGraphBuilder.AddNode(node, structSymbol, parentSymbol, NodeType.Struct);

        if (structSymbol != null)
        {
            FindBaseType(node, structSymbol);
            FindInterfaceTypes(node, structSymbol);
        }

        base.VisitStructDeclaration(node);
    }

    public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
    {
        INamedTypeSymbol? interfaceSymbol = _semanticModel.GetDeclaredSymbol(node);
        INamespaceSymbol? parentSymbol = interfaceSymbol?.ContainingNamespace;
        _hierarchicalGraphBuilder.AddNode(node, interfaceSymbol, parentSymbol, NodeType.Interface);

        if (interfaceSymbol != null)
        {
            FindInterfaceTypes(node, interfaceSymbol);
        }

        base.VisitInterfaceDeclaration(node);
    }

    public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
    {
        INamedTypeSymbol? enumSymbol = _semanticModel.GetDeclaredSymbol(node);
        INamespaceSymbol? parentSymbol = enumSymbol?.ContainingNamespace;
        _hierarchicalGraphBuilder.AddNode(node, enumSymbol, parentSymbol, NodeType.Enum);

        base.VisitEnumDeclaration(node);
    }

    /// <summary>
    /// RecordDeclarationSyntax is a type in the Roslyn syntax model that represents the syntax of a record declaration in C#. 
    /// Records are a feature introduced in C# 9 that provide a concise way to define immutable data types with built-in 
    /// functionality for value equality, and they can be thought of as lightweight classes primarily used for storing data.
    /// </summary>
    /// TODO: Implement 
    //public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
    //{
    //    var symbol = RegisterSymbol(node, NodeType.Record) as INamedTypeSymbol;
    //    RegisterBaseTypes(symbol);
    //    base.VisitRecordDeclaration(node);
    //}



    //// Type Names
    public override void VisitIdentifierName(IdentifierNameSyntax node)
    {
        //var symbol = _semanticModel.GetSymbolInfo(node).Symbol;
        //var context = _semanticModel.GetEnclosingSymbol(node.SpanStart);
        //_hierarchicalGraphBuilder.AddEdge(node, context, symbol, EdgeType.TypeUsage);

        base.VisitIdentifierName(node);
    }

    //public override void VisitQualifiedName(QualifiedNameSyntax node) => RegisterTypeReference(node);
    //public override void VisitGenericName(GenericNameSyntax node) => RegisterTypeReference(node);
    //public override void VisitPredefinedType(PredefinedTypeSyntax node) => RegisterTypeReference(node);
    //public override void VisitArrayType(ArrayTypeSyntax node) => RegisterTypeReference(node); // Failed=6/49
    //public override void VisitPointerType(PointerTypeSyntax node) => RegisterTypeReference(node);
    //public override void VisitNullableType(NullableTypeSyntax node) => RegisterTypeReference(node);
    //public override void VisitTupleType(TupleTypeSyntax node) => RegisterTypeReference(node);
    //public override void VisitAliasQualifiedName(AliasQualifiedNameSyntax node) => RegisterTypeReference(node);

    //// Method declarations
    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        IMethodSymbol? methodSymbol = _semanticModel.GetDeclaredSymbol(node);
        ISymbol? parentSymbol = methodSymbol?.ContainingSymbol;
        int cyclomaticComplexity = CalculateCyclomaticComplexity(node);
        _hierarchicalGraphBuilder.AddNode(node, methodSymbol, parentSymbol, NodeType.Method, cyclomaticComplexity);

        if (methodSymbol != null)
        {
            if (methodSymbol.IsOverride && methodSymbol.OverriddenMethod != null)
            {
                _hierarchicalGraphBuilder.AddEdge(node, methodSymbol, methodSymbol.OverriddenMethod, EdgeType.Overrride);
            }
        }

        base.VisitMethodDeclaration(node);
    }

    //public override void VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
    //{
    //    RegisterSymbol(node, NodeType.LocalFunction);
    //    base.VisitLocalFunctionStatement(node);
    //}

    public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
    {
        IMethodSymbol? constructorSymbol = _semanticModel.GetDeclaredSymbol(node);
        ISymbol? parentSymbol = constructorSymbol?.ContainingSymbol;
        _hierarchicalGraphBuilder.AddNode(node, constructorSymbol, parentSymbol, NodeType.Constructor);
        base.VisitConstructorDeclaration(node);
    }

    public override void VisitDestructorDeclaration(DestructorDeclarationSyntax node)
    {
        IMethodSymbol? destructorSymbol = _semanticModel.GetDeclaredSymbol(node);
        ISymbol? parentSymbol = destructorSymbol?.ContainingSymbol;
        _hierarchicalGraphBuilder.AddNode(node, destructorSymbol, parentSymbol, NodeType.Destructor);
        base.VisitDestructorDeclaration(node);
    }

    public override void VisitOperatorDeclaration(OperatorDeclarationSyntax node)
    {
        IMethodSymbol? operatorSymbol = _semanticModel.GetDeclaredSymbol(node);
        ISymbol? parentSymbol = operatorSymbol?.ContainingSymbol;
        _hierarchicalGraphBuilder.AddNode(node, operatorSymbol, parentSymbol, NodeType.Operator);
        base.VisitOperatorDeclaration(node);
    }

    // Property Declarations
    public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        IPropertySymbol? propertySymbol = _semanticModel.GetDeclaredSymbol(node);
        ISymbol? parentSymbol = propertySymbol?.ContainingSymbol;
        _hierarchicalGraphBuilder.AddNode(node, propertySymbol, parentSymbol, NodeType.Property);

        //ITypeSymbol? typeSymbol = _semanticModel.GetTypeInfo(node.Type).Type;
        //_hierarchicalGraphBuilder.AddEdge(node, parentSymbol, typeSymbol, EdgeType.PropertyType);

        base.VisitPropertyDeclaration(node);
    }

    // Field Declarations
    public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        foreach (VariableDeclaratorSyntax variableNode in node.Declaration.Variables)
        {
            IFieldSymbol? fieldSymbol = _semanticModel.GetDeclaredSymbol(variableNode) as IFieldSymbol;
            ISymbol? parentSymbol = fieldSymbol.ContainingSymbol;
            _hierarchicalGraphBuilder.AddNode(variableNode, fieldSymbol, parentSymbol, NodeType.Field);

        //    ITypeSymbol? typeSymbol = _semanticModel.GetTypeInfo(node.Declaration.Type).Type;
        //    _hierarchicalGraphBuilder.AddEdge(variableNode, parentSymbol, typeSymbol, EdgeType.FieldType);
        }

        base.VisitFieldDeclaration(node);
    }

    // Vafaiable Declarations
    public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
    {
        //foreach (VariableDeclaratorSyntax variableNode in node.Variables)
        //{
        //    ILocalSymbol? variableSymbol = _semanticModel.GetDeclaredSymbol(variableNode) as ILocalSymbol;
        //    ISymbol? parentSymbol = variableSymbol?.ContainingSymbol;
        //    _hierarchicalGraphBuilder.AddNode(variableNode, variableSymbol, parentSymbol, NodeType.Variable); //   Line=218 Failed=26/59

        //    ITypeSymbol? typeSymbol = _semanticModel.GetTypeInfo(node.Type).Type;
        //    _hierarchicalGraphBuilder.AddEdge(variableNode, parentSymbol, typeSymbol, EdgeType.VariableType); // Line=221 Failed=26/59
        //}

        base.VisitVariableDeclaration(node);
    }


    // Event Declaratioms
    public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
    {
        foreach (VariableDeclaratorSyntax eventField in node.Declaration.Variables)
        {
            IEventSymbol? eventFieldSymbol = _semanticModel.GetDeclaredSymbol(eventField) as IEventSymbol;
            ISymbol? parentSymbol = eventFieldSymbol?.ContainingSymbol;
            _hierarchicalGraphBuilder.AddNode(node, eventFieldSymbol, parentSymbol, NodeType.Event);
        }

        base.VisitEventFieldDeclaration(node);
    }

    public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
    {
        IFieldSymbol? enumMemberSymbol = _semanticModel.GetDeclaredSymbol(node);
        ISymbol? parentSymbol = enumMemberSymbol?.ContainingSymbol;
        _hierarchicalGraphBuilder.AddNode(node, enumMemberSymbol, parentSymbol, NodeType.EnumValue);
        base.VisitEnumMemberDeclaration(node);
    }

    // Method calls
    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        IMethodSymbol? callee = _semanticModel.GetSymbolInfo(node).Symbol as IMethodSymbol;
        ISymbol? caller = _semanticModel.GetEnclosingSymbol(node.SpanStart);
        if (callee != null && caller != null)
        {
            if (callee.MethodKind == MethodKind.DelegateInvoke)
            {
                // TODO: Implement
            }
            else
            {
                _hierarchicalGraphBuilder.AddEdge(node, caller, callee, EdgeType.Call);

                if (callee.IsOverride && callee.OverriddenMethod != null)
                {
                    _hierarchicalGraphBuilder.AddEdge(node, callee, callee.OverriddenMethod, EdgeType.Overrride);
                }
            }
        }
        base.VisitInvocationExpression(node);
    }

    public override void VisitReturnStatement(ReturnStatementSyntax node)
    {
        ITypeSymbol? returnTypeSymbol = _semanticModel.GetTypeInfo(node.Expression).Type;
        ISymbol? parentSymbol = _semanticModel.GetEnclosingSymbol(node.SpanStart);
        _hierarchicalGraphBuilder.AddEdge(node, parentSymbol, returnTypeSymbol, EdgeType.ReturnType);

        base.VisitReturnStatement(node);
    }

    public override void VisitTypeParameter(TypeParameterSyntax node)
    {
        ITypeParameterSymbol? typeParameterSymbol = _semanticModel.GetDeclaredSymbol(node) as ITypeParameterSymbol;
        ISymbol? parentSymbol = typeParameterSymbol?.ContainingSymbol;
        _hierarchicalGraphBuilder.AddNode(node, typeParameterSymbol, parentSymbol, NodeType.TypeParameter);

        if (typeParameterSymbol != null)
        {
            foreach (var constraint in typeParameterSymbol.ConstraintTypes)
            {
                _hierarchicalGraphBuilder.AddEdge(node, typeParameterSymbol, constraint, EdgeType.TemplateParameter);
            }
        }

        base.VisitTypeParameter(node);
    }

    // Event handling
    public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
    {
        IEventSymbol? eventSymbol = _semanticModel.GetSymbolInfo(node.Left).Symbol as IEventSymbol;
        IEnumerable<ISymbol> eventHandlerSymbols = _semanticModel.GetSymbolInfo(node.Right).CandidateSymbols;
        ISymbol? parentSymbol = eventSymbol?.ContainingSymbol;

        if (node.IsKind(SyntaxKind.AddAssignmentExpression))
        {
            foreach(ISymbol eventHandlerSymbol in eventHandlerSymbols)
            {
                _hierarchicalGraphBuilder.AddEdge(node, eventHandlerSymbol, eventSymbol, EdgeType.HandlEvent);
            }
            _hierarchicalGraphBuilder.AddEdge(node, parentSymbol, eventSymbol, EdgeType.SubscribeEvent);
        }
        else if (node.IsKind(SyntaxKind.SubtractAssignmentExpression))
        {
            foreach (ISymbol eventHandlerSymbol in eventHandlerSymbols)
            {
                _hierarchicalGraphBuilder.AddEdge(node, eventHandlerSymbol, eventSymbol, EdgeType.HandlEvent);
            }
            _hierarchicalGraphBuilder.AddEdge(node, parentSymbol, eventSymbol, EdgeType.UnsubscribeEvent);
        }

        base.VisitAssignmentExpression(node);
    }

    //public override void VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
    //{
    //    RegisterSymbol(node, NodeType.ConversionOperator);
    //    base.VisitConversionOperatorDeclaration(node);
    //}

    //// Directives
    //public override void VisitUsingDirective(UsingDirectiveSyntax node) //  Failed=1404/1404
    //{
    //    RegisterSymbol(node.Name, NodeType.Using);
    //    base.VisitUsingDirective(node);
    //}

    //public override void VisitExternAliasDirective(ExternAliasDirectiveSyntax node)
    //{
    //    // No symbol resolution for alias name
    //    base.VisitExternAliasDirective(node);
    //}

    //public override void VisitReferenceDirectiveTrivia(ReferenceDirectiveTriviaSyntax node)
    //{
    //    // Reference directives are typically handled in metadata, not symbol model
    //    base.VisitReferenceDirectiveTrivia(node);
    //}



    //public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node) // Failed=7707/7707
    //{
    //    var fullExpression = node.ToString();               // e.g. Console.WriteLine
    //    var left = node.Expression.ToString();              // e.g. Console
    //    var right = node.Name.Identifier.Text;              // e.g. WriteLine
    //    // e.g fileInfo.Exists()
    //    Console.WriteLine($"Member Access: {fullExpression}");
    //    Console.WriteLine($"  - Object/Type: {left}");
    //    Console.WriteLine($"  - Member: {right}");

    //    RegisterSymbol(node, NodeType.MemberAccess);
    //    base.VisitMemberAccessExpression(node);
    //}

    //public override void VisitElementAccessExpression(ElementAccessExpressionSyntax node) // Failed=624/624
    //{
    //    // ElementAccessExpressionSyntax, which represents array or indexer access like:
    //    Console.WriteLine($"Element Access: {node}");
    //    Console.WriteLine($"  - Target: {node.Expression}");

    //    foreach (var arg in node.ArgumentList.Arguments)
    //    {
    //        Console.WriteLine($"  - Index/Argument: {arg.Expression}");
    //    }

    //    base.VisitElementAccessExpression(node);

    //    RegisterSymbol(node, NodeType.ElementAccess);
    //    base.VisitElementAccessExpression(node);
    //}

    //public override void VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
    //{
    //    base.VisitConditionalAccessExpression(node);
    //}

    //// Attributes
    //public override void VisitAttribute(AttributeSyntax node) // Failed=825/825
    //{
    //    RegisterSymbol(node, NodeType.Attribute);
    //    base.VisitAttribute(node);
    //}

    //public override void VisitAttributeList(AttributeListSyntax node) 
    //{
    //    base.VisitAttributeList(node);
    //}

    //// Type Parameters & Constraints
    //public override void VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node)
    //{
    //    base.VisitTypeParameterConstraintClause(node);
    //}

    //public override void VisitTypeParameter(TypeParameterSyntax node)
    //{
    //    RegisterSymbol(node, NodeType.TypeParameter);
    //    base.VisitTypeParameter(node);
    //}

    //// Helper Methods


    //private void RegisterTypeReference(TypeSyntax node,
    //                                [CallerFilePath] string sourceFile = "",
    //                                [CallerMemberName] string method = "",
    //                                [CallerLineNumber] int lineNumber = 0)
    //{
    //    bool success = false;
    //    var symbol = _semanticModel.GetSymbolInfo(node).Symbol;
    //    if (symbol != null)
    //    {
    //        var context = _semanticModel.GetEnclosingSymbol(node.SpanStart);
    //        if (context != null)
    //        {
    //            _result.RegisterEdge(context, symbol, EdgeType.TypeUsage);
    //            success = true;
    //        }
    //    }
    //    _result.RegisterResult(node, success, sourceFile, method, lineNumber);
    //}

    private void FindBaseType(SyntaxNode node, INamedTypeSymbol symbol)
    {
        if (symbol.BaseType != null && symbol.BaseType.SpecialType != SpecialType.System_Object)
        {
            _hierarchicalGraphBuilder.AddEdge(node, symbol, symbol.BaseType, EdgeType.InheritsFrom);
        }
    }

    private void FindInterfaceTypes(SyntaxNode node, INamedTypeSymbol symbol)
    {
        foreach (var iface in symbol.Interfaces)
        {
            _hierarchicalGraphBuilder.AddEdge(node, symbol, iface, EdgeType.Implements);
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
}