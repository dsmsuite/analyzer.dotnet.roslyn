using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public enum EdgeType
    {
        Call,
        Inheritance,
        Implements,
        Overrride,
        TemplateParameter,
        Usage,
        Parameter,
        Returns,
        Subscribes,
        Unsubscribes,
        Triggers,
    }
}
