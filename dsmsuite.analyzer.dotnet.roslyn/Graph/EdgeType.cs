namespace dsmsuite.analyzer.dotnet.roslyn.Graph
{
    public enum EdgeType
    {
        Call,
        Inheritance,
        Implements,
        Overrride,
        TemplateParameter,
        Usage,
        Parameter,
        Returns,
        Subscribes,
        Unsubscribes,
        Triggers,
    }
}
