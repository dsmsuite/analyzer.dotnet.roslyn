I want to analyzer software dependencies for C# at a very detail level and represent it by a hierarchical graph.

Each software element will be represented by a node having the following attributes:
-Unique node-id represented an integer
-Node name represented by a string
-Unique node-type represented by an integer. The integer refers to a node type string.
-Parent-id referring to its parent node.
-File-id
-Begin line number
-End line number

Each software dependency will be represented by a edge having the following attributes:
-Edge consumer node-id
-Edge provider node-id
-Dependency strength
-Unique edge-type represented by an integer. The integer refers to a edge type string.
-File-id
-Begin line number
-End line number

The node types can be:
-File
-Class
-Enum
-Struct
-Function
-Variable

The edge types can be:
-Usage for edges caused by usage statement
-Inherit for edges caused by  class inheritance
-Override for edges caused by methods being overridden in a derived class
-Call for edges caused by function calls
-Use for edges caused by variables being used by functions
-Parameter for edges caused by parameter having a specific type. It will cause an edge between the function and parameter type.

I want use Roslyn to analyze these dependencies based on a visual studio solution file

I want the output to be a SQLite database. T

The database should have the following tables with mentioned columns. For each column a name and a type is given.
-MetadataTable
 -Creation date : string
 -Proggramming language: string

-SourceFileTable
  -Id: int
  -File location: string

-NodeTypeTable
  -Id: int
  -Name: string
  -Description: string

-EdgeTypeTable
  -Id: int
  -Name: string
  -Description: string

-NodeTable
  -Id: int
  -Name: string
  -Parent-id: int
  -Type-id: int
  -File-id: int
  -BeginLine: int
  -EndLine: int
  
-EdgeTable:
  -ConsumerId: int
  -ProviderId: int
  -Type-id: int
  -File-id: int
  -Line: int

Could you generated the source code for a command line utility. When generating code separate functionality by splitting up the code in several classes.



