using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Methods
{
    [TestClass]
    public sealed class MethodCallsTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            HierarchicalGraph hierarchicalGraph = RoslynTestFixture.Analyze("MethodCallsExample.cs");
        }

        [TestMethod]
        public void TestMethod2()
        {
            HierarchicalGraph hierarchicalGraph = RoslynTestFixture.Analyze("MethodCallsExample.cs");
        }
    }
}
