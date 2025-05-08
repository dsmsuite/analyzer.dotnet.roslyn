namespace dsmsuite.analyzer.dotnet.roslyn.test
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            RoslynTestFixturecs.Analyz2("C:\\Github\\dsmsuite.analyzer.dotnet.roslyn\\dsmsuite.analyzer.dotnet.roslyn.test\\Test1.cs");
        }

        [TestMethod]
        public void TestMethod2()
        {
            RoslynTestFixturecs.Analyz2("C:\\Github\\dsmsuite.analyzer.dotnet.roslyn\\dsmsuite.analyzer.dotnet.roslyn.test\\Test1.cs");
        }
    }
}
