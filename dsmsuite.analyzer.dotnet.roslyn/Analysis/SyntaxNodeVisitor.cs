using dsmsuite.analyzer.dotnet.roslyn.Analysis;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
                    RegisterNode(node, namespaceSymbol, namespaceSymbol.ContainingNamespace, NodeType.Namespace);
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
        RegisterNode(node, classSymbol, parentSymbol, NodeType.Class);

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
        RegisterNode(node, structSymbol, parentSymbol, NodeType.Struct);

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
        RegisterNode(node, interfaceSymbol, parentSymbol, NodeType.Interface);

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
        RegisterNode(node, enumSymbol, parentSymbol, NodeType.Enum);

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
        RegisterNode(node, methodSymbol, parentSymbol, NodeType.Method);

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
        RegisterNode(node, constructorSymbol, parentSymbol, NodeType.Constructor);
        base.VisitConstructorDeclaration(node);
    }

    public override void VisitDestructorDeclaration(DestructorDeclarationSyntax node)
    {
        IMethodSymbol? destructorSymbol = _semanticModel.GetDeclaredSymbol(node);
        ISymbol? parentSymbol = destructorSymbol?.ContainingSymbol;
        RegisterNode(node, destructorSymbol, parentSymbol, NodeType.Destructor);
        base.VisitDestructorDeclaration(node);
    }

    public override void VisitOperatorDeclaration(OperatorDeclarationSyntax node)
    {
        IMethodSymbol? operatorSymbol = _semanticModel.GetDeclaredSymbol(node);
        ISymbol? parentSymbol = operatorSymbol?.ContainingSymbol;
        RegisterNode(node, operatorSymbol, parentSymbol, NodeType.Operator);
        base.VisitOperatorDeclaration(node);
    }

    // Property Declarations
    public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node) 
    {
        IPropertySymbol? propertySymbol = _semanticModel.GetDeclaredSymbol(node);
        ISymbol? parentSymbol = propertySymbol?.ContainingSymbol;
        RegisterNode(node, propertySymbol, parentSymbol, NodeType.Property);

        ITypeSymbol? typeSymbol = _semanticModel.GetTypeInfo(node).Type;
        RegisterEdge(node, parentSymbol, typeSymbol, EdgeType.PropertyType); // TODO: Line=182 Failed=8/8

        base.VisitPropertyDeclaration(node);
    }

    // Field Declarations
    public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        // Loop over all fields (multiple fields can be declared in one FieldDeclaration) e.g. 'private int x, y;'
        foreach (VariableDeclaratorSyntax variableNode in node.Declaration.Variables)
        {
            IFieldSymbol? fieldSymbol = _semanticModel.GetDeclaredSymbol(variableNode) as IFieldSymbol;
            ISymbol? parentSymbol = fieldSymbol?.ContainingSymbol;
            RegisterNode(node, fieldSymbol, parentSymbol, NodeType.Field);

            ITypeSymbol? typeSymbol = _semanticModel.GetTypeInfo(node).Type;
            RegisterEdge(node, parentSymbol, typeSymbol, EdgeType.FieldType); // TODO: Line=198 Failed=23/23
        }

        base.VisitFieldDeclaration(node);
    }

    public override void VisitVariableDeclaration(VariableDeclarationSyntax node) // TODO: Failed=36/59
    {
        // Loop over all variables (multiple variables can be declared in one VariableDeclarationSyntax) e.g. 'private int x, y;'
        foreach (VariableDeclaratorSyntax variableNode in node.Variables)
        {
            IFieldSymbol? variableSymbol = _semanticModel.GetDeclaredSymbol(variableNode) as IFieldSymbol;
            ISymbol? parentSymbol = variableSymbol?.ContainingSymbol;
            RegisterNode(node, variableSymbol, parentSymbol, NodeType.Variable); // TODO: Line = 211 Failed = 36 / 59

            ITypeSymbol? typeSymbol = _semanticModel.GetTypeInfo(node).Type;
            RegisterEdge(node, parentSymbol, typeSymbol, EdgeType.VariableType); // TODO: Line=214 Failed=59/59
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
            RegisterNode(node, eventFieldSymbol, parentSymbol, NodeType.Event);
        }

        base.VisitEventFieldDeclaration(node);
    }

    public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
    {
        IFieldSymbol? enumMemberSymbol = _semanticModel.GetDeclaredSymbol(node);
        ISymbol? parentSymbol = enumMemberSymbol?.ContainingSymbol;
        RegisterNode(node, enumMemberSymbol, parentSymbol, NodeType.EnumValue);
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
                RegisterEdge(node, eventSymbol, eventSymbol, EdgeType.TriggerEvent); // TODO: Line=251 Failed=1/1
            }
            else
            {
                RegisterEdge(node, caller, callee, EdgeType.Call);
            }
        }
        base.VisitInvocationExpression(node);
    }

    public override void VisitReturnStatement(ReturnStatementSyntax node)
    {
        ITypeSymbol? typeSymbol = _semanticModel.GetTypeInfo(node).Type;
        ISymbol? parentSymbol = typeSymbol?.ContainingSymbol;
        RegisterEdge(node, parentSymbol, typeSymbol, EdgeType.ReturnType); // TODO: Line=265 Failed=10/10

        base.VisitReturnStatement(node);
    }

    public override void VisitTypeParameter(TypeParameterSyntax node)
    {
        ITypeSymbol? typeSymbol = _semanticModel.GetTypeInfo(node).Type;
        ISymbol? parentSymbol = typeSymbol?.ContainingSymbol;
        RegisterEdge(node, parentSymbol, typeSymbol, EdgeType.ParameterType); // TODO: Line=274 Failed=2/2

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
            RegisterEdge(node, eventHandlerSymbol, eventSymbol, EdgeType.SubscribeEvent);
            RegisterEdge(node, parentSymbol, eventSymbol, EdgeType.HandlEvent);
        }
        else if (node.IsKind(SyntaxKind.SubtractAssignmentExpression))
        {
            RegisterEdge(node, eventHandlerSymbol, eventSymbol, EdgeType.UnsubscribeEvent);
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
    private bool RegisterNode(SyntaxNode node, 
                              ISymbol? nodeSymbol,
                              ISymbol? parent,
                              NodeType nodeType,
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
                _result.RegisterNode(nodeSymbol, parent, nodeType, node);
            }
            success = true;
        }

        _result.RegisterResult(actionDescription, node, success, sourceFile, method, lineNumber);
        return success;
    }

    private bool RegisterEdge(SyntaxNode node, 
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
}