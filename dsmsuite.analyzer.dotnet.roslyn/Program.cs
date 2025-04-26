using dsmsuite.analyzer.dotnet.roslyn.Analysis;
using dsmsuite.analyzer.dotnet.roslyn.Data;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.Extensions.DependencyInjection;

if (args.Length < 2)
{
    Console.WriteLine("Usage: CodeDependencyAnalyzer <source-directory> <sqlite-db-path>");
    return;
}

var sourcePath = args[0];
var dbPath = args[1];

var services = new ServiceCollection();

// Register dependencies
services.AddSingleton<ICodeAnalyzer, RoslynCodeAnalyzer>();
services.AddSingleton<IGraphBuilder, InMemoryGraphBuilder>();
services.AddSingleton<IGraphRepository>(provider => new SqliteGraphRepository(dbPath));

var serviceProvider = services.BuildServiceProvider();

var analyzer = serviceProvider.GetRequiredService<ICodeAnalyzer>();
var graphBuilder = serviceProvider.GetRequiredService<IGraphBuilder>();
var repository = serviceProvider.GetRequiredService<IGraphRepository>();

Console.WriteLine($"Analyzing code at: {sourcePath}");
await analyzer.AnalyzeAsync(sourcePath, graphBuilder);

Console.WriteLine("Saving results to database...");
repository.SaveNodes(graphBuilder.GetAllNodes());
repository.SaveEdges(graphBuilder.GetAllEdges());

Console.WriteLine("Done.");