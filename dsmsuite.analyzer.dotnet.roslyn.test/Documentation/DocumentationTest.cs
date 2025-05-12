using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Documentation
{
    [TestClass]
    public class DocumentationTest : TestFixture
    {
        [TestMethod]
        public void TestMethod1()
        {
           Analyze("DocumentationExample.cs");
        }

        [TestMethod]
        public void TestMethod2()
        {
             Analyze("DocumentationExample.cs");        }
    }
}
