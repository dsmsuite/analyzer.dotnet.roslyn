using dsmsuite.analyzer.dotnet.roslyn.test.Interfacing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Inheritance
{
    public class BaseClass
    {
        public virtual void BaseMethod() { }
    }

    public class DerivedClass : BaseClass
    {
        public override void BaseMethod() { }
        public void DerivedMethod() { }
    }
}
