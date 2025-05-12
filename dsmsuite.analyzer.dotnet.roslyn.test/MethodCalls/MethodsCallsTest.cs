using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Methods
{
    [TestClass]
    public sealed class MethodCallsTest : TestFixture
    {
        [TestMethod]
        public void TestMethod1()
        {
             Analyze("MethodCallsExample.cs");
        }

        [TestMethod]
        public void TestMethod2()
        {
             Analyze("MethodCallsExample.cs");
        }
    }
}
