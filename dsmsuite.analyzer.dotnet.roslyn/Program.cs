using dsmsuite.analyzer.dotnet.roslyn.Analysis.Analyzer;
using dsmsuite.analyzer.dotnet.roslyn.Data;
using dsmsuite.analyzer.dotnet.roslyn.Util;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using dsmsuite.analyzer.dotnet.roslyn.Analysis.Reporting;
using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

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
ResultReporter resultReporter = new ResultReporter();
ResultCollector resultCollector = new ResultCollector(resultReporter);
SolutionAnalyzer analyzer = new SolutionAnalyzer(solutionFileInfo.FullName, resultCollector);
await analyzer.AnalyzeAsync();

SqliteGraphRepository sqliteGraphRepository = new SqliteGraphRepository(outputFileFileInfo.FullName);
//sqliteGraphRepository.SaveNode(resultCollector.Graph);

Logger.LogUserMessage("Done.");
Logger.Flush();

