using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Interfacing
{
    public interface Interface1
    {
        void Interface1Method();
    }

    public interface Interface2
    {
        void Interface2Method();
    }

    public class InterfaceImplemation : Interface1, Interface2
    {
        public void Interface1Method()
        {
        }

        public void Interface2Method()
        {

        }
    }
}
