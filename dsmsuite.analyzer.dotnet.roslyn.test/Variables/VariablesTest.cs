using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Variables
{
    [TestClass]
    public sealed class VariablesTest : TestFixture
    {
        [TestMethod]
        public void TestMethod1()
        {
             Analyze("VariablesExample.cs");
        }

        [TestMethod]
        public void TestMethod2()
        {
             Analyze("VariablesExample.cs");
        }
    }
}