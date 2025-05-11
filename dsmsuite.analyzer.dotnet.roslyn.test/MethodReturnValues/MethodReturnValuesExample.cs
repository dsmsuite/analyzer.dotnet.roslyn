namespace dsmsuite.analyzer.dotnet.roslyn.test.MethodReturnValues
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

    class ReturnTypeConsumer
    {
        public void MethodWithReturnTypeVoid()
        {
        }

        public int MethodWithReturnTypeInt()
        {
            return 0;
        }

        public ProviderEnum MethodWithGenericReturnTypeEnum()
        {
            ProviderEnum value = ProviderEnum.enum_val1;
            return value;
        }

        public ProviderStruct MethodWithGenericReturnTypeStruct()
        {
            ProviderStruct value = new ProviderStruct(0, "test");
            return value;
        }

        public ProviderClass MethodWithReturnTypeClass()
        {
            ProviderClass value = new ProviderClass();
            return value;
        }

        public ProviderClass? MethodWithReturnTypeNullableClass()
        {
            ProviderClass value = new ProviderClass();
            return value;
        }



        public List<ProviderListTemplateArgument> MethodWithListReturnType()
        {
            List<ProviderListTemplateArgument> value = new List<ProviderListTemplateArgument>();
            return value;
        }

        public ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2> MethodWithGenericClassReturnType()
        {
            ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2> value = new ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2>();
            return value;

        }
    };
}
