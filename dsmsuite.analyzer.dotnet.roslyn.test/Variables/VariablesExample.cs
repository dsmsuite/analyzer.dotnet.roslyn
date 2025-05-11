namespace dsmsuite.analyzer.dotnet.roslyn.test.Variables
{
    public struct ProviderStruct
    {
        public ProviderStruct()
        {
            member1 = 0;
            member2 = "val2";
        }

        public ProviderStruct(int val1, string val2)
        {
            member1 = val1;
            member2 = val2;
        }

        public int member1;
        public string member2;
    };

    public enum ProviderEnum
    {
        enum_val1,
        enum_val2,
        enum_val3
    };

    public class ProviderClass
    {
        public void PublicMethodA() { }
        public void PublicMethodB() { }
    }

    public class ProviderListTemplateArgument
    {
        public void PublicMethodA() { }
        public void PublicMethodB() { }
    };

    public class ProviderTemplateArgument1
    {
        public void PublicMethodA() { }
        public void PublicMethodB() { }
    };

    public class ProviderTemplateArgument2
    {
        public void PublicMethodA() { }
        public void PublicMethodB() { }
    };

    public class ProviderGenericClass<T, U> where T : new() where U : new()
    {
        public ProviderGenericClass() { }
        ~ProviderGenericClass() { }

        public T GetFirstTemplateArgument() { return new T(); }
        public U GetSecondTemplateArgument() { return new U(); }
    };
}
