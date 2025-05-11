using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Events
{
    [TestClass]
    public class EventsTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            HierarchicalGraph hierarchicalGraph = RoslynTestFixture.Analyze("EventsExample.cs");
        }

        [TestMethod]
        public void TestMethod2()
        {
            HierarchicalGraph hierarchicalGraph = RoslynTestFixture.Analyze("EventsExample.cs");
        }
    }
}
