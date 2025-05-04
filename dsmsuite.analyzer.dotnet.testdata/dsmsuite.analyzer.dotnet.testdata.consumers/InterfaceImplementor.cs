using dsmsuite.analyzer.dotnet.testdata.providers;

namespace dsmsuite.analyzer.dotnet.testdata.consumers
{
    public class InterfaceImplementor : IProviderInterface
    {
        public event EventHandler<ProviderEventArgs> ProviderChanged;

        public void InterfaceMethod() { }            // Implements ProviderInterface method
    }
}
