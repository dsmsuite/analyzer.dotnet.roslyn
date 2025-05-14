using dsmsuite.analyzer.dotnet.roslyn.Graph;

namespace dsmsuite.analyzer.dotnet.roslyn.test.MetaData
{
    [TestClass]
    public class MetaDataTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("MetaDataExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("MetaDataExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }

        [TestMethod]
        public void TestNodesExist()
        {
            Analyze("MetaDataExample.cs");

            // TODO: Fix filename not filled in test case

            Assert.IsTrue(NodeExists("MetaData", NodeType.Namespace));
            Assert.IsTrue(NodeCountIs(1, NodeType.Namespace));

            INode? interfaceNode = FindNode("MetaData.IMetaData", NodeType.Interface);
            Assert.IsNotNull(interfaceNode);
            Assert.AreEqual(9, interfaceNode.Startline);
            Assert.AreEqual(12, interfaceNode.Endline);
            Assert.IsTrue(NodeCountIs(1, NodeType.Interface));

            INode? classNode = FindNode("MetaData.MetaDataClass", NodeType.Class);
            Assert.IsNotNull(classNode);
            Assert.AreEqual(14, classNode.Startline);
            Assert.AreEqual(19, classNode.Endline);
            Assert.IsTrue(NodeCountIs(1, NodeType.Class));

            INode? interfaceMethodNode = FindNode("MetaData.IMetaData.InterfaceMethod", NodeType.Method);
            Assert.IsNotNull(interfaceMethodNode);
            Assert.AreEqual(11, interfaceMethodNode.Startline);
            Assert.AreEqual(11, interfaceMethodNode.Endline);

            INode? classMethodNode = FindNode("MetaData.MetaDataClass.InterfaceMethod", NodeType.Method);
            Assert.IsNotNull(classMethodNode);
            Assert.AreEqual(16, classMethodNode.Startline);
            Assert.AreEqual(18, classMethodNode.Endline);
            Assert.IsTrue(NodeCountIs(2, NodeType.Method));
        }

        [TestMethod]
        public void TestEdgesExist()
        {
            Analyze("MetaDataExample.cs");

            // TODO: Fix filename not filled in test case

            IEdge? implementsEdge = FindEdge("MetaData.MetaDataClass", "MetaData.IMetaData", EdgeType.Implements);
            Assert.IsNotNull(implementsEdge);
            Assert.AreEqual(14, implementsEdge.Line);
            Assert.IsTrue(EdgeCountIs(1, EdgeType.Implements));
        }
    }
}
