using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;
using dsmsuite.analyzer.dotnet.roslyn.Graph;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Methods
{
    [TestClass]
    public sealed class MethodCallsTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("MethodCallsExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("MethodCallsExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }

        [TestMethod]
        public void TestNodesExist()
        {
             Analyze("MethodCallsExample.cs");

            Assert.IsTrue(NodeExists("Methods", NodeType.Namespace));
            Assert.IsTrue(NodeExists("Methods.ProviderStruct", NodeType.Struct));
            Assert.IsTrue(NodeExists("Methods.ProviderStruct..ctor", NodeType.Constructor));
            Assert.IsTrue(NodeExists("Methods.ProviderStruct.structMember1", NodeType.Field));
            Assert.IsTrue(NodeExists("Methods.ProviderStruct.structMember2", NodeType.Field));
            Assert.IsTrue(NodeExists("Methods.ProviderEnum", NodeType.Enum));
            Assert.IsTrue(NodeExists("Methods.ProviderEnum.enumVal1", NodeType.EnumValue));
            Assert.IsTrue(NodeExists("Methods.ProviderEnum.enumVal2", NodeType.EnumValue));
            Assert.IsTrue(NodeExists("Methods.ProviderClass", NodeType.Class));
            Assert.IsTrue(NodeExists("Methods.ProviderClass.ProviderClassMethod", NodeType.Method));
            Assert.IsTrue(NodeExists("Methods.ProviderListTemplateArgument", NodeType.Class));
            Assert.IsTrue(NodeExists("Methods.ProviderListTemplateArgument.ProviderListTemplateArgumentMethod", NodeType.Method));
            Assert.IsTrue(NodeExists("Methods.ProviderTemplateArgument1", NodeType.Class));
            Assert.IsTrue(NodeExists("Methods.ProviderTemplateArgument1.ProviderTemplateArgument1Method", NodeType.Method));
            Assert.IsTrue(NodeExists("Methods.ProviderTemplateArgument2", NodeType.Class));
            Assert.IsTrue(NodeExists("Methods.ProviderTemplateArgument2.ProviderTemplateArgument2Method", NodeType.Method));
            Assert.IsTrue(NodeExists("Methods.ProviderGenericClass", NodeType.Class));
            Assert.IsTrue(NodeExists("Methods.ProviderGenericClass.T", NodeType.TypeParameter));
            Assert.IsTrue(NodeExists("Methods.ProviderGenericClass.U", NodeType.TypeParameter));
            Assert.IsTrue(NodeExists("Methods.ProviderGenericClass.GetFirstTemplateArgument", NodeType.Method));
            Assert.IsTrue(NodeExists("Methods.ProviderGenericClass.GetSecondTemplateArgument", NodeType.Method));

            Assert.IsTrue(NodeExists("Methods.ParameterConsumer", NodeType.Class));
            Assert.IsTrue(NodeExists("Methods.ParameterConsumer.MethodWithIntParameter", NodeType.Method));
            Assert.IsTrue(NodeExists("Methods.ParameterConsumer.MethodWithEnumParameter", NodeType.Method));
            Assert.IsTrue(NodeExists("Methods.ParameterConsumer.MethodWithStructParameter", NodeType.Method));
            Assert.IsTrue(NodeExists("Methods.ParameterConsumer.MethodWithClassParameter", NodeType.Method));
            Assert.IsTrue(NodeExists("Methods.ParameterConsumer.MethodWithNullableClassParameter", NodeType.Method));
            Assert.IsTrue(NodeExists("Methods.ParameterConsumer.MethodWithStdListParameter", NodeType.Method));
            Assert.IsTrue(NodeExists("Methods.ParameterConsumer.MethodWithGenericClassParameter", NodeType.Method));
        }

        [TestMethod]
        public void TestEdgesExist()
        {
             Analyze("MethodCallsExample.cs");

            Assert.IsTrue(EdgeExists("Methods.ProviderGenericClass.GetFirstTemplateArgument", "Methods.ProviderGenericClass.T", EdgeType.ReturnType));
            Assert.IsTrue(EdgeExists("Methods.ProviderGenericClass.GetSecondTemplateArgument", "Methods.ProviderGenericClass.U", EdgeType.ReturnType));
            Assert.IsTrue(EdgeExists("Methods.ParameterConsumer.MethodWithClassParameter", "Methods.ProviderClass.ProviderClassMethod", EdgeType.Call));
            Assert.IsTrue(EdgeExists("Methods.ParameterConsumer.MethodWithNullableClassParameter", "Methods.ProviderClass.ProviderClassMethod", EdgeType.Call));
            Assert.IsTrue(EdgeExists("Methods.ParameterConsumer.MethodWithStdListParameter", "Methods.ProviderListTemplateArgument.ProviderListTemplateArgumentMethod", EdgeType.Call));
            Assert.IsTrue(EdgeExists("Methods.ParameterConsumer.MethodWithGenericClassParameter", "Methods.ProviderTemplateArgument1.ProviderTemplateArgument1Method", EdgeType.Call));
            Assert.IsTrue(EdgeExists("Methods.ParameterConsumer.MethodWithGenericClassParameter", "Methods.ProviderTemplateArgument2.ProviderTemplateArgument2Method", EdgeType.Call));


        }
    }
}
