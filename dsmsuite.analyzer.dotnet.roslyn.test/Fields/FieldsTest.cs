using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;
using dsmsuite.analyzer.dotnet.roslyn.Graph;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Fields
{

    [TestClass]
    public class FieldsTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("FieldsExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("FieldsExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }

        [TestMethod]
        public void TestNodesExist()
        {
             Analyze("FieldsExample.cs");

            Assert.IsTrue(NodeExists("Fields", NodeType.Namespace));
            Assert.IsTrue(NodeExists("Fields.ProviderStruct", NodeType.Struct));
            Assert.IsTrue(NodeExists("Fields.ProviderStruct..ctor", NodeType.Constructor));
            Assert.IsTrue(NodeExists("Fields.ProviderStruct.structMember1", NodeType.Field));
            Assert.IsTrue(NodeExists("Fields.ProviderStruct.structMember2", NodeType.Field));
            Assert.IsTrue(NodeExists("Fields.ProviderEnum", NodeType.Enum));
            Assert.IsTrue(NodeExists("Fields.ProviderEnum.enumVal1", NodeType.EnumValue));
            Assert.IsTrue(NodeExists("Fields.ProviderEnum.enumVal2", NodeType.EnumValue));
            Assert.IsTrue(NodeExists("Fields.ProviderClass", NodeType.Class));
            Assert.IsTrue(NodeExists("Fields.ProviderClass.ProviderClassMethod", NodeType.Method));
            Assert.IsTrue(NodeExists("Fields.ProviderListTemplateArgument", NodeType.Class));
            Assert.IsTrue(NodeExists("Fields.ProviderListTemplateArgument.ProviderListTemplateArgumentMethod", NodeType.Method));
            Assert.IsTrue(NodeExists("Fields.ProviderTemplateArgument1", NodeType.Class));
            Assert.IsTrue(NodeExists("Fields.ProviderTemplateArgument1.ProviderTemplateArgument1Method", NodeType.Method));
            Assert.IsTrue(NodeExists("Fields.ProviderTemplateArgument2", NodeType.Class));
            Assert.IsTrue(NodeExists("Fields.ProviderTemplateArgument2.ProviderTemplateArgument2Method", NodeType.Method));
            Assert.IsTrue(NodeExists("Fields.ProviderGenericClass", NodeType.Class));
            Assert.IsTrue(NodeExists("Fields.ProviderGenericClass.T", NodeType.TypeParameter));
            Assert.IsTrue(NodeExists("Fields.ProviderGenericClass.U", NodeType.TypeParameter));
            Assert.IsTrue(NodeExists("Fields.ProviderGenericClass.GetFirstTemplateArgument", NodeType.Method));
            Assert.IsTrue(NodeExists("Fields.ProviderGenericClass.GetSecondTemplateArgument", NodeType.Method));

            Assert.IsTrue(NodeExists("Fields.FieldConsumer", NodeType.Class));
            Assert.IsTrue(NodeExists("Fields.FieldConsumer..ctor", NodeType.Constructor));
            Assert.IsTrue(NodeExists("Fields.FieldConsumer.MethodUsingIntMember", NodeType.Method));
            Assert.IsTrue(NodeExists("Fields.FieldConsumer.MethodUsingEnumMember", NodeType.Method));
            Assert.IsTrue(NodeExists("Fields.FieldConsumer.MethodUsingStructMember", NodeType.Method));
            Assert.IsTrue(NodeExists("Fields.FieldConsumer.MethodUsingClassMember", NodeType.Method));
            Assert.IsTrue(NodeExists("Fields.FieldConsumer.MethodUsingStdListMember", NodeType.Method));
            Assert.IsTrue(NodeExists("Fields.FieldConsumer.MethodUsingGenericClassMember", NodeType.Method));
            Assert.IsTrue(NodeExists("Fields.FieldConsumer._intMember", NodeType.Field));
            Assert.IsTrue(NodeExists("Fields.FieldConsumer._enumMember", NodeType.Field));
            Assert.IsTrue(NodeExists("Fields.FieldConsumer._structMember", NodeType.Field));
            Assert.IsTrue(NodeExists("Fields.FieldConsumer._classMember", NodeType.Field));
            Assert.IsTrue(NodeExists("Fields.FieldConsumer._listClassMember", NodeType.Field));
            Assert.IsTrue(NodeExists("Fields.FieldConsumer._genericClassMember", NodeType.Field));
        }

        [TestMethod]
        public void TestEdgesExist()
        {
             Analyze("FieldsExample.cs");

            Assert.IsTrue(EdgeExists("Fields.ProviderGenericClass.GetFirstTemplateArgument", "Fields.ProviderGenericClass.T", EdgeType.ReturnType));
            Assert.IsTrue(EdgeExists("Fields.ProviderGenericClass.GetSecondTemplateArgument", "Fields.ProviderGenericClass.U", EdgeType.ReturnType));
            Assert.IsTrue(EdgeExists("Fields.FieldConsumer.MethodUsingClassMember", "Fields.ProviderClass.ProviderClassMethod", EdgeType.Call));
            Assert.IsTrue(EdgeExists("Fields.FieldConsumer.MethodUsingStdListMember", "Fields.ProviderListTemplateArgument.ProviderListTemplateArgumentMethod", EdgeType.Call));
            Assert.IsTrue(EdgeExists("Fields.FieldConsumer.MethodUsingGenericClassMember", "Fields.ProviderTemplateArgument1.ProviderTemplateArgument1Method", EdgeType.Call));
            Assert.IsTrue(EdgeExists("Fields.FieldConsumer.MethodUsingGenericClassMember", "Fields.ProviderTemplateArgument2.ProviderTemplateArgument2Method", EdgeType.Call));


        }
    }
}
