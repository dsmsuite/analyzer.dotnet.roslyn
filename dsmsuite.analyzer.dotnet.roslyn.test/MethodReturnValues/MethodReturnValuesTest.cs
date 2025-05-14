using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;
using dsmsuite.analyzer.dotnet.roslyn.Graph;

namespace dsmsuite.analyzer.dotnet.roslyn.test.MethodReturnValues
{
    [TestClass]
    public sealed class MethodReturnValuesTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("MethodReturnValuesExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("MethodReturnValuesExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }

        [TestMethod]
        public void TestNodesExist()
        {
             Analyze("MethodReturnValuesExample.cs");

            Assert.IsTrue(NodeExists("MethodReturnValues", NodeType.Namespace));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderStruct", NodeType.Struct));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderStruct..ctor", NodeType.Constructor));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderStruct.structMember1", NodeType.Field));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderStruct.structMember2", NodeType.Field));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderEnum", NodeType.Enum));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderEnum.enumVal1", NodeType.EnumValue));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderEnum.enumVal2", NodeType.EnumValue));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderClass", NodeType.Class));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderClass.ProviderClassMethod", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderListTemplateArgument", NodeType.Class));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderListTemplateArgument.ProviderListTemplateArgumentMethod", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderTemplateArgument1", NodeType.Class));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderTemplateArgument1.ProviderTemplateArgument1Method", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderTemplateArgument2", NodeType.Class));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderTemplateArgument2.ProviderTemplateArgument2Method", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderGenericClass", NodeType.Class));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderGenericClass.T", NodeType.TypeParameter));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderGenericClass.U", NodeType.TypeParameter));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderGenericClass.GetFirstTemplateArgument", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodReturnValues.ProviderGenericClass.GetSecondTemplateArgument", NodeType.Method));

            Assert.IsTrue(NodeExists("MethodReturnValues.ReturnTypeConsumer", NodeType.Class));
            Assert.IsTrue(NodeExists("MethodReturnValues.ReturnTypeConsumer.MethodWithReturnTypeVoid", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodReturnValues.ReturnTypeConsumer.MethodWithReturnTypeInt", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodReturnValues.ReturnTypeConsumer.MethodWithGenericReturnTypeEnum", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodReturnValues.ReturnTypeConsumer.MethodWithGenericReturnTypeStruct", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodReturnValues.ReturnTypeConsumer.MethodWithReturnTypeClass", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodReturnValues.ReturnTypeConsumer.MethodWithReturnTypeNullableClass", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodReturnValues.ReturnTypeConsumer.MethodWithListReturnType", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodReturnValues.ReturnTypeConsumer.MethodWithGenericClassReturnType", NodeType.Method));

        }

        [TestMethod]
        public void TestEdgesExist()
        {
             Analyze("MethodReturnValuesExample.cs");

            Assert.IsTrue(EdgeExists("MethodReturnValues.ProviderGenericClass.GetFirstTemplateArgument", "MethodReturnValues.ProviderGenericClass.T", EdgeType.ReturnType));
            Assert.IsTrue(EdgeExists("MethodReturnValues.ProviderGenericClass.GetSecondTemplateArgument", "MethodReturnValues.ProviderGenericClass.U", EdgeType.ReturnType));
            Assert.IsTrue(EdgeExists("MethodReturnValues.ReturnTypeConsumer.MethodWithGenericReturnTypeEnum", "MethodReturnValues.ProviderEnum", EdgeType.ReturnType));
            Assert.IsTrue(EdgeExists("MethodReturnValues.ReturnTypeConsumer.MethodWithGenericReturnTypeStruct", "MethodReturnValues.ProviderStruct", EdgeType.ReturnType));
            Assert.IsTrue(EdgeExists("MethodReturnValues.ReturnTypeConsumer.MethodWithReturnTypeClass", "MethodReturnValues.ProviderClass", EdgeType.ReturnType));
            Assert.IsTrue(EdgeExists("MethodReturnValues.ReturnTypeConsumer.MethodWithReturnTypeNullableClass", "MethodReturnValues.ProviderClass", EdgeType.ReturnType));


        }
    }
}
