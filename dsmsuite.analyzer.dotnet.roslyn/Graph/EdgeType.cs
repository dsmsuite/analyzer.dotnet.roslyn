namespace dsmsuite.analyzer.dotnet.roslyn.Graph
{
    public enum EdgeType
    {
        Call,
        Inheritance,
        Implements,
        Overrride,
        TemplateParameter,
        VariableUsage,
        ParameterUsage,
        FieldUsage,
        Parameter,
        Returns,
        Subscribes,
        Unsubscribes,
        Triggers,
    }
}
