namespace MiniSquare25Compiler.Lexer
{
    public class Token
    {
        public TokenType Type { get; }
        public string Lexeme { get; }
        public int Line { get; }
        public int Column { get; }
        public object? Value { get; }

        public Token(TokenType type, string lexeme, int line, int column, object? value = null)
        {
            Type = type;
            Lexeme = lexeme;
            Line = line;
            Column = column;
            Value = value;
        }

        public override string ToString()
        {
            return $"Token({Type}, '{Lexeme}', Line {Line}, Col {Column})";
        }
    }
}
