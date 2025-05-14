using dsmsuite.analyzer.dotnet.roslyn.Analysis.Registration;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using dsmsuite.analyzer.dotnet.roslyn.test;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.Extensions.Logging;
using Mono.Cecil;
using System.Xml.Linq;

namespace Events
{
    [TestClass]
    public class EventsTest : TestFixture
    {
        [TestMethod]
        public void TestNoFailures()
        {
            Analyze("EventsExample.cs");
            Assert.AreEqual(0, FailedCount, "There should be no failures in the analysis.");
        }

        [TestMethod]
        public void TestNoIgnores()
        {
            Analyze("EventsExample.cs");
            Assert.AreEqual(0, IgnoredCount, "There should be no ignores in the analysis.");
        }

        [TestMethod]
        public void TestNodesExist()
        {
            Analyze("EventsExample.cs");

            Assert.IsTrue(NodeExists("Events", NodeType.Namespace));
            Assert.IsTrue(NodeCountIs(1, NodeType.Namespace));

            Assert.IsTrue(NodeExists("Events.ProviderEventArgs", NodeType.Class));
            Assert.IsTrue(NodeExists("Events.ProviderClass", NodeType.Class));
            Assert.IsTrue(NodeExists("Events.EventConsumer", NodeType.Class));
            Assert.IsTrue(NodeCountIs(3, NodeType.Class));

            Assert.IsTrue(NodeExists("Events.IProviderInterface", NodeType.Interface));
            Assert.IsTrue(NodeCountIs(1, NodeType.Interface));

            Assert.IsTrue(NodeExists("Events.ProviderEventArgs.Name", NodeType.Property));
            Assert.IsTrue(NodeCountIs(1, NodeType.Property));

            Assert.IsTrue(NodeExists("Events.IProviderInterface.ProviderChanged", NodeType.Event));
            Assert.IsTrue(NodeExists("Events.ProviderClass.ProviderChanged", NodeType.Event));
            Assert.IsTrue(NodeCountIs(2, NodeType.Event));

            Assert.IsTrue(NodeExists("Events.ProviderClass.TriggerEvent", NodeType.Method));
            Assert.IsTrue(NodeExists("Events.EventConsumer.MethodSubscribeInterfaceEvent", NodeType.Method));
            Assert.IsTrue(NodeExists("Events.EventConsumer.MethodUnsubscribeInterfaceEvent", NodeType.Method));
            Assert.IsTrue(NodeExists("Events.EventConsumer.MethodSubscribeClassEvent", NodeType.Method));
            Assert.IsTrue(NodeExists("Events.EventConsumer.MethodUnsubscribeClassEvent", NodeType.Method));
            Assert.IsTrue(NodeExists("Events.EventConsumer.ProviderChangedEventHandler", NodeType.Method));
            Assert.IsTrue(NodeCountIs(6, NodeType.Method));


        }

        [TestMethod]
        public void TestEdgesExist()
        {
            Analyze("EventsExample.cs");

            Assert.IsTrue(EdgeExists("Events.ProviderClass", "Events.IProviderInterface", EdgeType.Implements));
            Assert.IsTrue(EdgeCountIs(1, EdgeType.Implements));

            Assert.IsTrue(EdgeExists("Events.EventConsumer.ProviderChangedEventHandler", "Events.IProviderInterface.ProviderChanged", EdgeType.HandlEvent));
            Assert.IsTrue(EdgeExists("Events.EventConsumer.ProviderChangedEventHandler", "Events.IProviderInterface.ProviderChanged",  EdgeType.HandlEvent));
            Assert.IsTrue(EdgeExists("Events.EventConsumer.ProviderChangedEventHandler", "Events.ProviderClass.ProviderChanged", EdgeType.HandlEvent));
            Assert.IsTrue(EdgeExists("Events.EventConsumer.ProviderChangedEventHandler", "Events.ProviderClass.ProviderChanged", EdgeType.HandlEvent));
            Assert.IsTrue(EdgeCountIs(4, EdgeType.HandlEvent));

            Assert.IsTrue(EdgeExists("Events.IProviderInterface", "Events.IProviderInterface.ProviderChanged", EdgeType.SubscribeEvent));
            Assert.IsTrue(EdgeExists("Events.IProviderInterface", "Events.ProviderClass.ProviderChanged", EdgeType.SubscribeEvent));
            Assert.IsTrue(EdgeCountIs(1, EdgeType.SubscribeEvent));

            Assert.IsTrue(EdgeExists("Events.ProviderClass", "Events.IProviderInterface.ProviderChanged", EdgeType.UnsubscribeEvent));
            Assert.IsTrue(EdgeExists("Events.ProviderClass", "Events.ProviderClass.ProviderChanged", EdgeType.UnsubscribeEvent));
            Assert.IsTrue(EdgeCountIs(1, EdgeType.UnsubscribeEvent));
        }
    }
}
