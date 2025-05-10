namespace dsmsuite.analyzer.dotnet.roslyn.Data
{
    internal class SqlCommands
    {
        public const string CreateDatabase = @"
-- Table: Run
CREATE TABLE AnalysisRunInfo(
    id INTEGER PRIMARY KEY,            -- Unique ID
    timestamp DATETIME NOT NULL,       -- Timestamp when analysis was performed
    description TEXT,                  -- A description of the purpose of the analysis run
    author TEXT                        -- Optionally the name of the person who ran the analysis
    source_code_language TEXT          -- Programming language used in source code
 );

-- Table: Node
CREATE TABLE Node(
    id INTEGER PRIMARY KEY,                                    -- Unique ID
    parent_id INTEGER,                                         -- The optional id of the parent node
    name TEXT NOT NULL,                                        -- The namme of the node
    node_type_id INTEGER NOT NULL,                             -- The node type
    source_file_id INTEGER,                                    -- The source file where the node was found if source file information available
    begin_line_no INTEGER,                                     -- The line number where the node source code begins if source file information available
    end_line_no INTEGER,                                       -- The line number where the node source code ends if source file information available
    complexity INTEGER,                                        -- The cylomatic complexity metric for functions if it has been measured
    documentation TEXT,                                        -- An optional documentation as found in the source code
    annotation TEXT,                                           -- An optional annotation based on human or ai evaluation

    -- FOREIGN KEY(parent_id) REFERENCES Node(id),             -- Enforcing this constrant would mean parents need to be processed before children
    FOREIGN KEY(node_type_id) REFERENCES NodeType(id),
    FOREIGN KEY(source_file_id) REFERENCES SourceFile(id)
);

-- Table: Edge
CREATE TABLE Edge(
    id INTEGER PRIMARY KEY,                                    -- Unique ID
    source_id INTEGER NOT NULL,                                -- The source node of the edge
    target_id INTEGER NOT NULL,                                -- The target node of the edge
    edge_type_id INTEGER NOT NULL,                             -- The edge type
    strength INTEGER NOT NULL,                                 -- The edge strength
    source_file_id INTEGER,                                    -- The source file where the edge was found if source file information available
    line_no INTEGER,                                           -- The line number where the edge was found if source file information available
    annotation TEXT,                                           -- An optional annotation based on human or ai evaluation
    
    FOREIGN KEY(source_id) REFERENCES Node(id),
    FOREIGN KEY(target_id) REFERENCES Node(id),
    FOREIGN KEY(edge_type_id) REFERENCES EdgeType(id),
    FOREIGN KEY(source_file_id) REFERENCES SourceFile(id)
);

-- Table: NodeType
CREATE TABLE NodeType(
    id INTEGER PRIMARY KEY,                                    -- Unique ID
    name TEXT NOT NULL UNIQUE                                  -- Name of tye node type
);

-- Table: EdgeType
CREATE TABLE EdgeType(
    id INTEGER PRIMARY KEY,                                    -- Unique ID
    name TEXT NOT NULL UNIQUE                                  -- Name of the edge type
);

-- Table: SourceFile
CREATE TABLE SourceFile(
    id INTEGER PRIMARY KEY,                                    -- Unique ID
    filename TEXT NOT NULL UNIQUE                              -- The full path of the source file;
);";

        public const string InsertNode = @"INSERT INTO Node (id, name, node_type_id, parent_id, source_file_id, begin_line_no, end_line_no, complexity, documentation, annotation)
                                                VALUES (@id,  @name, @node_type_id, @parent_id, @source_file_id, @begin_line_no, @end_line_no, @complexity, @documentation, @annotation);";

        public const string InsertEdge = @"INSERT INTO Edge (id, source_id, target_id, edge_type_id, strength, source_file_id, line_no, annotation)
                                                VALUES (@id, @source_id, @target_id, @edge_type_id, @strength, @source_file_id, @line_no, @annotation);";

        public const string InsertFilename = @"INSERT INTO SourceFile (id, filename)
                                                VALUES (@id, @filename);";

        public const string InsertNodeType = @"INSERT INTO NodeType (id, name)
                                                VALUES (@id, @name);";

        public const string InsertEdgeType = @"INSERT INTO EdgeType (id, name)
                                                VALUES (@id, @name);";
    }
}
