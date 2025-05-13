using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.test.MetaData
{
    public interface IMetaData
    {
        void InterfaceMethod();
    }

    public class MetaDataClass : IMetaData
    {
        public void InterfaceMethod()
        {
        }
    }
}
