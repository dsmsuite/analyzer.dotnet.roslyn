using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;
using dsmsuite.analyzer.dotnet.roslyn.Analysis.Reporting;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using dsmsuite.analyzer.dotnet.roslyn.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Runtime.CompilerServices;

namespace dsmsuite.analyzer.dotnet.roslyn.test
{
    public class ReporterFake : IResultReporter
    {
        public void ReportResult(string actionDescription, string syntaxNodeFilename, int syntaxNodeline, Result result, [CallerFilePath] string sourceFile = "", [CallerMemberName] string method = "", [CallerLineNumber] int lineNumber = 0)
        {
        }
    }

    public class RoslynTestFixture
    {
        public static HierarchicalGraph Analyze(string sourceCodeFile, [CallerFilePath] string callerFilePath = "")
        {
            SyntaxTree tree = CreateSyntaxTreeFromSourceCodeFile(sourceCodeFile, callerFilePath);
            SemanticModel semanticModel = CreateSemanticModel(tree);
            HierarchicalGraph hierarchicalGraph = CreateHierarchicalGraph();
            SyntaxNodeVisitor walker = new SyntaxNodeVisitor(semanticModel, hierarchicalGraph);
            walker.Visit(tree.GetRoot());
            hierarchicalGraph.Build();
            PrintHierarchicalGraph(hierarchicalGraph);

            return hierarchicalGraph;
        }

        public void ReportResult(string actionDescription, string syntaxNodeFilename, int syntaxNodeline, Result result, [CallerFilePath] string sourceFile = "", [CallerMemberName] string method = "", [CallerLineNumber] int lineNumber = 0)
        {

        }

        public static void PrintHierarchicalGraph(HierarchicalGraph hierarchicalGraph)
        {
            foreach (INode node in hierarchicalGraph.Nodes)
            {
                Console.WriteLine($"Node: name={node.Name} type={node.NodeType} file={node.Filename} lines={node.Startline}-{node.Endline}");
            }
            foreach (IEdge edge in hierarchicalGraph.Edges)
            {
                Console.WriteLine($"Edge: source={edge.Source.Name} target={edge.Target.Name} type={edge.EdgeType} file={edge.Filename} line={edge.Line}");
            }   
        }

        private static SyntaxTree CreateSyntaxTreeFromSourceCodeFile(string sourceCodeFile, string callerFilePath)
        {
            string? callerDirectoryPath = Path.GetDirectoryName(callerFilePath);
            Assert.IsNotNull(callerDirectoryPath, "Caller directory path cannot be null.");

            string filename = Path.Combine(callerDirectoryPath, sourceCodeFile);
            string code = File.ReadAllText(filename);
            return CSharpSyntaxTree.ParseText(code);
        }

        private static SemanticModel CreateSemanticModel(SyntaxTree tree)
        {
            CSharpCompilation compilation = CreateCompilationUnit(tree);
            return compilation.GetSemanticModel(tree);
        }

        private static HierarchicalGraph CreateHierarchicalGraph()
        {
            ReporterFake reporterFakeInstance = new ReporterFake();
            return new HierarchicalGraph(reporterFakeInstance);
        }

        private static CSharpCompilation CreateCompilationUnit(SyntaxTree tree)
        {
            Guid guid = Guid.NewGuid();
            PortableExecutableReference mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            CSharpCompilation compilation = CSharpCompilation.Create(
                $"Analysis_{guid}",
                syntaxTrees: new[] { tree },
                references: new[] { mscorlib });
            return compilation;
        }


    }
}
