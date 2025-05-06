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
        ParameterType,
        ReturnType,
        FieldType,
        PropertyType,
        VariableType,
        SubscribeEvent,
        UnsubscribeEvent,
        TriggerEvent,
        HandlEvent,
        TypeUsage,
        InheritsFrom,
        Type,
    }
}
