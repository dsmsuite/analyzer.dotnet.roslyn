using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.MethodParameters
{

    [TestClass]
    public sealed class MethodParametersTest : TestFixture
    {
        [TestMethod]
        public void TestMethod1()
        {
             Analyze("MethodParametersExample.cs");
        }

        [TestMethod]
        public void TestMethod2()
        {
             Analyze("MethodParametersExample.cs");
        }
    }
}
