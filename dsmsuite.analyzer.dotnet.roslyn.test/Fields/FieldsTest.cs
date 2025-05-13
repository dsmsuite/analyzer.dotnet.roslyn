using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Fields
{

    [TestClass]
    public class FieldsTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("FieldsExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("FieldsExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }

        [TestMethod]
        public void TestMethod1()
        {
             Analyze("FieldsExample.cs");
        }

        [TestMethod]
        public void TestMethod2()
        {
             Analyze("FieldsExample.cs");
        }
    }
}
