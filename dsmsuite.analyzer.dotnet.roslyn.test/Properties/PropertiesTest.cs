using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Properties
{
    [TestClass]
    public class PropertiesTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            HierarchicalGraph hierarchicalGraph = RoslynTestFixture.Analyze("PropertiesExample.cs");
        }

        [TestMethod]
        public void TestMethod2()
        {
            HierarchicalGraph hierarchicalGraph = RoslynTestFixture.Analyze("PropertiesExample.cs");
        }
    }
}
