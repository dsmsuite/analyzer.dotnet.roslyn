namespace dsmsuite.analyzer.dotnet.testdata.providers
{

	class ParameterConsumer
	{
		public void MethodWithIntParameter(int intParameter) 
		{
            intParameter = 123;
        }

		public void MethodWithEnumParameter(ProviderEnum enumParameter) 
		{
            enumParameter = ProviderEnum.enum_val1;
        }
		
		public void MethodWithStructParameter(ProviderStruct structParameter)
		{
            structParameter.member1 = 1;
            structParameter.member2 = "test";
        }
		
		public void MethodWithClassParameter(ProviderClass classParameter) 
		{
            classParameter.PublicMethodA();
            classParameter.PublicMethodB();
        }

        public void MethodWithNullableClassParameter(ProviderClass? classParameter)
        {
            classParameter?.PublicMethodA();
            classParameter?.PublicMethodB();
        }

        public void MethodWithStdListParameter(List<ProviderListTemplateArgument> listParameter) 
		{
            ProviderListTemplateArgument firstElement = listParameter.First<ProviderListTemplateArgument>();
            firstElement.PublicMethodA();
            firstElement.PublicMethodB();

            // Use implicit type
            listParameter[0].PublicMethodC();
            listParameter[0].PublicMethodD();
        }
		
		public void MethodWithGenericClassParameter(ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2> genericClassParameter) 
		{
            // Use explicit type
            ProviderTemplateArgument1? t = genericClassParameter.GetFirstTemplateArgument();
            t?.PublicMethodA();
            t?.PublicMethodB();

            ProviderTemplateArgument2? u = genericClassParameter.GetSecondTemplateArgument();
            u?.PublicMethodA();
            u?.PublicMethodB();

            // Use implicit type
            genericClassParameter.GetFirstTemplateArgument()?.PublicMethodC();
            genericClassParameter.GetFirstTemplateArgument()?.PublicMethodD();

            genericClassParameter.GetSecondTemplateArgument()?.PublicMethodC();
            genericClassParameter.GetSecondTemplateArgument()?.PublicMethodD();
        }
	};
}
