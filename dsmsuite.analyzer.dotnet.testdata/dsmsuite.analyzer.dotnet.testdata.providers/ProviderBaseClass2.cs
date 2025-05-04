namespace dsmsuite.analyzer.dotnet.testdata.providers
{

public class ProviderBaseClass2 : ProviderBaseClass4
{
	public ProviderBaseClass2() { }
    ~ProviderBaseClass2() { }

    public virtual void ConcreteBaseMethod() { } // Overrides BaseClass4 method implementation
};
}

