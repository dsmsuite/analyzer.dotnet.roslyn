using System;

namespace dsmsuite.analyzer.dotnet.testdata.providers
{
    public abstract class ProviderAbstractClass
    {
        public event EventHandler<ProviderEventArgs> ProviderChanged;

        public ProviderAbstractClass() { }
        ~ProviderAbstractClass() { }

        public abstract void AbstractBaseMethod();

        private void TriggerEvent()
        {
            ProviderChanged?.Invoke(this, new ProviderEventArgs());
        }
    };
}

