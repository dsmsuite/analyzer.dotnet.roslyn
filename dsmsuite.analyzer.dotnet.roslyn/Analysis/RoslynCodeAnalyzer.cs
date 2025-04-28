using dsmsuite.analyzer.dotnet.roslyn.Data;
using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.Build.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System.Drawing;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace dsmsuite.analyzer.dotnet.roslyn.Analysis
{
    public class RoslynCodeAnalyzer : ICodeAnalyzer, IDependencyVisitorCallback
    {
        private int _filenameIndex = 0;
        private int _nodeIndex = 0;
        private int _edgeIndex = 0;
        private int _nodeTypeIndex = 0;
        private int _edgeTypeIndex = 0;
        private readonly Dictionary<string, int> _filenameIds = [];
        private readonly Dictionary<ISymbol, Node> _nodes = [];
        private readonly List<Edge> _edges = [];
        private readonly Dictionary<NodeType, int> _nodeTypeIds = [];
        private readonly Dictionary<EdgeType, int> _edgeTypeIds = [];

        private class Node
        {
            private int _id;
            private ISymbol _symbol;
            private SyntaxNode _syntaxNode;
            private NodeType _nodeType;
            private int? _cyclomaticComplexity;

            public Node(int id, ISymbol symbol, SyntaxNode syntaxNode, NodeType nodeType, int? cyclomaticComplexity)
            {
                _id = id;
                _symbol = symbol;
                _syntaxNode = syntaxNode;
                _nodeType = nodeType;
                _cyclomaticComplexity = cyclomaticComplexity;
            }

            public int Id => _id;
            public string Name => _symbol.Name;
            public string Fullname => _symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            public ISymbol? Parent => _symbol.ContainingSymbol;
            public NodeType NodeType => _nodeType;
            public string Filename => _syntaxNode.SyntaxTree?.FilePath ?? "";
            public int Startline => _syntaxNode.GetLocation().GetLineSpan().StartLinePosition.Line;
            public int Endline => _syntaxNode.GetLocation().GetLineSpan().EndLinePosition.Line;
            public int LinesOfCode => Endline - Startline + 1;
            public int? CyclomaticComplexity { get; }
        }

        private class Edge
        {
            private int _id;
            private Node _source;
            private Node _target;
            private EdgeType _edgeType;

            public Edge(int id, Node source, Node target, EdgeType edgeType)
            {
                _id = id;
                _source = source;
                _target = target;
                _edgeType = edgeType;
            }

            public int Id => _id;
            public Node Source => _source;
            public Node Target => _target;
            public EdgeType EdgeType => _edgeType;
        }

        private void RegisterFilename(string filename)
        {
            if (!_filenameIds.ContainsKey(filename))
            {
                _filenameIndex++;
                _filenameIds[filename] = _filenameIndex;
            }
        }

        private void RegsiterNodeType(NodeType nodeType)
        {
            if (!_nodeTypeIds.ContainsKey(nodeType))
            {
                _nodeTypeIndex++;
                _nodeTypeIds[nodeType] = _nodeTypeIndex;
            }
        }

        private void RegsiterEdgeType(EdgeType edgeType)
        {
            if (!_edgeTypeIds.ContainsKey(edgeType))
            {
                _edgeTypeIndex++;
                _edgeTypeIds[edgeType] = _edgeTypeIndex;
            }
        }

        public int? RegisterNode(ISymbol symbol, NodeType nodeType, SyntaxNode syntaxNode, int? cyclomaticComplexity)
        {
            _nodeIndex++;
            Node node = new Node(_nodeIndex, symbol, syntaxNode, nodeType, cyclomaticComplexity);
            _nodes[symbol] = node;

            RegsiterNodeType(nodeType);
            RegisterFilename(node.Filename);

            return _nodeIndex;
        }

        public int? RegisterEdge(ISymbol source, ISymbol target, EdgeType edgeType)
        {
            _edgeIndex++;
            if (_nodes.ContainsKey(source) && _nodes.ContainsKey(target))
            {
                Node sourceNode = _nodes[source];
                Node targetNode = _nodes[target];
                _edges.Add(new Edge(_edgeIndex, sourceNode, targetNode, edgeType));

                RegsiterEdgeType(edgeType);
            }

            return null;
        }

        public void Save(IGraphRepository graphRepository)
        {
            foreach (KeyValuePair<NodeType, int> keyValuePair in _nodeTypeIds)
            {
                graphRepository.SaveNodeType(keyValuePair.Value, keyValuePair.Key.ToString());
            }

            foreach (KeyValuePair<EdgeType, int> keyValuePair in _edgeTypeIds)
            {
                graphRepository.SaveEdgeType(keyValuePair.Value, keyValuePair.Key.ToString());
            }

            foreach (KeyValuePair<string, int> keyValuePair in _filenameIds)
            {
                graphRepository.SaveSourceFilename(keyValuePair.Value, keyValuePair.Key);
            }
            foreach (Node node in _nodes.Values)
            {
                int? filenameId = _filenameIds[node.Filename];
                int? nodeTypeId = _nodeTypeIds[node.NodeType];
                int? parentId = null;
                if (node.Parent != null)
                {
                    if (_nodes.ContainsKey(node.Parent))
                    {
                        parentId = _nodes[node.Parent].Id;
                    }
                }
                if (filenameId != null && nodeTypeId != null)
                {
                    graphRepository.SaveNode(node.Id, node.Fullname, nodeTypeId.Value, parentId, filenameId.Value, node.Startline, node.Endline, node.LinesOfCode, node.CyclomaticComplexity);
                }
            }

            foreach (Edge edge in _edges)
            {
                int? edgeTypeId = _edgeTypeIds[edge.EdgeType];
                if (edgeTypeId != null)
                {
                    graphRepository.SaveEdge(edge.Id, edge.Source.Id, edge.Target.Id, edgeTypeId.Value, 1);
                }
            }
        }


        public async Task AnalyzeAsync(string solutionPath, IGraphBuilder graphBuilder)
        {
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution currSolution = workspace.OpenSolutionAsync(solutionPath).Result;

            foreach (Project project in currSolution.Projects)
            {
                if (project.FilePath != null)
                {
                    Console.WriteLine($"Processing project {project.FilePath}");

                    foreach (var document in project.Documents)
                    {
                        if (document.FilePath != null)
                        {
                            var syntaxTree = await document.GetSyntaxTreeAsync();
                            var semanticModel = await document.GetSemanticModelAsync();

                            if (syntaxTree == null || semanticModel == null) continue;

                            var root = await syntaxTree.GetRootAsync();
                            var visitor = new DependencyVisitor(semanticModel, this, document.FilePath ?? "UnknownFile.cs");
                            visitor.Visit(root);
                        }
                    }
                }
            }
        }
    }
}
;