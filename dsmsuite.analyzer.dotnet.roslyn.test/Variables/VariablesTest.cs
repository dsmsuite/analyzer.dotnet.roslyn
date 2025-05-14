using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;
using dsmsuite.analyzer.dotnet.roslyn.Graph;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Variables
{
    [TestClass]
    public sealed class VariablesTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("VariablesExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("VariablesExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }

        [TestMethod]
        public void TestNodesExist()
        {
             Analyze("VariablesExample.cs");

            Assert.IsTrue(NodeExists("Variables", NodeType.Namespace));
            Assert.IsTrue(NodeExists("Variables.ProviderStruct", NodeType.Struct));
            Assert.IsTrue(NodeExists("Variables.ProviderStruct..ctor", NodeType.Constructor));
            Assert.IsTrue(NodeExists("Variables.ProviderStruct.structMember1", NodeType.Field));
            Assert.IsTrue(NodeExists("Variables.ProviderStruct.structMember2", NodeType.Field));
            Assert.IsTrue(NodeExists("Variables.ProviderEnum", NodeType.Enum));
            Assert.IsTrue(NodeExists("Variables.ProviderEnum.enumVal1", NodeType.EnumValue));
            Assert.IsTrue(NodeExists("Variables.ProviderEnum.enumVal2", NodeType.EnumValue));
            Assert.IsTrue(NodeExists("Variables.ProviderClass", NodeType.Class));
            Assert.IsTrue(NodeExists("Variables.ProviderClass.ProviderClassMethod", NodeType.Method));
            Assert.IsTrue(NodeExists("Variables.ProviderListTemplateArgument", NodeType.Class));
            Assert.IsTrue(NodeExists("Variables.ProviderListTemplateArgument.ProviderListTemplateArgumentMethod", NodeType.Method));
            Assert.IsTrue(NodeExists("Variables.ProviderTemplateArgument1", NodeType.Class));
            Assert.IsTrue(NodeExists("Variables.ProviderTemplateArgument1.ProviderTemplateArgument1Method", NodeType.Method));
            Assert.IsTrue(NodeExists("Variables.ProviderTemplateArgument2", NodeType.Class));
            Assert.IsTrue(NodeExists("Variables.ProviderTemplateArgument2.ProviderTemplateArgument2Method", NodeType.Method));
            Assert.IsTrue(NodeExists("Variables.ProviderGenericClass", NodeType.Class));
            Assert.IsTrue(NodeExists("Variables.ProviderGenericClass.T", NodeType.TypeParameter));
            Assert.IsTrue(NodeExists("Variables.ProviderGenericClass.U", NodeType.TypeParameter));
            Assert.IsTrue(NodeExists("Variables.ProviderGenericClass.GetFirstTemplateArgument", NodeType.Method));
            Assert.IsTrue(NodeExists("Variables.ProviderGenericClass.GetSecondTemplateArgument", NodeType.Method));

            Assert.IsTrue(NodeExists("Variables.VariableConsumer", NodeType.Class));
            Assert.IsTrue(NodeExists("Variables.VariableConsumer.MethodUsingIntVariable", NodeType.Method));
            Assert.IsTrue(NodeExists("Variables.VariableConsumer.MethodUsingEnumVariable", NodeType.Method));
            Assert.IsTrue(NodeExists("Variables.VariableConsumer.MethodUsingStructVariable", NodeType.Method));
            Assert.IsTrue(NodeExists("Variables.VariableConsumer.MethodUsingClassVariable", NodeType.Method));
            Assert.IsTrue(NodeExists("Variables.VariableConsumer.MethodUsingStandardGenericContainerListVariable", NodeType.Method));
            Assert.IsTrue(NodeExists("Variables.VariableConsumer.MethodUsingGenericClassVariable", NodeType.Method));
        }

        [TestMethod]
        public void TestEdgesExist()
        {
             Analyze("VariablesExample.cs");

            Assert.IsTrue(EdgeExists("Variables.ProviderGenericClass.GetFirstTemplateArgument", "Variables.ProviderGenericClass.T", EdgeType.ReturnType));
            Assert.IsTrue(EdgeExists("Variables.ProviderGenericClass.GetSecondTemplateArgument", "Variables.ProviderGenericClass.U", EdgeType.ReturnType));
            Assert.IsTrue(EdgeExists("Variables.VariableConsumer.MethodUsingClassVariable", "Variables.ProviderClass.ProviderClassMethod", EdgeType.Call));
            Assert.IsTrue(EdgeExists("Variables.VariableConsumer.MethodUsingStandardGenericContainerListVariable", "Variables.ProviderListTemplateArgument.ProviderListTemplateArgumentMethod", EdgeType.Call));
            Assert.IsTrue(EdgeExists("Variables.VariableConsumer.MethodUsingGenericClassVariable", "Variables.ProviderTemplateArgument1.ProviderTemplateArgument1Method", EdgeType.Call));
            Assert.IsTrue(EdgeExists("Variables.VariableConsumer.MethodUsingGenericClassVariable", "Variables.ProviderTemplateArgument2.ProviderTemplateArgument2Method", EdgeType.Call));


        }
    }
}