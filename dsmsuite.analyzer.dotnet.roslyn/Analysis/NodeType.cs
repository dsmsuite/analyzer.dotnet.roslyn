using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public enum NodeType
    {
        Class,
        Struct,
        Enum,
        EnumValue,
        Method,
        Variable,
        Property,
        Field,
        Event
    }
}
