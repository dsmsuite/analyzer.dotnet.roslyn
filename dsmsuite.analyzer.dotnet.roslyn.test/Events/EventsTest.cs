using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Events
{
    [TestClass]
    public class EventsTest : TestFixture
    {
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
