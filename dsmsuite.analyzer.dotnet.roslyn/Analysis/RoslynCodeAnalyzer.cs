using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;


namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public class RoslynCodeAnalyzer : ICodeAnalyzer
    {
        public async Task AnalyzeAsync(string solutionPath, IGraphBuilder graphBuilder)
        {
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution currSolution = workspace.OpenSolutionAsync(solutionPath).Result;

            foreach (Project projectPath in currSolution.Projects)
            {
                if (projectPath.FilePath != null)
                {
                    Project project = await workspace.OpenProjectAsync(projectPath.FilePath);
                    foreach (var document in project.Documents)
                    {
                        var syntaxTree = await document.GetSyntaxTreeAsync();
                        var semanticModel = await document.GetSemanticModelAsync();

                        if (syntaxTree == null || semanticModel == null) continue;

                        var root = await syntaxTree.GetRootAsync();
                        var visitor = new DependencyVisitor(semanticModel, graphBuilder, document.FilePath ?? "UnknownFile.cs");
                        visitor.Visit(root);
                    }
                }
            }
        }
    }
}
