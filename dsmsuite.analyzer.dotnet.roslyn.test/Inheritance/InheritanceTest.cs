using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;
using dsmsuite.analyzer.dotnet.roslyn.Graph;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Inheritance
{
    [TestClass]
    public sealed class InheritanceTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("InheritanceExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("InheritanceExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }

        [TestMethod]
        public void TestNodesExist()
        {
            Analyze("InheritanceExample.cs");

            Assert.IsTrue(NodeExists("Inheritance", NodeType.Namespace));
            Assert.IsTrue(NodeCountIs(1, NodeType.Namespace));

            Assert.IsTrue(NodeExists("Inheritance.BaseClass", NodeType.Class));
            Assert.IsTrue(NodeExists("Inheritance.DerivedClass", NodeType.Class));
            Assert.IsTrue(NodeCountIs(2, NodeType.Class));

            Assert.IsTrue(NodeExists("Inheritance.BaseClass.BaseMethod", NodeType.Method));
            Assert.IsTrue(NodeExists("Inheritance.DerivedClass.BaseMethod", NodeType.Method));
            Assert.IsTrue(NodeExists("Inheritance.DerivedClass.DerivedMethod", NodeType.Method));
            Assert.IsTrue(NodeCountIs(3, NodeType.Method));
        }

        [TestMethod]
        public void TestEdgesExist()
        {
            Analyze("InheritanceExample.cs");

            Assert.IsTrue(EdgeExists("Inheritance.DerivedClass", "Inheritance.BaseClass", EdgeType.InheritsFrom));
            Assert.IsTrue(EdgeCountIs(1, EdgeType.InheritsFrom));

            Assert.IsTrue(EdgeExists("Inheritance.DerivedClass.BaseMethod", "Inheritance.BaseClass.BaseMethod", EdgeType.Overrride));
            Assert.IsTrue(EdgeCountIs(1, EdgeType.Overrride));
        }
    }
}