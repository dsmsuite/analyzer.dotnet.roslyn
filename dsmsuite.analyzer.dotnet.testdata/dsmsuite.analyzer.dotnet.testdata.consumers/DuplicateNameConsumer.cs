using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dsmsuite.analyzer.dotnet.testdata;

namespace dsmsuite.analyzer.dotnet.testdata.consumers
{
    public class DuplicateNameConsumer : IDuplicateName, providers.IDuplicateName
    {
    }
}
