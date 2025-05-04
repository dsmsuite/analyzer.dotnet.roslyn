namespace dsmsuite.analyzer.dotnet.testdata.providers
{

    class VariableConsumer
    {
        public void MethodUsingIntVariable()
        {
            int intVariable = 123;
        }

        public void MethodUsingEnumVariable()
        {
            ProviderEnum enumVariable = ProviderEnum.enum_val1;

            switch (enumVariable)
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
        public void MethodUsingStructVariable()
        {
            ProviderStruct structVariable;
            structVariable.member1 = 1;
            structVariable.member2 = "test";
        }

        public void MethodUsingClassVariable()
        {
            ProviderClass classVariable = new ProviderClass(); ;
            classVariable.PublicMethodA();
            classVariable.PublicMethodB();
        }

        public void MethodUsingStandardGenericContainerListVariable()
        {
            List<ProviderListTemplateArgument> listVariable = new List<ProviderListTemplateArgument>();

            // Use explicit type
            ProviderListTemplateArgument firstElement = listVariable.First();
            firstElement.PublicMethodA();
            firstElement.PublicMethodB();

            // Use implicit type
            listVariable.First().PublicMethodC();
            listVariable.First().PublicMethodD();
        }

        public void MethodUsingGenericClassVariable()
        {
            ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2> genericClassVariable = new ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2>();

            // Use explicit type
            ProviderTemplateArgument1 t = genericClassVariable.GetFirstTemplateArgument();
            t.PublicMethodA();
            t.PublicMethodB();

            ProviderTemplateArgument2 u = genericClassVariable.GetSecondTemplateArgument();
            u.PublicMethodA();
            u.PublicMethodB();

            // Use implicit type
            genericClassVariable.GetFirstTemplateArgument().PublicMethodC();
            genericClassVariable.GetFirstTemplateArgument().PublicMethodD();

            genericClassVariable.GetSecondTemplateArgument().PublicMethodC();
            genericClassVariable.GetSecondTemplateArgument().PublicMethodD();
        }
    };
}

