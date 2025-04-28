using dsmsuite.analyzer.dotnet.roslyn.Analysis;
using dsmsuite.analyzer.dotnet.roslyn.Data;
using Microsoft.Extensions.DependencyInjection;

if (args.Length < 2)
{
    Console.WriteLine($"Usage: CodeDependencyAnalyzer <solution_path> <sqlite-db-path>");
    return;
}

FileInfo solutionFileInfo = new FileInfo(Path.GetFullPath(args[0]));
FileInfo outputFileFileInfo = new FileInfo(Path.GetFullPath(args[1]));

if (solutionFileInfo.Extension != ".sln")
{
    Console.WriteLine($"Input file {solutionFileInfo.FullName} does not a solution file");
    return;
}

if (outputFileFileInfo.Extension != ".sql")
{
    Console.WriteLine($"Output file {outputFileFileInfo.FullName} does not a database file");
    return;
}

if (!solutionFileInfo.Exists)
{
    Console.WriteLine($"Solution file {solutionFileInfo.FullName} does not exist");
    return;
}

if (outputFileFileInfo.Exists)
{
    Console.WriteLine($"Solution file {outputFileFileInfo.FullName} already exists");
    return;
}

var services = new ServiceCollection();

// Register dependencies
services.AddSingleton<ICodeAnalyzer, CodeAnalyzer>();
services.AddSingleton<IGraphRepository>(provider => new SqliteGraphRepository(outputFileFileInfo.FullName));

var serviceProvider = services.BuildServiceProvider();

var analyzer = serviceProvider.GetRequiredService<ICodeAnalyzer>();
var repository = serviceProvider.GetRequiredService<IGraphRepository>();

Console.WriteLine($"Analyzing code at: {solutionFileInfo.FullName}");
await analyzer.AnalyzeAsync(solutionFileInfo.FullName, repository);

//Console.WriteLine("Saving results to database at {outputFileFileInfo.FullName}");
//repository.Create();
//repository.SaveNodes(graphBuilder.GetAllNodes());
//repository.SaveEdges(graphBuilder.GetAllEdges());

Console.WriteLine("Done.");

