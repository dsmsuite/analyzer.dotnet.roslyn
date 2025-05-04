using dsmsuite.analyzer.dotnet.testdata.providers;

namespace dsmsuite.analyzer.dotnet.testdata.consumers
{

    class FieldConsumer
    {
        public FieldConsumer()
        {
            _intMember = 123;
            _enumMember = ProviderEnum.enum_val1;
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
                case ProviderEnum.enum_val1:
                    break;
                case ProviderEnum.enum_val2:
                    break;
                case ProviderEnum.enum_val3:
                    break;
                default:
                    break;
            }
        }

        public void MethodUsingStructMember()
        {
            _structMember.member1 = 1;
            _structMember.member2 = "test";
        }

        public void MethodUsingClassMember()
        {
            _classMember.PublicMethodA();
            _classMember.PublicMethodB();
        }

        public void MethodUsingStdListMember()
        {
            // Use explicit type
            ProviderListTemplateArgument firstElement = _listClassMember.First<ProviderListTemplateArgument>();
            firstElement.PublicMethodA();
            firstElement.PublicMethodB();

            // Use implicit type
            _listClassMember[0].PublicMethodC();
            _listClassMember[0].PublicMethodD();
        }

        public void MethodUsingGenericClassMember()
        {
            // Use explicit type
            ProviderTemplateArgument1? t = _genericClassMember.GetFirstTemplateArgument();
            t?.PublicMethodA();
            t?.PublicMethodB();

            ProviderTemplateArgument2? u = _genericClassMember.GetSecondTemplateArgument();
            u?.PublicMethodA();
            u?.PublicMethodB();

            // Use implicit type
            _genericClassMember.GetFirstTemplateArgument()?.PublicMethodC();
            _genericClassMember.GetFirstTemplateArgument()?.PublicMethodD();

            _genericClassMember.GetSecondTemplateArgument()?.PublicMethodC();
            _genericClassMember.GetSecondTemplateArgument()?.PublicMethodD();
        }

        private int _intMember;
        private ProviderEnum _enumMember;
        private ProviderStruct _structMember;
        private ProviderClass _classMember;
        private List<ProviderListTemplateArgument> _listClassMember;
        private ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2> _genericClassMember;
    };
}
