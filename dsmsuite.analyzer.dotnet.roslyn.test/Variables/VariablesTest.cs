using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Variables
{
    [TestClass]
    public sealed class VariablesTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("VariablesExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("VariablesExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }

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