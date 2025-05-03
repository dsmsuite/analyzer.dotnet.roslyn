namespace dsmsuite.analyzer.dotnet.testdata.providers
{
    class ProviderStdListTemplateArgument
    {
        public void PublicMethodA() { }
        public void PublicMethodB() { }
        public void PublicMethodC() { }
        public void PublicMethodD() { }
    };

    class ProviderTemplateArgument1
    {
        public void PublicMethodA() { }
        public void PublicMethodB() { }
        public void PublicMethodC() { }
        public void PublicMethodD() { }
    };

    class ProviderTemplateArgument2
    {
        public void PublicMethodA() { }
        public void PublicMethodB() { }
        public void PublicMethodC() { }
        public void PublicMethodD() { }
    };

    class ProviderGenericClass<T, U> where T : new() where U : new()
    {
        public ProviderGenericClass() { }
        ~ProviderGenericClass() { }

        public T GetFirstTemplateArgument() { return new T(); }
        public U GetSecondTemplateArgument() { return new U(); }
    };
}