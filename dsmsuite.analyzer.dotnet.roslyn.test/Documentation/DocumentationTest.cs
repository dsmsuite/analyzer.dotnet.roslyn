using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;
using dsmsuite.analyzer.dotnet.roslyn.Graph;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Documentation
{
    [TestClass]
    public class DocumentationTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("DocumentationExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("DocumentationExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }


        [TestMethod]
        public void TestNodeHaveCorrectComments()
        {
            Analyze("DocumentationExample.cs");

            Assert.IsTrue(NodeExists("Documentation", NodeType.Namespace));
            Assert.IsTrue(NodeCountIs(1, NodeType.Namespace));

            INode? classNode = FindNode("Documentation.DocumentatedClass", NodeType.Class);
            Assert.IsNotNull(classNode);
            Assert.AreEqual("// Class with leading comments", classNode.Comment);

            INode? methodNode = FindNode("Documentation.DocumentatedClass.MethodWitLeadingComments", NodeType.Method);
            Assert.IsNotNull(methodNode);
            Assert.AreEqual( "// Method with leading comments", methodNode.Comment);
            Assert.IsTrue(NodeCountIs(1, NodeType.Method));

            INode? propertyNode1 = FindNode("Documentation.DocumentatedClass.PropertWitLeadingComments", NodeType.Property);
            Assert.IsNotNull(propertyNode1);
            Assert.AreEqual("// Property with leading comments", propertyNode1.Comment);
            INode? propertyNode2 = FindNode("Documentation.DocumentatedClass.PropertWitTrailingComments", NodeType.Property);
            Assert.IsNotNull(propertyNode2);
            Assert.AreEqual("// Property with trailing comments", propertyNode2.Comment);
            Assert.IsTrue(NodeCountIs(2, NodeType.Property));

            INode? fieldNode1 = FindNode("Documentation.DocumentatedClass._fieldWithLeadingComments", NodeType.Field);
            Assert.IsNotNull(fieldNode1);
            Assert.AreEqual("// Field with leading comments", fieldNode1.Comment);
            INode? fieldNode2 = FindNode("Documentation.DocumentatedClass._fieldWithTrailingComments", NodeType.Field);
            Assert.IsNotNull(fieldNode2);
            Assert.AreEqual("// Field with trailing comments", fieldNode2.Comment);
            Assert.IsTrue(NodeCountIs(2, NodeType.Field));

            INode? eventNode1 = FindNode("Documentation.DocumentatedClass.EventWithLeadingComments", NodeType.Event);
            Assert.IsNotNull(eventNode1);
            Assert.AreEqual("// Event with leading comments", eventNode1.Comment);
            INode? eventNode2 = FindNode("Documentation.DocumentatedClass.EventWithTrailingComments", NodeType.Event);
            Assert.IsNotNull(eventNode2);
            Assert.AreEqual("// Event with trailing comments", eventNode2.Comment);
            Assert.IsTrue(NodeCountIs(2, NodeType.Event));
        }

    }
}
