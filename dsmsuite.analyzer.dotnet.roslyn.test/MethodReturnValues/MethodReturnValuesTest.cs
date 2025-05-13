using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.MethodReturnValues
{
    [TestClass]
    public sealed class MethodReturnValuesTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("MethodReturnValuesExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("MethodReturnValuesExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }

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
