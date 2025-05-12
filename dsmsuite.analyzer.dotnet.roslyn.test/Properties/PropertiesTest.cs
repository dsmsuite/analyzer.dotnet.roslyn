using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Properties
{
    [TestClass]
    public class PropertiesTest : TestFixture
    {
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
