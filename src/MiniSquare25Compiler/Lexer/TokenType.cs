namespace MiniSquare25Compiler.Lexer
{
    public enum TokenType
    {
        // Keywords
        Program,
        Begin,
        End,
        Var,
        Const,
        If,
        Then,
        Else,
        While,
        Do,
        Read,
        Write,
        Integer,
        Boolean,
        
        // Operators
        Plus,           // +
        Minus,          // -
        Multiply,       // *
        Divide,         // /
        Equals,         // =
        NotEquals,      // !=
        LessThan,       // <
        GreaterThan,    // >
        LessOrEqual,    // <=
        GreaterOrEqual, // >=
        And,            // &&
        Or,             // ||
        Not,            // !
        
        // Assignment
        Assign,         // :=
        
        // Delimiters
        Semicolon,      // ;
        Colon,          // :
        Comma,          // ,
        LeftParen,      // (
        RightParen,     // )
        
        // Literals
        IntLiteral,
        BoolLiteral,
        Identifier,
        
        // Special
        EndOfFile,
        Unknown
    }
}
