namespace dsmsuite.analyzer.dotnet.roslyn.test.Variables
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

    class VariableConsumer
    {
        public void MethodUsingIntVariable()
        {
            int intVariable = 123;
        }

        public void MethodUsingEnumVariable()
        {
            ProviderEnum enumVariable = ProviderEnum.enumVal1;

            switch (enumVariable)
            {
                case ProviderEnum.enumVal1:
                    break;
                case ProviderEnum.enumVal2:
                    break;
                default:
                    break;
            }
        }
        public void MethodUsingStructVariable()
        {
            ProviderStruct structVariable;
            structVariable.structMember1 = 1;
            structVariable.structMember2 = "test";
        }

        public void MethodUsingClassVariable()
        {
            ProviderClass classVariable = new ProviderClass(); ;
            classVariable.ProviderClassMethod();
        }

        public void MethodUsingStandardGenericContainerListVariable()
        {
            List<ProviderListTemplateArgument> listVariable = new List<ProviderListTemplateArgument>();

            ProviderListTemplateArgument firstElement = listVariable.First();
            firstElement.ProviderListTemplateArgumentMethod();
        }

        public void MethodUsingGenericClassVariable()
        {
            ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2> genericClassVariable = new ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2>();

            ProviderTemplateArgument1 t = genericClassVariable.GetFirstTemplateArgument();
            t.ProviderTemplateArgument1Method();

            ProviderTemplateArgument2 u = genericClassVariable.GetSecondTemplateArgument();
            u.ProviderTemplateArgument2Method();
        }
    };
}
