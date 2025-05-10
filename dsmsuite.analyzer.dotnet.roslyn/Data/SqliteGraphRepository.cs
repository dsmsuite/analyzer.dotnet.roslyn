using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace dsmsuite.analyzer.dotnet.roslyn.Data
{
    public class SqliteGraphRepository : IGraphRepository
    {
        private readonly string _connectionString;
        private int _filenameIndex = 0;
        private int _nodeTypeIndex = 0;
        private int _edgeTypeIndex = 0;

        private readonly Dictionary<string, int> _filenameIds = [];
        private readonly Dictionary<NodeType, int> _nodeTypeIds = [];
        private readonly Dictionary<EdgeType, int> _edgeTypeIds = [];

        public SqliteGraphRepository(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
            Batteries.Init();
        }

        public void Save(IHierarchicalGraph hierarchicalGraph)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                Create(connection);

                foreach (INode node in hierarchicalGraph.Nodes)
                {
                    int filenameId = RegisterFilename(node.Filename);
                    int nodeTypeId = RegisterNodeType(node.NodeType);
                    InsertNode(connection, node.Id, node.Name, nodeTypeId, node.Parent?.Id, filenameId, node.Startline, node.Endline, node.CyclomaticComplexity);
                }

                foreach (IEdge edge in hierarchicalGraph.Edges)
                {
                    int filenameId = RegisterFilename(edge.Filename);
                    int edgeTypeId = RegisterEdgeType(edge.EdgeType);
                    InsertEdge(connection, edge.Id, edge.Source.Id, edge.Target.Id, edgeTypeId, 1, filenameId, edge.Line);
                }

                foreach (KeyValuePair<string, int> keyValuePair in _filenameIds)
                {
                    InsertFilename(connection, keyValuePair.Value, keyValuePair.Key);
                }

                foreach (KeyValuePair<NodeType, int> keyValuePair in _nodeTypeIds)
                {
                    InsertNodeType(connection, keyValuePair.Value, keyValuePair.Key.ToString());
                }

                foreach (KeyValuePair<EdgeType, int> keyValuePair in _edgeTypeIds)
                {
                    InsertEdgeType(connection, keyValuePair.Value, keyValuePair.Key.ToString());
                }
            }
        }

        private void Create(SqliteConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = SqlCommands.CreateDatabase;
                command.ExecuteNonQuery();
            }
        }

        private void InsertNode(SqliteConnection connection, int id, string name, int nodeTypeId, int? parentId, int filenameId, int begin, int end, int cyclomaticComplexity)
        {
            using var transaction = connection.BeginTransaction();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = SqlCommands.InsertNode;

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@node_type_id", nodeTypeId);
                command.Parameters.AddWithValue("@parent_id", (object?)parentId ?? DBNull.Value);
                command.Parameters.AddWithValue("@source_file_id", filenameId);
                command.Parameters.AddWithValue("@begin_line_no", begin);
                command.Parameters.AddWithValue("@end_line_no", end);
                command.Parameters.AddWithValue("@complexity", cyclomaticComplexity);
                command.Parameters.AddWithValue("@documentation", DBNull.Value);
                command.Parameters.AddWithValue("@annotation", DBNull.Value);
                command.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        private void InsertEdge(SqliteConnection connection, int id, int sourceId, int targetId, int edgeTypeId, int strength, int filenameId, int line)
        {
            using var transaction = connection.BeginTransaction();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = SqlCommands.InsertEdge;

                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("source_id", sourceId);
                command.Parameters.AddWithValue("target_id", targetId);
                command.Parameters.AddWithValue("edge_type_id", edgeTypeId);
                command.Parameters.AddWithValue("strength", strength);
                command.Parameters.AddWithValue("source_file_id", filenameId);
                command.Parameters.AddWithValue("line_no", line);
                command.Parameters.AddWithValue("annotation", DBNull.Value);
                command.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        private void InsertFilename(SqliteConnection connection, int id, string filename)
        {
            using var transaction = connection.BeginTransaction();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = SqlCommands.InsertFilename;

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@filename", filename);

                command.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        private void InsertNodeType(SqliteConnection connection, int id, string name)
        {
            using var transaction = connection.BeginTransaction();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = SqlCommands.InsertNodeType;

                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("name", name);

                command.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        private void InsertEdgeType(SqliteConnection connection, int id, string name)
        {
            using var transaction = connection.BeginTransaction();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = SqlCommands.InsertEdgeType;

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);

                command.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        private int RegisterFilename(string filename)
        {
            if (_filenameIds.ContainsKey(filename))
            {
                return _filenameIds[filename];
            }
            else
            {
                _filenameIndex++;
                _filenameIds[filename] = _filenameIndex;
                return _filenameIndex;
            }
        }

        private int RegisterNodeType(NodeType nodeType)
        {
            if (_nodeTypeIds.ContainsKey(nodeType))
            {
                return _nodeTypeIds[nodeType];
            }
            else
            {
                _nodeTypeIndex++;
                _nodeTypeIds[nodeType] = _nodeTypeIndex;
                return _nodeTypeIndex;
            }
        }

        private int RegisterEdgeType(EdgeType edgeType)
        {
            if (_edgeTypeIds.ContainsKey(edgeType))
            {
                return _edgeTypeIds[edgeType];
            }
            else
            {
                _edgeTypeIndex++;
                _edgeTypeIds[edgeType] = _edgeTypeIndex;
                return _edgeTypeIndex;
            }
        }
    }
}
