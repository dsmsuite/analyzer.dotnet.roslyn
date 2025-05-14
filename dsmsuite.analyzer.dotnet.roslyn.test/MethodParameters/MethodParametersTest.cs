using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;
using dsmsuite.analyzer.dotnet.roslyn.Graph;

namespace dsmsuite.analyzer.dotnet.roslyn.test.MethodParameters
{

    [TestClass]
    public sealed class MethodParametersTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("MethodParametersExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("MethodParametersExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }

        [TestMethod]
        public void TestNodesExist()
        {
             Analyze("MethodParametersExample.cs");

            Assert.IsTrue(NodeExists("MethodParameters", NodeType.Namespace));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderStruct", NodeType.Struct));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderStruct..ctor", NodeType.Constructor));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderStruct.structMember1", NodeType.Field));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderStruct.structMember2", NodeType.Field));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderEnum", NodeType.Enum));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderEnum.enumVal1", NodeType.EnumValue));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderEnum.enumVal2", NodeType.EnumValue));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderClass", NodeType.Class));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderClass.ProviderClassMethod", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderListTemplateArgument", NodeType.Class));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderListTemplateArgument.ProviderListTemplateArgumentMethod", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderTemplateArgument1", NodeType.Class));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderTemplateArgument1.ProviderTemplateArgument1Method", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderTemplateArgument2", NodeType.Class));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderTemplateArgument2.ProviderTemplateArgument2Method", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderGenericClass", NodeType.Class));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderGenericClass.T", NodeType.TypeParameter));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderGenericClass.U", NodeType.TypeParameter));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderGenericClass.GetFirstTemplateArgument", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodParameters.ProviderGenericClass.GetSecondTemplateArgument", NodeType.Method));

            Assert.IsTrue(NodeExists("MethodParameters.ParameterConsumer", NodeType.Class));
            Assert.IsTrue(NodeExists("MethodParameters.ParameterConsumer.MethodWithIntParameter", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodParameters.ParameterConsumer.MethodWithEnumParameter", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodParameters.ParameterConsumer.MethodWithStructParameter", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodParameters.ParameterConsumer.MethodWithClassParameter", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodParameters.ParameterConsumer.MethodWithNullableClassParameter", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodParameters.ParameterConsumer.MethodWithStdListParameter", NodeType.Method));
            Assert.IsTrue(NodeExists("MethodParameters.ParameterConsumer.MethodWithGenericClassParameter", NodeType.Method));
        }

        [TestMethod]
        public void TestEdgesExist()
        {
             Analyze("MethodParametersExample.cs");

            Assert.IsTrue(EdgeExists("MethodParameters.ProviderGenericClass.GetFirstTemplateArgument", "MethodParameters.ProviderGenericClass.T", EdgeType.ReturnType));
            Assert.IsTrue(EdgeExists("MethodParameters.ProviderGenericClass.GetSecondTemplateArgument", "MethodParameters.ProviderGenericClass.U", EdgeType.ReturnType));
            Assert.IsTrue(EdgeExists("MethodParameters.ParameterConsumer.MethodWithClassParameter", "MethodParameters.ProviderClass.ProviderClassMethod", EdgeType.Call));
            Assert.IsTrue(EdgeExists("MethodParameters.ParameterConsumer.MethodWithNullableClassParameter", "MethodParameters.ProviderClass.ProviderClassMethod", EdgeType.Call));
            Assert.IsTrue(EdgeExists("MethodParameters.ParameterConsumer.MethodWithStdListParameter", "MethodParameters.ProviderListTemplateArgument.ProviderListTemplateArgumentMethod", EdgeType.Call));
            Assert.IsTrue(EdgeExists("MethodParameters.ParameterConsumer.MethodWithGenericClassParameter", "MethodParameters.ProviderTemplateArgument1.ProviderTemplateArgument1Method", EdgeType.Call));
            Assert.IsTrue(EdgeExists("MethodParameters.ParameterConsumer.MethodWithGenericClassParameter", "MethodParameters.ProviderTemplateArgument2.ProviderTemplateArgument2Method", EdgeType.Call));


        }
    }
}
