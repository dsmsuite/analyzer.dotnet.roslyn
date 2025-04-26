using dsmsuite.analyzer.dotnet.roslyn.Graph;
using Microsoft.Data.Sqlite;

namespace dsmsuite.analyzer.dotnet.roslyn.Data
{
    public class SqliteGraphRepository : IGraphRepository
    {
        private readonly string _connectionString;

        public SqliteGraphRepository(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
        }

        public void SaveNodes(IEnumerable<GraphNode> nodes)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            foreach (var node in nodes)
            {
                var command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO NodeDetails (name, node_type_id, location_id)
                VALUES ($name, (SELECT id FROM NodeType WHERE text = $type), 1);

                INSERT INTO Node (parent_id, loc, cc, detail_id, created)
                VALUES ($parent, $loc, $cc, last_insert_rowid(), 1);";

                command.Parameters.AddWithValue("$name", node.Name);
                command.Parameters.AddWithValue("$type", node.Type);
                command.Parameters.AddWithValue("$parent", node.ParentId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("$loc", node.LinesOfCode);
                command.Parameters.AddWithValue("$cc", node.CyclomaticComplexity ?? (object)DBNull.Value);
                command.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        public void SaveEdges(IEnumerable<GraphEdge> edges)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            foreach (var edge in edges)
            {
                var command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO EdgeDetails (edge_type_id, location_id)
                VALUES ((SELECT id FROM EdgeType WHERE text = $type), 1);

                INSERT INTO Edge (source_id, target_id, strength, detail_id, created)
                VALUES ($src, $tgt, $strength, last_insert_rowid(), 1);";

                command.Parameters.AddWithValue("$src", edge.SourceId);
                command.Parameters.AddWithValue("$tgt", edge.TargetId);
                command.Parameters.AddWithValue("$strength", edge.Strength);
                command.Parameters.AddWithValue("$type", edge.Type);
                command.ExecuteNonQuery();
            }

            transaction.Commit();
        }
    }
}
