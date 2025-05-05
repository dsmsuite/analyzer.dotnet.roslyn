using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace dsmsuite.analyzer.dotnet.roslyn.Data
{
    public class SqliteGraphRepository : IGraphRepository
    {
        private readonly string _connectionString;

        public SqliteGraphRepository(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
            Batteries.Init();
        }

        public void Create()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = SqlCommands.CreateDatabase;
                    command.ExecuteNonQuery();
                }
            }
        }
        public void SaveNodeType(int id, string name)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using var transaction = connection.BeginTransaction();

                // Create a table
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO NodeType (id, name)
                                            VALUES (@id, @name);";

                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("name", name);

                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
        }

        public void SaveEdgeType(int id, string name)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using var transaction = connection.BeginTransaction();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO EdgeType (id, name)
                                            VALUES (@id, @name);";

                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@name", name);

                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
        }

        public void SaveSourceFilename(int id, string filename)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using var transaction = connection.BeginTransaction();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO SourceFile (id, filename)
                                            VALUES (@id, @filename);";

                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@filename", filename);

                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
        }
        public void SaveNode(int id, string name, int nodeTypeId, int? parentId, int filenameId, int begin, int end, int cyclomaticComplexity)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using var transaction = connection.BeginTransaction();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO Node (id, name, node_type_id, parent_id, source_file_id, begin_line_no, end_line_no, complexity, documentation, annotation)
                                            VALUES (@id,  @name, @node_type_id, @parent_id, @source_file_id, @begin_line_no, @end_line_no, @complexity, @documentation, @annotation);";

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
        }

        public void SaveEdge(int id, int sourceId, int targetId, int edgeTypeId, int strength)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO Edge (id, source_id, target_id, edge_type_id, strength, source_file_id, line_no, annotation)
                                        VALUES (@id, @source_id, @target_id, @edge_type_id, @strength, @source_file_id, @line_no, @annotation);";

                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("source_id", sourceId);
                command.Parameters.AddWithValue("target_id", targetId);
                command.Parameters.AddWithValue("edge_type_id", edgeTypeId);
                command.Parameters.AddWithValue("strength", strength);
                command.Parameters.AddWithValue("source_file_id", DBNull.Value);
                command.Parameters.AddWithValue("line_no", DBNull.Value);
                command.Parameters.AddWithValue("annotation", DBNull.Value);
                command.ExecuteNonQuery();
            }

            transaction.Commit();
        }
    }
}
