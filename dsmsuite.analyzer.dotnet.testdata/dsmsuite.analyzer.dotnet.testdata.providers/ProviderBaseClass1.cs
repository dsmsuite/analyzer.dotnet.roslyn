namespace dsmsuite.analyzer.dotnet.testdata.providers
{

    class ProviderBaseClass1 : ProviderAbstractClass, ProviderInterface
    {
        public ProviderBaseClass1() { }
        ~ProviderBaseClass1() { }

        public void InterfaceMethod() { }            // Implements ProviderInterface method
        public override void AbstractBaseMethod() { } // Implements AbstractClass abstract method
        public virtual void ConcreteBaseMethod() { } // Overrides BaseClass2 method implementation
    };

}