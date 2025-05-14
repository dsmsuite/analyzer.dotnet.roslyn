namespace dsmsuite.analyzer.dotnet.roslyn.test.Fields
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

    public class FieldConsumer
    {
        public FieldConsumer()
        {
            _intMember = 123;
            _enumMember = ProviderEnum.enumVal1;
            _structMember = new ProviderStruct(1, "test");
            _classMember = new ProviderClass();
            _listClassMember = new List<ProviderListTemplateArgument>();
            _genericClassMember = new ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2>();
        }

        public void MethodUsingIntMember()
        {
            _intMember = 123;
        }

        public void MethodUsingEnumMember()
        {
            switch (_enumMember)
            {
                case ProviderEnum.enumVal1:
                    break;
                case ProviderEnum.enumVal2:
                    break;
                default:
                    break;
            }
        }

        public void MethodUsingStructMember()
        {
            _structMember.structMember1 = 1;
            _structMember.structMember2 = "test";
        }

        public void MethodUsingClassMember()
        {
            _classMember.ProviderClassMethod();
        }

        public void MethodUsingStdListMember()
        {
            ProviderListTemplateArgument firstElement = _listClassMember.First<ProviderListTemplateArgument>();
            firstElement.ProviderListTemplateArgumentMethod();
        }

        public void MethodUsingGenericClassMember()
        {
            ProviderTemplateArgument1? t = _genericClassMember.GetFirstTemplateArgument();
            t?.ProviderTemplateArgument1Method();

            ProviderTemplateArgument2? u = _genericClassMember.GetSecondTemplateArgument();
            u?.ProviderTemplateArgument2Method();
        }

        private int _intMember;
        private ProviderEnum _enumMember;
        private ProviderStruct _structMember;
        private ProviderClass _classMember;
        private List<ProviderListTemplateArgument> _listClassMember;
        private ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2> _genericClassMember;
    };
}
