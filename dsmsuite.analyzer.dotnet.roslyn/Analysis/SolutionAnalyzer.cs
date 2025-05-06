using dsmsuite.analyzer.dotnet.roslyn.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;


namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public class SolutionAnalyzer : ICodeAnalyzer
    {
        public async Task AnalyzeAsync(string InputPath, IGraphRepository graphRepository)
        {
            CodeAnalysisResult codeAnalysisResult = new CodeAnalysisResult();
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution solution = await workspace.OpenSolutionAsync(InputPath);

            foreach (Project project in solution.Projects)
            {
                if (project.FilePath != null)
                {
                    Console.WriteLine($"Processing project {project.FilePath}");

                    Compilation? compilation = await project.GetCompilationAsync();

                    if (compilation != null)
                    {
                        foreach (Document document in project.Documents)
                        {
                            if (document.FilePath != null)
                            {
                                SyntaxTree? syntaxTree = await document.GetSyntaxTreeAsync();
                                if (syntaxTree != null)
                                {
                                    SemanticModel semanticModel = compilation.GetSemanticModel(syntaxTree);

                                    SyntaxNode root = await syntaxTree.GetRootAsync();
                                    SyntaxNodeVisitor visitor = new SyntaxNodeVisitor(semanticModel, codeAnalysisResult);
                                    visitor.Visit(root);
                                }
                            }
                        }
                    }
                }
            }

            codeAnalysisResult.Save(graphRepository);
        }
    }
}
;