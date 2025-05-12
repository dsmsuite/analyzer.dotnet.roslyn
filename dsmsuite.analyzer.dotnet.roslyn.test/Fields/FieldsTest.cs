using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Fields
{

    [TestClass]
    public class FieldsTest : TestFixture
    {
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
