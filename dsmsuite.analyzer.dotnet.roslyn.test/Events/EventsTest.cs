using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Events
{
    [TestClass]
    public class EventsTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("EventsExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("EventsExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }

        [TestMethod]
        public void TestMethod1()
        {
             Analyze("EventsExample.cs");
        }

        [TestMethod]
        public void TestMethod2()
        {
             Analyze("EventsExample.cs");
        }
    }
}
