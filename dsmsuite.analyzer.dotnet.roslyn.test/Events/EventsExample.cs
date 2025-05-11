using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Events
{
    public class ProviderEventArgs
    {
        public string Name { get; set; }
    }

    public interface IProviderInterface
    {
        event EventHandler<ProviderEventArgs> ProviderChanged;
    }

    public class ProviderClass : IProviderInterface
    {
        public event EventHandler<ProviderEventArgs> ProviderChanged;

        public void TriggerEvent()
        {
            ProviderChanged?.Invoke(this, new ProviderEventArgs());
        }
    };

    public class EventConsumer
    {
        public void MethodSubscribeInterfaceEvent()
        {
            IProviderInterface provider = new ProviderClass();
            provider.ProviderChanged += ProviderChangedEventHandler;
        }

        public void MethodUnsubscribeInterfaceEvent()
        {
            IProviderInterface provider = new ProviderClass();
            provider.ProviderChanged -= ProviderChangedEventHandler;
        }

        public void MethodSubscribeClassEvent()
        {
            ProviderClass provider = new ProviderClass();
            provider.ProviderChanged += ProviderChangedEventHandler;
        }

        public void MethodUnsubscribeClassEvent()
        {
            ProviderClass provider = new ProviderClass();
            provider.ProviderChanged -= ProviderChangedEventHandler;
        }

        private void ProviderChangedEventHandler(object? sender, ProviderEventArgs e)
        {
            if (e.Name == "?") { }
        }
    };
}
