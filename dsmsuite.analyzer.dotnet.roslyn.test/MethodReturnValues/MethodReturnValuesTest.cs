using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.MethodReturnValues
{
    [TestClass]
    public sealed class MethodReturnValuesTest : TestFixture
    {
        [TestMethod]
        public void TestMethod1()
        {
             Analyze("MethodReturnValuesExample.cs");
        }

        [TestMethod]
        public void TestMethod2()
        {
             Analyze("MethodReturnValuesExample.cs");
        }
    }
}
