using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;
using dsmsuite.analyzer.dotnet.roslyn.Graph;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Properties
{
    [TestClass]
    public class PropertiesTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("PropertiesExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("PropertiesExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }

        [TestMethod]
        public void TestNodesExist()
        {
            Analyze("PropertiesExample.cs");

            Assert.IsTrue(NodeExists("Properties", NodeType.Namespace));
            Assert.IsTrue(NodeExists("Properties.ProviderStruct", NodeType.Struct));
            Assert.IsTrue(NodeExists("Properties.ProviderStruct..ctor", NodeType.Constructor));
            Assert.IsTrue(NodeExists("Properties.ProviderStruct.structMember1", NodeType.Field));
            Assert.IsTrue(NodeExists("Properties.ProviderStruct.structMember2", NodeType.Field));
            Assert.IsTrue(NodeExists("Properties.ProviderEnum", NodeType.Enum));
            Assert.IsTrue(NodeExists("Properties.ProviderEnum.enumVal1", NodeType.EnumValue));
            Assert.IsTrue(NodeExists("Properties.ProviderEnum.enumVal2", NodeType.EnumValue));
            Assert.IsTrue(NodeExists("Properties.ProviderClass", NodeType.Class));
            Assert.IsTrue(NodeExists("Properties.ProviderClass.ProviderClassMethod", NodeType.Method));
            Assert.IsTrue(NodeExists("Properties.ProviderListTemplateArgument", NodeType.Class));
            Assert.IsTrue(NodeExists("Properties.ProviderListTemplateArgument.ProviderListTemplateArgumentMethod", NodeType.Method));
            Assert.IsTrue(NodeExists("Properties.ProviderTemplateArgument1", NodeType.Class));
            Assert.IsTrue(NodeExists("Properties.ProviderTemplateArgument1.ProviderTemplateArgument1Method", NodeType.Method));
            Assert.IsTrue(NodeExists("Properties.ProviderTemplateArgument2", NodeType.Class));
            Assert.IsTrue(NodeExists("Properties.ProviderTemplateArgument2.ProviderTemplateArgument2Method", NodeType.Method));
            Assert.IsTrue(NodeExists("Properties.ProviderGenericClass", NodeType.Class));
            Assert.IsTrue(NodeExists("Properties.ProviderGenericClass.T", NodeType.TypeParameter));
            Assert.IsTrue(NodeExists("Properties.ProviderGenericClass.U", NodeType.TypeParameter));
            Assert.IsTrue(NodeExists("Properties.ProviderGenericClass.GetFirstTemplateArgument", NodeType.Method));
            Assert.IsTrue(NodeExists("Properties.ProviderGenericClass.GetSecondTemplateArgument", NodeType.Method));

            Assert.IsTrue(NodeExists("Properties.PropertyConsumer", NodeType.Class));
            Assert.IsTrue(NodeExists("Properties.PropertyConsumer..ctor", NodeType.Constructor));
            Assert.IsTrue(NodeExists("Properties.PropertyConsumer.MethodUsingIntMember", NodeType.Method));
            Assert.IsTrue(NodeExists("Properties.PropertyConsumer.MethodUsingEnumMember", NodeType.Method));
            Assert.IsTrue(NodeExists("Properties.PropertyConsumer.MethodUsingStructMember", NodeType.Method));
            Assert.IsTrue(NodeExists("Properties.PropertyConsumer.MethodUsingClassMember", NodeType.Method));
            Assert.IsTrue(NodeExists("Properties.PropertyConsumer.MethodUsingStdListMember", NodeType.Method));
            Assert.IsTrue(NodeExists("Properties.PropertyConsumer.MethodUsingGenericClassMember", NodeType.Method));
            Assert.IsTrue(NodeExists("Properties.PropertyConsumer._intProtertyBackingField", NodeType.Field));
            Assert.IsTrue(NodeExists("Properties.PropertyConsumer.IntPropertyWithBackingField", NodeType.Property));
            Assert.IsTrue(NodeExists("Properties.PropertyConsumer.IntProperty", NodeType.Property));
            Assert.IsTrue(NodeExists("Properties.PropertyConsumer.EnumProperty", NodeType.Property));
            Assert.IsTrue(NodeExists("Properties.PropertyConsumer.StructProperty", NodeType.Property));
            Assert.IsTrue(NodeExists("Properties.PropertyConsumer.ClassProperty", NodeType.Property));
            Assert.IsTrue(NodeExists("Properties.PropertyConsumer.ListClassProperty", NodeType.Property));
            Assert.IsTrue(NodeExists("Properties.PropertyConsumer.GenericClassProperty", NodeType.Property));
        }

        [TestMethod]
        public void TestEdgesExist()
        {
            Analyze("PropertiesExample.cs");

            Assert.IsTrue(EdgeExists("Properties.ProviderGenericClass.GetFirstTemplateArgument", "Properties.ProviderGenericClass.T", EdgeType.ReturnType));
            Assert.IsTrue(EdgeExists("Properties.ProviderGenericClass.GetSecondTemplateArgument", "Properties.ProviderGenericClass.U", EdgeType.ReturnType));
            Assert.IsTrue(EdgeExists("Properties.PropertyConsumer.MethodUsingClassMember", "Properties.ProviderClass.ProviderClassMethod", EdgeType.Call));
            Assert.IsTrue(EdgeExists("Properties.PropertyConsumer.MethodUsingStdListMember", "Properties.ProviderListTemplateArgument.ProviderListTemplateArgumentMethod", EdgeType.Call));
            Assert.IsTrue(EdgeExists("Properties.PropertyConsumer.MethodUsingGenericClassMember", "Properties.ProviderTemplateArgument1.ProviderTemplateArgument1Method", EdgeType.Call));
            Assert.IsTrue(EdgeExists("Properties.PropertyConsumer.MethodUsingGenericClassMember", "Properties.ProviderTemplateArgument2.ProviderTemplateArgument2Method", EdgeType.Call));


        }
    }
}
