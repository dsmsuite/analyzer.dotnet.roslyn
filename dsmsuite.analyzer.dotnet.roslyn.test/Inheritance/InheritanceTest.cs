using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Inheritance
{
    [TestClass]
    public sealed class InheritanceTest : TestFixture
    {
        [TestMethod]
        public void TestMethod1()
        {
             Analyze("InheritanceExample.cs");
        }

        [TestMethod]
        public void TestMethod2()
        {
             Analyze("InheritanceExample.cs");
        }
    }
}