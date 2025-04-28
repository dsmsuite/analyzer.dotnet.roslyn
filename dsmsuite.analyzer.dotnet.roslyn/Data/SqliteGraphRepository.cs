using Microsoft.Data.Sqlite;

namespace dsmsuite.analyzer.dotnet.roslyn.Data
{
    public class SqliteGraphRepository : IGraphRepository
    {
        private readonly string _connectionString;

        public SqliteGraphRepository(string dbPath)
        {
            // _connectionString = "Data Source=MyDatabase.sqlite;Version=3;";
            _connectionString = $"Data Source={dbPath}";
        }

        public void Create()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                // Create a table
                string sql = "CREATE TABLE IF NOT EXISTS highscores (name VARCHAR(20), score INT)";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public void SaveNodeType(int id, string name)
        {
        }
        public void SaveEdgeType(int id, string name)
        {
        }

        public void SaveSourceFilename(int id, string filename)
        {

        }
        public void SaveNode(int id, string name, int nodeTypeId, int? parentId, int filenameId, int begin, int end, int loc, int? cyclomaticComplexity)
        {
            //            id INTEGER PRIMARY KEY,                                    --Unique ID
            //parent_id INTEGER,                                         --The optional id of the parent node
            //    name TEXT NOT NULL,                                        --The namme of the node
            //    node_type_id INTEGER NOT NULL,                             --The node type
            //    source_file_id INTEGER NOT NULL,                           --The source file where the node was found
            //    begin_line_no INTEGER NOT NULL,                            --The line number where the node source code begins
            //    end_line_no INTEGER NOT NULL,                              --The line number where the node source code ends
            //    lines_of_code_metric INTEGER,                              --The optional lines of code metric
            //    complexity INTEGER,                                        --The optional cylomatic complexity metric for functions
            //    documentation TEXT, --The optional documentation as found in the source code
            //    annotation TEXT, --An optional annotation based on human or ai evaluation

            //using var connection = new SqliteConnection(_connectionString);
            //connection.Open();

            //using var transaction = connection.BeginTransaction();

            //foreach (var node in nodes)
            //{
            //    var command = connection.CreateCommand();
            //    command.CommandText = @"
            //    INSERT INTO NodeDetails (name, node_type_id, location_id)
            //    VALUES ($name, (SELECT id FROM NodeType WHERE text = $type), 1);

            //    INSERT INTO Node (parent_id, loc, cc, detail_id, created)
            //    VALUES ($parent, $loc, $cc, last_insert_rowid(), 1);";

            //    command.Parameters.AddWithValue("$name", node.Name);
            //    command.Parameters.AddWithValue("$type", node.Type);
            //    command.Parameters.AddWithValue("$parent", node.ParentId ?? (object)DBNull.Value);
            //    command.Parameters.AddWithValue("$loc", node.LinesOfCode);
            //    command.Parameters.AddWithValue("$cc", node.CyclomaticComplexity ?? (object)DBNull.Value);
            //    command.ExecuteNonQuery();
            //}

            //transaction.Commit();
        }

        public void SaveEdge(int id, int sourceId, int targetId, int edgeTYpe, int strength)
        {
            //using var connection = new SqliteConnection(_connectionString);
            //connection.Open();

            //using var transaction = connection.BeginTransaction();

            //foreach (var edge in edges)
            //{
            //    var command = connection.CreateCommand();
            //    command.CommandText = @"
            //    INSERT INTO EdgeDetails (edge_type_id, location_id)
            //    VALUES ((SELECT id FROM EdgeType WHERE text = $type), 1);

            //    INSERT INTO Edge (source_id, target_id, strength, detail_id, created)
            //    VALUES ($src, $tgt, $strength, last_insert_rowid(), 1);";

            //    command.Parameters.AddWithValue("$src", edge.SourceId);
            //    command.Parameters.AddWithValue("$tgt", edge.TargetId);
            //    command.Parameters.AddWithValue("$strength", edge.Strength);
            //    command.Parameters.AddWithValue("$type", edge.Type);
            //    command.ExecuteNonQuery();
            //}

            //transaction.Commit();
        }
    }
}
