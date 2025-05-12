using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Mono.Cecil;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Interfacing
{
    [TestClass]
    public sealed class InterfacingTest : TestFixture
    {
        [TestMethod]
        public void TestNodesExist()
        {
            Analyze("InterfacingExample.cs");

            Assert.IsTrue(NodeExists("Interfacing", NodeType.Namespace));
            Assert.IsTrue(NodeExists("Interfacing.Interface1", NodeType.Interface));
            Assert.IsTrue(NodeExists("Interfacing.Interface1.Interface1Method", NodeType.Method));
            Assert.IsTrue(NodeExists("Interfacing.Interface2", NodeType.Interface));
            Assert.IsTrue(NodeExists("Interfacing.Interface2.Interface2Method", NodeType.Method));
            Assert.IsTrue(NodeExists("Interfacing.InterfaceImplemation", NodeType.Class));
            Assert.IsTrue(NodeExists("Interfacing.InterfaceImplemation.Interface1Method", NodeType.Method));
            Assert.IsTrue(NodeExists("Interfacing.InterfaceImplemation.Interface2Method", NodeType.Method));
            Assert.IsTrue(NodeCountIs(8));
        }

        [TestMethod]
        public void TestEdgesExist()
        {
            Analyze("InterfacingExample.cs");

            Assert.IsTrue(EdgeExists("Interfacing.InterfaceImplemation", "Interfacing.Interface1", EdgeType.Implements));
            Assert.IsTrue(EdgeExists("Interfacing.InterfaceImplemation", "Interfacing.Interface1", EdgeType.Implements));
            Assert.IsTrue(EdgeCountIs(2));
        }
    }
}
