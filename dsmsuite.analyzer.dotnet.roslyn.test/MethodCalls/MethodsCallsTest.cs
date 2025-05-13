using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Methods
{
    [TestClass]
    public sealed class MethodCallsTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("MethodCallsExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("MethodCallsExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }

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
