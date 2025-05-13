namespace dsmsuite.analyzer.dotnet.roslyn.test.Documentation
{
    // Class with leading comments
    public class DocumentatedClass
    {
        // Method with leading comments
        public void MethodWitLeadingComments() 
        {
            // Variable with leading comments 
            int variableWitLeadingComments = 0;

            int variableWithTrainingComments = 1; // Variable with trailing comments

            if (variableWitLeadingComments == variableWithTrainingComments) // Statement with trailng comments
            {

            }
        }

        // Property with leading comments 
        public string? PropertWitLeadingComments { get; set; }
           
        public string? PropertWitTrailingComments { get; set; } // Property with trailing comments

        // Field with leading comments 
        private int _fieldWithLeadingComments;

        private int _fieldWithTrailingComments; // Field with trailing comments

        // Event with leading comments 
        public event EventHandler? EventWithLeadingComments;

        public event EventHandler? EventWithTrailingComments; //Event with trailing comment
    }
}
