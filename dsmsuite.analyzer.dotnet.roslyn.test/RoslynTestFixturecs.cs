using dsmsuite.analyzer.dotnet.roslyn.Analysis;
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
            string code = File.ReadAllText(file);

            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            SyntaxNode root = tree.GetRoot();

            Guid guid = Guid.NewGuid();
            PortableExecutableReference mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            CSharpCompilation compilation = CSharpCompilation.Create(
                $"Analysis_{guid}",
                syntaxTrees: new[] { tree },
                references: new[] { mscorlib });

            SemanticModel semanticModel = compilation.GetSemanticModel(tree);

            CodeAnalysisResult result = new CodeAnalysisResult();
            SyntaxNodeVisitor walker = new SyntaxNodeVisitor(semanticModel, result);
            walker.Visit(root);

            //Assert.IsTrue(result.EdgeTypes.Count >  0);
        }
    }
}
