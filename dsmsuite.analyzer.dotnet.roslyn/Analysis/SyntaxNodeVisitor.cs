using dsmsuite.analyzer.dotnet.roslyn.Analysis;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

public class SyntaxNodeVisitor : CSharpSyntaxWalker
{
    private readonly SemanticModel _semanticModel;
    private readonly ICodeAnalysisResult _result;

    public SyntaxNodeVisitor(SemanticModel semanticModel, ICodeAnalysisResult result)
        : base(SyntaxWalkerDepth.Token)
    {
        _semanticModel = semanticModel;
        _result = result;
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
                    RegisterNodeIfNotNull(node, namespaceSymbol, namespaceSymbol.ContainingNamespace, NodeType.Namespace);
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
        RegisterNodeIfNotNull(node, classSymbol, parentSymbol, NodeType.Class);

        if (classSymbol != null)
        {
            FindBaseType(classSymbol);
            FindInterfaceTypes(classSymbol);
        }

        base.VisitClassDeclaration(node);
    }

    public override void VisitStructDeclaration(StructDeclarationSyntax node)
    {
        INamedTypeSymbol? structSymbol = _semanticModel.GetDeclaredSymbol(node);
        INamespaceSymbol? parentSymbol = structSymbol?.ContainingNamespace;
        RegisterNodeIfNotNull(node, structSymbol, parentSymbol, NodeType.Struct);

        if (structSymbol != null)
        {
            FindBaseType(structSymbol);
            FindInterfaceTypes(structSymbol);
        }

        base.VisitStructDeclaration(node);
    }

    public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
    {
        INamedTypeSymbol? interfaceSymbol = _semanticModel.GetDeclaredSymbol(node);
        INamespaceSymbol? parentSymbol = interfaceSymbol?.ContainingNamespace;
        RegisterNodeIfNotNull(node, interfaceSymbol, parentSymbol, NodeType.Interface);

        if (interfaceSymbol != null)
        {
            FindInterfaceTypes(interfaceSymbol);
        }

        base.VisitInterfaceDeclaration(node);
    }

    public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
    {
        INamedTypeSymbol? enumSymbol = _semanticModel.GetDeclaredSymbol(node);
        INamespaceSymbol? parentSymbol = enumSymbol?.ContainingNamespace;
        RegisterNodeIfNotNull(node, enumSymbol, parentSymbol, NodeType.Enum);

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
    //public override void VisitIdentifierName(IdentifierNameSyntax node) => RegisterTypeReference(node);
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
        RegisterNodeIfNotNull(node, methodSymbol, parentSymbol, NodeType.Method, cyclomaticComplexity);

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
        RegisterNodeIfNotNull(node, constructorSymbol, parentSymbol, NodeType.Constructor);
        base.VisitConstructorDeclaration(node);
    }

    public override void VisitDestructorDeclaration(DestructorDeclarationSyntax node)
    {
        IMethodSymbol? destructorSymbol = _semanticModel.GetDeclaredSymbol(node);
        ISymbol? parentSymbol = destructorSymbol?.ContainingSymbol;
        RegisterNodeIfNotNull(node, destructorSymbol, parentSymbol, NodeType.Destructor);
        base.VisitDestructorDeclaration(node);
    }

    public override void VisitOperatorDeclaration(OperatorDeclarationSyntax node)
    {
        IMethodSymbol? operatorSymbol = _semanticModel.GetDeclaredSymbol(node);
        ISymbol? parentSymbol = operatorSymbol?.ContainingSymbol;
        RegisterNodeIfNotNull(node, operatorSymbol, parentSymbol, NodeType.Operator);
        base.VisitOperatorDeclaration(node);
    }

