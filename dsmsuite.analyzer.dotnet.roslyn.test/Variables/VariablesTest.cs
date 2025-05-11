using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Variables
{
    [TestClass]
    public sealed class VariablesTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            HierarchicalGraph hierarchicalGraph = RoslynTestFixture.Analyze("VariablesExample.cs");
        }

        [TestMethod]
        public void TestMethod2()
        {
            HierarchicalGraph hierarchicalGraph = RoslynTestFixture.Analyze("VariablesExample.cs");
        }
    }
}