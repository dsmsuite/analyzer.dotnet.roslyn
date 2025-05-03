using dsmsuite.analyzer.dotnet.roslyn.Analysis;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using System.Linq;
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

    // Namespace Declarations
    public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
        RegisterSymbol(node, NodeType.Namespace);
        base.VisitNamespaceDeclaration(node);
    }

    public override void VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node)
    {
        RegisterSymbol(node, NodeType.Namespace);
        base.VisitFileScopedNamespaceDeclaration(node);
    }

    // Type Declarations
    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        var symbol = RegisterSymbol(node, NodeType.Class) as INamedTypeSymbol;
        RegisterBaseTypes(symbol);
        base.VisitClassDeclaration(node);
    }

    public override void VisitStructDeclaration(StructDeclarationSyntax node)
    {
        var symbol = RegisterSymbol(node, NodeType.Struct) as INamedTypeSymbol;
        RegisterBaseTypes(symbol);
        base.VisitStructDeclaration(node);
    }

    public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
    {
        var symbol = RegisterSymbol(node, NodeType.Interface) as INamedTypeSymbol;
        RegisterBaseTypes(symbol);
        base.VisitInterfaceDeclaration(node);
    }

    public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
    {
        var symbol = RegisterSymbol(node, NodeType.Record) as INamedTypeSymbol;
        RegisterBaseTypes(symbol);
        base.VisitRecordDeclaration(node);
    }

    public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
    {
        RegisterSymbol(node, NodeType.Enum);
        base.VisitEnumDeclaration(node);
    }

    // Type Names
    public override void VisitIdentifierName(IdentifierNameSyntax node) => RegisterTypeReference(node);
    public override void VisitQualifiedName(QualifiedNameSyntax node) => RegisterTypeReference(node);
    public override void VisitGenericName(GenericNameSyntax node) => RegisterTypeReference(node);
    public override void VisitPredefinedType(PredefinedTypeSyntax node) => RegisterTypeReference(node);
    public override void VisitArrayType(ArrayTypeSyntax node) => RegisterTypeReference(node); // Failed=6/49
    public override void VisitPointerType(PointerTypeSyntax node) => RegisterTypeReference(node);
    public override void VisitNullableType(NullableTypeSyntax node) => RegisterTypeReference(node);
    public override void VisitTupleType(TupleTypeSyntax node) => RegisterTypeReference(node);
    public override void VisitAliasQualifiedName(AliasQualifiedNameSyntax node) => RegisterTypeReference(node);

    // Function Declarations
    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        RegisterSymbol(node, NodeType.Method);
        base.VisitMethodDeclaration(node);
    }

    public override void VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
    {
        RegisterSymbol(node, NodeType.LocalFunction);
        base.VisitLocalFunctionStatement(node);
    }

    public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
    {
        RegisterSymbol(node, NodeType.Constructor);
        base.VisitConstructorDeclaration(node);
    }

    public override void VisitDestructorDeclaration(DestructorDeclarationSyntax node)
    {
        RegisterSymbol(node, NodeType.Destructor);
        base.VisitDestructorDeclaration(node);
    }

    public override void VisitOperatorDeclaration(OperatorDeclarationSyntax node)
    {
        RegisterSymbol(node, NodeType.Operator);
        base.VisitOperatorDeclaration(node);
    }

    public override void VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
    {
        RegisterSymbol(node, NodeType.ConversionOperator);
        base.VisitConversionOperatorDeclaration(node);
    }

    // Directives
    public override void VisitUsingDirective(UsingDirectiveSyntax node) //  Failed=1404/1404
    {
        RegisterSymbol(node.Name, NodeType.Using);
        base.VisitUsingDirective(node);
    }

    public override void VisitExternAliasDirective(ExternAliasDirectiveSyntax node)
    {
        // No symbol resolution for alias name
        base.VisitExternAliasDirective(node);
    }

    public override void VisitReferenceDirectiveTrivia(ReferenceDirectiveTriviaSyntax node)
    {
        // Reference directives are typically handled in metadata, not symbol model
        base.VisitReferenceDirectiveTrivia(node);
    }

    // Expressions
    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        var symbol = _semanticModel.GetSymbolInfo(node).Symbol as IMethodSymbol;
        var caller = _semanticModel.GetEnclosingSymbol(node.SpanStart);
        if (symbol != null && caller != null)
        {
            _result.RegisterEdge(caller, symbol, EdgeType.Calls);
        }
        base.VisitInvocationExpression(node);
    }

    public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node) // Failed=7707/7707
    {
        var fullExpression = node.ToString();               // e.g. Console.WriteLine
        var left = node.Expression.ToString();              // e.g. Console
        var right = node.Name.Identifier.Text;              // e.g. WriteLine
        // e.g fileInfo.Exists()
        Console.WriteLine($"Member Access: {fullExpression}");
        Console.WriteLine($"  - Object/Type: {left}");
        Console.WriteLine($"  - Member: {right}");

        RegisterSymbol(node, NodeType.MemberAccess);
        base.VisitMemberAccessExpression(node);
    }

    public override void VisitElementAccessExpression(ElementAccessExpressionSyntax node) // Failed=624/624
    {
        // ElementAccessExpressionSyntax, which represents array or indexer access like:
        Console.WriteLine($"Element Access: {node}");
        Console.WriteLine($"  - Target: {node.Expression}");

        foreach (var arg in node.ArgumentList.Arguments)
        {
            Console.WriteLine($"  - Index/Argument: {arg.Expression}");
        }

        base.VisitElementAccessExpression(node);

        RegisterSymbol(node, NodeType.ElementAccess);
        base.VisitElementAccessExpression(node);
    }

    public override void VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
    {
        base.VisitConditionalAccessExpression(node);
    }

    // Attributes
    public override void VisitAttribute(AttributeSyntax node) // Failed=825/825
    {
        RegisterSymbol(node, NodeType.Attribute);
        base.VisitAttribute(node);
    }

    public override void VisitAttributeList(AttributeListSyntax node) 
    {
        base.VisitAttributeList(node);
    }

    // Type Parameters & Constraints
    public override void VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node)
    {
        base.VisitTypeParameterConstraintClause(node);
    }

    public override void VisitTypeParameter(TypeParameterSyntax node)
    {
        RegisterSymbol(node, NodeType.TypeParameter);
        base.VisitTypeParameter(node);
    }

    // Helper Methods
    private ISymbol? RegisterSymbol(SyntaxNode node,
                                    NodeType type,
                                    [CallerFilePath] string sourceFile = "",
                                    [CallerMemberName] string method = "",
                                    [CallerLineNumber] int lineNumber = 0)
    {
        bool success = false;
        var symbol = _semanticModel.GetDeclaredSymbol(node);
        if (symbol != null)
        {
            _result.RegisterNode(symbol, symbol.ContainingSymbol, type, node);
            success = true;
        }

        _result.RegisterResult(node, success, sourceFile, method, lineNumber); 
        return symbol;
    }

    private void RegisterTypeReference(TypeSyntax node,
                                    [CallerFilePath] string sourceFile = "",
                                    [CallerMemberName] string method = "",
                                    [CallerLineNumber] int lineNumber = 0)
    {
        bool success = false;
        var symbol = _semanticModel.GetSymbolInfo(node).Symbol;
        if (symbol != null)
        {
            var context = _semanticModel.GetEnclosingSymbol(node.SpanStart);
            if (context != null)
            {
                _result.RegisterEdge(context, symbol, EdgeType.TypeUsage);
                success = true;
            }
        }
        _result.RegisterResult(node, success, sourceFile, method, lineNumber);
    }

    private void RegisterBaseTypes(INamedTypeSymbol? symbol)
    {
        if (symbol == null) return;

        if (symbol.BaseType != null && symbol.BaseType.SpecialType != SpecialType.System_Object)
            _result.RegisterEdge(symbol, symbol.BaseType, EdgeType.InheritsFrom);

        foreach (var iface in symbol.Interfaces)
            _result.RegisterEdge(symbol, iface, EdgeType.Implements);
    }
}