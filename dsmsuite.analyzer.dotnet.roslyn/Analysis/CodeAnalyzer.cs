using dsmsuite.analyzer.dotnet.roslyn.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;


namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public class CodeAnalyzer : ICodeAnalyzer
    {
        public async Task AnalyzeAsync(string solutionPath, IGraphRepository graphRepository)
        {
            CodeAnalysisResult codeAnalysisResult = new CodeAnalysisResult();
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution currSolution = await workspace.OpenSolutionAsync(solutionPath);

            foreach (Project project in currSolution.Projects)
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