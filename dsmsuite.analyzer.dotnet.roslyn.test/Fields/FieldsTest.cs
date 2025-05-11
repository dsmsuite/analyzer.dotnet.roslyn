using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Fields
{
    [TestClass]
    public class FieldsTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            HierarchicalGraph hierarchicalGraph = RoslynTestFixture.Analyze("FieldsExample.cs");
        }

        [TestMethod]
        public void TestMethod2()
        {
            HierarchicalGraph hierarchicalGraph = RoslynTestFixture.Analyze("FieldsExample.cs");
        }
    }
}
