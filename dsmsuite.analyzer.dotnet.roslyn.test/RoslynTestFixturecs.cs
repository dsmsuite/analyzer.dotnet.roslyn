using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;
using dsmsuite.analyzer.dotnet.roslyn.Analysis.Reporting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.test
{
    public class RoslynTestFixturecs
    {
        //public void Analyze(string file)
        //{
        //    Workspace workspace = new AdhocWorkspace();
        //    ProjectInfo projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(),
        //                                         VersionStamp.Create(),
        //                                         "MyProject",
        //                                         "MyAssembly",
        //                                         LanguageNames.CSharp);

        //    Project project = workspace.AddProject(projectInfo);

        //    string sourceCode = File.ReadAllText(file);
        //    Document document = workspace.AddDocument(project.Id, Path.GetFileName(file), SourceText.From(sourceCode));

        //    SyntaxTree syntaxTree = document.GetSyntaxTreeAsync().Result;
        //    SyntaxNode root = syntaxTree.GetRoot();
        //    Compilation compilation = project.GetCompilationAsync().Result;
        //    SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);
        //}

        public static void Analyz2(string file)
        {
            SyntaxTree tree = CreateSyntaxTree(file);
            SemanticModel semanticModel = CreateSemanticModel(tree);
            ResultReporter reporter = new ResultReporter();
            ResultCollector result = new ResultCollector(reporter);
            SyntaxNodeVisitor walker = new SyntaxNodeVisitor(semanticModel, result);
            walker.Visit(tree.GetRoot());

            //Assert.IsTrue(result.EdgeTypes.Count >  0);
        }

        private static SyntaxTree CreateSyntaxTree(string file)
        {
            string code = File.ReadAllText(file);
            return CSharpSyntaxTree.ParseText(code);
        }

        private static SemanticModel CreateSemanticModel(SyntaxTree tree)
        {
            Guid guid = Guid.NewGuid();
            PortableExecutableReference mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            CSharpCompilation compilation = CSharpCompilation.Create(
                $"Analysis_{guid}",
                syntaxTrees: new[] { tree },
                references: new[] { mscorlib });
            return compilation.GetSemanticModel(tree);
        }
    }
}
