using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Properties
{
    [TestClass]
    public class PropertiesTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("PropertiesExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("PropertiesExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }

        [TestMethod]
        public void TestMethod1()
        {
            Analyze("PropertiesExample.cs");
        }

        [TestMethod]
        public void TestMethod2()
        {
            Analyze("PropertiesExample.cs");
        }
    }
}
