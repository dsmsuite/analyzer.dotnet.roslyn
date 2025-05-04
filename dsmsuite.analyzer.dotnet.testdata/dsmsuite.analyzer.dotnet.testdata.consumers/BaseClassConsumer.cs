namespace dsmsuite.analyzer.dotnet.testdata.providers
{
    class BaseClassConsumer
    {
        public void MethodCallingInterfaceMethody()
        {
            IProviderInterface provider = new ProviderBaseClass1();
            provider.InterfaceMethod();
        }

        public void MethodCallingAbstractFunctionsInClassHierarchy()
        {
            ProviderAbstractClass provider = new ProviderBaseClass1();
            provider.AbstractBaseMethod();
        }

        public void MethodCallingConcreteFunctionsInClassHierarchy()
        {
            ProviderBaseClass1 provider = new ProviderBaseClass1();
            provider.InterfaceMethod();
            provider.AbstractBaseMethod();
            provider.ConcreteBaseMethod();
        }
    };
}
