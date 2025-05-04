using System;

namespace dsmsuite.analyzer.dotnet.testdata.providers
{
    class EventConsumer
    {
        public void MethodSubscribeInterfaceEvent()
        {
            IProviderInterface provider = new ProviderBaseClass1();
            provider.ProviderChanged += ProviderChangedEventHandler;
        }

        public void MethodUnsubscribeInterfaceEvent()
        {
            IProviderInterface provider = new ProviderBaseClass1();
            provider.ProviderChanged -= ProviderChangedEventHandler;
        }

        public void MethodSubscribeAbstractClassEvent()
        {
            ProviderAbstractClass provider = new ProviderBaseClass1();
            provider.ProviderChanged += ProviderChangedEventHandler;
        }

        public void MethodUnsubscribeAbstractClassEvent()
        {
            ProviderAbstractClass provider = new ProviderBaseClass1();
            provider.ProviderChanged -= ProviderChangedEventHandler;
        }

        public void MethodSubscribeConcretClassEvent()
        {
            ProviderBaseClass1 provider = new ProviderBaseClass1();
            provider.ProviderChanged += ProviderChangedEventHandler;
        }

        public void MethodUnsubscribeConcretClassEvent()
        {
            ProviderBaseClass1 provider = new ProviderBaseClass1();
            provider.ProviderChanged -= ProviderChangedEventHandler;
        }

        private void ProviderChangedEventHandler(object? sender, ProviderEventArgs e)
        {
            if (e.Name == "?") { }
        }
    };
}