    // Property Declarations
    public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node) 
    {
        IPropertySymbol? propertySymbol = _semanticModel.GetDeclaredSymbol(node);
        ISymbol? parentSymbol = propertySymbol?.ContainingSymbol;
        RegisterNodeIfNotNull(node, propertySymbol, parentSymbol, NodeType.Property);

        ITypeSymbol? typeSymbol = _semanticModel.GetTypeInfo(node.Type).Type;
        RegisterEdgeIfNotNull(node, parentSymbol, typeSymbol, EdgeType.PropertyType); 

        base.VisitPropertyDeclaration(node);
    }

    // Field Declarations
    public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        foreach (VariableDeclaratorSyntax variableNode in node.Declaration.Variables)
        {
            IFieldSymbol? fieldSymbol = _semanticModel.GetDeclaredSymbol(variableNode) as IFieldSymbol;
            ISymbol? parentSymbol = fieldSymbol.ContainingSymbol;
            RegisterNodeIfNotNull(variableNode, fieldSymbol, parentSymbol, NodeType.Field);

            ITypeSymbol? typeSymbol = _semanticModel.GetTypeInfo(node.Declaration.Type).Type;
            RegisterEdgeIfNotNull(variableNode, parentSymbol, typeSymbol, EdgeType.FieldType);
        }

        base.VisitFieldDeclaration(node);
    }

    // Vafaiable Declarations
    public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
    {
        foreach (VariableDeclaratorSyntax variableNode in node.Variables)
        {
            ILocalSymbol? variableSymbol = _semanticModel.GetDeclaredSymbol(variableNode) as ILocalSymbol;
            ISymbol? parentSymbol = variableSymbol?.ContainingSymbol;
            RegisterNodeIfNotNull(variableNode, variableSymbol, parentSymbol, NodeType.Variable); //  Line=212 Failed=36/59

            ITypeSymbol? typeSymbol = _semanticModel.GetTypeInfo(node.Type).Type;
            RegisterEdgeIfNotNull(variableNode, parentSymbol, typeSymbol, EdgeType.VariableType); // Line=215 Failed=36/59
        }

        base.VisitVariableDeclaration(node);
    }


    // Event Declaratioms
    public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
    {
        foreach (VariableDeclaratorSyntax eventField in node.Declaration.Variables)
        {
            IEventSymbol? eventFieldSymbol = _semanticModel.GetDeclaredSymbol(eventField) as IEventSymbol;
            ISymbol? parentSymbol = eventFieldSymbol?.ContainingSymbol;
            RegisterNodeIfNotNull(node, eventFieldSymbol, parentSymbol, NodeType.Event);
        }

        base.VisitEventFieldDeclaration(node);
    }

    public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
    {
        IFieldSymbol? enumMemberSymbol = _semanticModel.GetDeclaredSymbol(node);
        ISymbol? parentSymbol = enumMemberSymbol?.ContainingSymbol;
        RegisterNodeIfNotNull(node, enumMemberSymbol, parentSymbol, NodeType.EnumValue);
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
                IEventSymbol? eventSymbol = _semanticModel.GetSymbolInfo(node.Expression).Symbol as IEventSymbol;
                RegisterEdgeIfNotNull(node, caller, eventSymbol, EdgeType.TriggerEvent); //Line=253 Failed=1/1
            }
            else
            {
                RegisterEdgeIfNotNull(node, caller, callee, EdgeType.Call);
            }
        }
        base.VisitInvocationExpression(node);
    }

    public override void VisitReturnStatement(ReturnStatementSyntax node)
    {
        ITypeSymbol? returnTypeSymbol = _semanticModel.GetTypeInfo(node.Expression).Type;
        ISymbol? parentSymbol = _semanticModel.GetEnclosingSymbol(node.SpanStart);
        RegisterEdgeIfNotNull(node, parentSymbol, returnTypeSymbol, EdgeType.ReturnType);

        base.VisitReturnStatement(node);
    }

    public override void VisitTypeParameter(TypeParameterSyntax node)
    {
        ITypeParameterSymbol? typeParameterSymbol = _semanticModel.GetDeclaredSymbol(node) as ITypeParameterSymbol;
        ISymbol? parentSymbol = typeParameterSymbol?.ContainingSymbol;
        RegisterNodeIfNotNull(node, typeParameterSymbol, parentSymbol, NodeType.TypeParameter);

        if (typeParameterSymbol != null)
        {
            foreach (var constraint in typeParameterSymbol.ConstraintTypes)
            {
                RegisterEdgeIfNotNull(node, typeParameterSymbol, constraint, EdgeType.TemplateParameter);
            }
        }

        base.VisitTypeParameter(node);
    }

    // Event handling
    public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
    {
        IEventSymbol? eventSymbol = _semanticModel.GetSymbolInfo(node.Left).Symbol as IEventSymbol;
        ISymbol? eventHandlerSymbol = _semanticModel.GetSymbolInfo(node.Right).Symbol;
        ISymbol? parentSymbol = eventSymbol?.ContainingSymbol;
        if (node.IsKind(SyntaxKind.AddAssignmentExpression))
        {
            RegisterEdgeIfNotNull(node, eventHandlerSymbol, eventSymbol, EdgeType.SubscribeEvent);
            RegisterEdgeIfNotNull(node, parentSymbol, eventSymbol, EdgeType.HandlEvent);
        }
        else if (node.IsKind(SyntaxKind.SubtractAssignmentExpression))
        {
            RegisterEdgeIfNotNull(node, eventHandlerSymbol, eventSymbol, EdgeType.UnsubscribeEvent);
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
    private bool RegisterNodeIfNotNull(SyntaxNode node, 
                              ISymbol? nodeSymbol,
                              ISymbol? parent,
                              NodeType nodeType,
                              int cyclomaticComplexity = 0,
                              [CallerFilePath] string sourceFile = "",
                              [CallerMemberName] string method = "",
                              [CallerLineNumber] int lineNumber = 0)
    {
        bool success = false;
        string actionDescription = $"Parse node={nodeType}";
        if (nodeSymbol != null)
        {
            if (!_result.IsNodeRegistered(nodeSymbol))
            {
                _result.RegisterNode(nodeSymbol, parent, nodeType, node, cyclomaticComplexity);
            }
            success = true;
        }

        _result.RegisterResult(actionDescription, node, success, sourceFile, method, lineNumber);
        return success;
    }

    private bool RegisterEdgeIfNotNull(SyntaxNode node, 
                               ISymbol? edgeSource,
                               ISymbol? edgeTarget,
                               EdgeType edgeType,
                               [CallerFilePath] string sourceFile = "",
                               [CallerMemberName] string method = "",
                               [CallerLineNumber] int lineNumber = 0)
    {
        bool success = false;
        string actionDescription = $"Parse edge={edgeType}";

        if (edgeSource != null && edgeTarget != null)
        {
            _result.RegisterEdge(edgeSource, edgeTarget, edgeType);
            success = true;
        }

        _result.RegisterResult(actionDescription, node, success, sourceFile, method, lineNumber);

        return success;
    }

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

    private void FindBaseType(INamedTypeSymbol symbol)
    {
        if (symbol.BaseType != null && symbol.BaseType.SpecialType != SpecialType.System_Object)
        {
            _result.RegisterEdge(symbol, symbol.BaseType, EdgeType.InheritsFrom);
        }
    }

    private void FindInterfaceTypes(INamedTypeSymbol symbol)
    {
        foreach (var iface in symbol.Interfaces)
        {
            _result.RegisterEdge(symbol, iface, EdgeType.Implements);
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