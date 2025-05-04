using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.testdata.providers
{
    public class ProviderEventArgs
    {
        public string Name { get; set; }
    }

    public interface IProviderInterface
    {
        event EventHandler<ProviderEventArgs> ProviderChanged;
        void InterfaceMethod();
    }
}
