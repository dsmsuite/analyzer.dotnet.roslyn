using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;
using dsmsuite.analyzer.dotnet.roslyn.Analysis.Reporting;
using dsmsuite.analyzer.dotnet.roslyn.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;


namespace dsmsuite.analyzer.dotnet.roslyn.Analysis.Analyzer
{
    public class SolutionAnalyzer : ICodeAnalyzer
    {
        private readonly string _solutionPath;
        private readonly IResultCollector _results;

        public SolutionAnalyzer(string solutionPath, IResultCollector results)
        {
            _solutionPath = solutionPath;
            _results = results;
        }

        public async Task AnalyzeAsync()
        {
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution solution = await workspace.OpenSolutionAsync(_solutionPath);

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
                                    SyntaxNodeVisitor visitor = new SyntaxNodeVisitor(semanticModel, _results);
                                    visitor.Visit(root);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
;