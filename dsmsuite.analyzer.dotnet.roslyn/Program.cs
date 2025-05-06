using dsmsuite.analyzer.dotnet.roslyn.Analysis;
using dsmsuite.analyzer.dotnet.roslyn.Data;
using dsmsuite.analyzer.dotnet.roslyn.Util;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

Logger.Init(Assembly.GetExecutingAssembly(), true);
Logger.LogLevel = LogLevel.Error;

if (args.Length < 2)
{
    Console.WriteLine($"Usage: CodeDependencyAnalyzer <solution_path> <sqlite-db-path>");
    return;
}

FileInfo solutionFileInfo = new FileInfo(Path.GetFullPath(args[0]));
FileInfo outputFileFileInfo = new FileInfo(Path.GetFullPath(args[1]));

if (solutionFileInfo.Extension != ".sln")
{
    Logger.LogUserMessage($"Input file {solutionFileInfo.FullName} does not a solution file");
    return;
}

if (outputFileFileInfo.Extension != ".sql")
{
    Logger.LogUserMessage($"Output file {outputFileFileInfo.FullName} does not a database file");
    return;
}

if (!solutionFileInfo.Exists)
{
    Logger.LogUserMessage($"Solution file {solutionFileInfo.FullName} does not exist");
    return;
}

if (outputFileFileInfo.Exists)
{
    Logger.LogUserMessage($"Solution file {outputFileFileInfo.FullName} already exists");
    return;
}

var services = new ServiceCollection();

// Register dependencies
services.AddSingleton<ICodeAnalyzer, SolutionAnalyzer>();
services.AddSingleton<IGraphRepository>(provider => new SqliteGraphRepository(outputFileFileInfo.FullName));

var serviceProvider = services.BuildServiceProvider();

var analyzer = serviceProvider.GetRequiredService<ICodeAnalyzer>();
var repository = serviceProvider.GetRequiredService<IGraphRepository>();

Console.WriteLine($"Analyzing code at: {solutionFileInfo.FullName}");
await analyzer.AnalyzeAsync(solutionFileInfo.FullName, repository);

Logger.LogUserMessage("Done.");
Logger.Flush();

