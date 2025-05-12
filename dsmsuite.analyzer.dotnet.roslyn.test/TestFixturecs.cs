using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;
using dsmsuite.analyzer.dotnet.roslyn.Analysis.Reporting;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using dsmsuite.analyzer.dotnet.roslyn.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Mono.Cecil.Cil;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace dsmsuite.analyzer.dotnet.roslyn.test
{
    public class ReporterFake : IResultReporter
    {
        public void ReportResult(string actionDescription, string syntaxNodeFilename, int syntaxNodeline, Result result, [CallerFilePath] string sourceFile = "", [CallerMemberName] string method = "", [CallerLineNumber] int lineNumber = 0)
        {
            Console.WriteLine($"Action: {actionDescription} syntaxNodeFilename={syntaxNodeFilename} syntaxNodeline={syntaxNodeline} result={result} sourceFile={sourceFile} method={method} lineNumber={lineNumber}");
        }
    }

    public class TestFixture
    {
        private string _namespace = GetNamespace();
        private HierarchicalGraph _hierarchicalGraph = CreateHierarchicalGraph();

        public void Analyze(string sourceCodeFile, [CallerFilePath] string callerFilePath = "")
        {
            SyntaxTree tree = CreateSyntaxTreeFromSourceCodeFile(sourceCodeFile, callerFilePath);
            SemanticModel semanticModel = CreateSemanticModel(tree);
            SyntaxNodeVisitor walker = new SyntaxNodeVisitor(semanticModel, _hierarchicalGraph);
            walker.Visit(tree.GetRoot());
            _hierarchicalGraph.Build();
        }

        public string Namespace => _namespace;

        public bool NodeCountIs(int expectedNodeCount)
        {
            int actualNodeCount = 0;
            foreach (INode node in _hierarchicalGraph.Nodes)
            {
                if (node.Fullname.StartsWith($"{_namespace}."))
                {
                    actualNodeCount++;
                }
            }

            bool actualNodeCountOk = actualNodeCount == expectedNodeCount;

            if (!actualNodeCountOk)
            {
                Console.WriteLine($"Expected node count: {expectedNodeCount}");
                Console.WriteLine($"Actual node count: {actualNodeCount}");
            }

            return actualNodeCountOk;
        }

        public bool NodeExists(string name, NodeType nodeType)
        {
            int count = 0;
            foreach (INode node in _hierarchicalGraph.Nodes)
            {
                if (NodeNameMatches(node, name) && 
                    NodeTypeMatches(node, nodeType))
                {
                    count++;
                }
            }

            if (count != 1)
            {
                Console.WriteLine($"Expected node:");
                Console.WriteLine($"  name={GetExpectedNodeName(name)} type={nodeType}");

                Console.WriteLine("Actual nodes:");
                foreach (INode node in _hierarchicalGraph.Nodes)
                {
                    Console.WriteLine($"  name={node.Fullname} type={node.NodeType} file={node.Filename} lines={node.Startline}-{node.Endline}");
                }
            }

            return count == 1;
        }

        public bool EdgeCountIs(int expectedEdgeCount)
        {
            bool actualEdgeCountOk = _hierarchicalGraph.Edges.Count() == expectedEdgeCount;

            if (!actualEdgeCountOk)
            {
                Console.WriteLine($"Expected edge count: {expectedEdgeCount}");
                Console.WriteLine($"Actual edge count: {_hierarchicalGraph.Edges.Count()}");
            }

            return actualEdgeCountOk;
        }

        public bool EdgeExists(string source, string target, EdgeType edgeType)
        {
            int count = 0;
            foreach (IEdge edge in _hierarchicalGraph.Edges)
            {
                if (NodeNameMatches(edge.Source, source) && 
                    NodeNameMatches(edge.Target, target) && 
                    EdgeTypeMatches(edge, edge.EdgeType))
                {
                    count++;
                }
            }

            if (count != 1)
            {
                Console.WriteLine($"Expected edge:");
                Console.WriteLine($"  source={GetExpectedNodeName(source)} target={GetExpectedNodeName(target)} type={edgeType}");

                Console.WriteLine("Actual edges:");
                foreach (IEdge edge in _hierarchicalGraph.Edges)
                {
                    Console.WriteLine($"  Edge: source={edge.Source.Fullname} target={edge.Target.Fullname} type={edge.EdgeType} file={edge.Filename} line={edge.Line}");
                }
            }
            return count == 1;
        }

        private bool NodeNameMatches(INode node, string actual)
        {
            return node.Fullname == GetExpectedNodeName(actual);
        }

        private string GetExpectedNodeName(string actual)
        {
            return $"{_namespace}.{actual}";
        }

        private bool NodeTypeMatches(INode node, NodeType nodeType)
        {
            return node.NodeType == nodeType;
        }

        private bool EdgeTypeMatches(IEdge edge, EdgeType edgeType)
        {
            return edge.EdgeType == edgeType;
        }

        public void ReportResult(string actionDescription, string syntaxNodeFilename, int syntaxNodeline, Result result, [CallerFilePath] string sourceFile = "", [CallerMemberName] string method = "", [CallerLineNumber] int lineNumber = 0)
        {

        }

        private SyntaxTree CreateSyntaxTreeFromSourceCodeFile(string sourceCodeFile, string callerFilePath)
        {
            string? callerDirectoryPath = Path.GetDirectoryName(callerFilePath);
            Assert.IsNotNull(callerDirectoryPath, "Caller directory path cannot be null.");

            string filename = Path.Combine(callerDirectoryPath, sourceCodeFile);
            string code = File.ReadAllText(filename);
            return CSharpSyntaxTree.ParseText(code);
        }

        private SemanticModel CreateSemanticModel(SyntaxTree tree)
        {
            CSharpCompilation compilation = CreateCompilationUnit(tree);
            return compilation.GetSemanticModel(tree);
        }

        private static HierarchicalGraph CreateHierarchicalGraph()
        {
            ReporterFake reporterFakeInstance = new ReporterFake();
            return new HierarchicalGraph(reporterFakeInstance);
        }

        private static string GetNamespace()
        {
            string? assemblyName = typeof(TestFixture).Assembly.GetName().Name;
            Assert.IsNotNull(assemblyName, "Assembly name cannot be null.");
            return assemblyName;
        }

        private CSharpCompilation CreateCompilationUnit(SyntaxTree tree)
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
