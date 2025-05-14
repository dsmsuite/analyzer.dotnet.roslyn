namespace dsmsuite.analyzer.dotnet.roslyn.test.MethodReturnValues
{
    public struct ProviderStruct
    {
        public ProviderStruct(int val1, string val2)
        {
            structMember1 = val1;
            structMember2 = val2;
        }

        public int structMember1;
        public string structMember2;
    };

    public enum ProviderEnum
    {
        enumVal1,
        enumVal2
    };

    public class ProviderClass
    {
        public void ProviderClassMethod() { }
    }

    public class ProviderListTemplateArgument
    {
        public void ProviderListTemplateArgumentMethod() { }
    };

    public class ProviderTemplateArgument1
    {
        public void ProviderTemplateArgument1Method() { }
    };

    public class ProviderTemplateArgument2
    {
        public void ProviderTemplateArgument2Method() { }
    };

    public class ProviderGenericClass<T, U> where T : new() where U : new()
    {
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
            ProviderEnum value = ProviderEnum.enumVal1;
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
