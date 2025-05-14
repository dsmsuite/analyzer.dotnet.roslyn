namespace dsmsuite.analyzer.dotnet.roslyn.test.MethodParameters
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

    class ParameterConsumer
    {
        public void MethodWithIntParameter(int intParameter)
        {
            intParameter = 123;
        }

        public void MethodWithEnumParameter(ProviderEnum enumParameter)
        {
            enumParameter = ProviderEnum.enumVal1;
        }

        public void MethodWithStructParameter(ProviderStruct structParameter)
        {
            structParameter.structMember1 = 1;
            structParameter.structMember2 = "test";
        }

        public void MethodWithClassParameter(ProviderClass classParameter)
        {
            classParameter.ProviderClassMethod();
        }

        public void MethodWithNullableClassParameter(ProviderClass? classParameter)
        {
            classParameter?.ProviderClassMethod();
        }

        public void MethodWithStdListParameter(List<ProviderListTemplateArgument> listParameter)
        {
            ProviderListTemplateArgument firstElement = listParameter.First<ProviderListTemplateArgument>();
            firstElement.ProviderListTemplateArgumentMethod();
        }

        public void MethodWithGenericClassParameter(ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2> genericClassParameter)
        {
            ProviderTemplateArgument1? t = genericClassParameter.GetFirstTemplateArgument();
            t?.ProviderTemplateArgument1Method();

            ProviderTemplateArgument2? u = genericClassParameter.GetSecondTemplateArgument();
            u?.ProviderTemplateArgument2Method();
        }
    };
}
