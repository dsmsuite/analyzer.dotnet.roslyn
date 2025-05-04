using dsmsuite.analyzer.dotnet.testdata.providers;

namespace dsmsuite.analyzer.dotnet.testdata.consumers
{
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