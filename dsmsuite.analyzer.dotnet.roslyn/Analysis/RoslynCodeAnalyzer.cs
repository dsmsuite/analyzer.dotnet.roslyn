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

            foreach (Project project in currSolution.Projects)
            {
                if (project.FilePath != null)
                {
                    Console.WriteLine($"Processing project {project.FilePath}");

                    foreach (var document in project.Documents)
                    {
                        if (document.FilePath != null)
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
}
