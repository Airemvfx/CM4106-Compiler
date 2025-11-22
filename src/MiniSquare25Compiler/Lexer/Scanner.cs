using System.Text;

namespace MiniSquare25Compiler.Lexer
{
    public class Scanner
    {
        private readonly string _source;
        private readonly List<Token> _tokens = new();
        private int _start = 0;
        private int _current = 0;
        private int _line = 1;
        private int _column = 1;

        private static readonly Dictionary<string, TokenType> _keywords = new()
        {
            { "program", TokenType.Program },
            { "begin", TokenType.Begin },
            { "end", TokenType.End },
            { "var", TokenType.Var },
            { "const", TokenType.Const },
            { "if", TokenType.If },
            { "then", TokenType.Then },
            { "else", TokenType.Else },
            { "while", TokenType.While },
            { "do", TokenType.Do },
            { "read", TokenType.Read },
            { "write", TokenType.Write },
            { "integer", TokenType.Integer },
            { "boolean", TokenType.Boolean },
            { "true", TokenType.BoolLiteral },
            { "false", TokenType.BoolLiteral }
        };

        public Scanner(string source)
        {
            _source = source;
        }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                _start = _current;
                ScanToken();
            }

            _tokens.Add(new Token(TokenType.EndOfFile, "", _line, _column));
            return _tokens;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(TokenType.LeftParen); break;
                case ')': AddToken(TokenType.RightParen); break;
                case ',': AddToken(TokenType.Comma); break;
                case ';': AddToken(TokenType.Semicolon); break;
                case '+': AddToken(TokenType.Plus); break;
                case '-': AddToken(TokenType.Minus); break;
                case '*': AddToken(TokenType.Multiply); break;
                case '/': 
                    if (Match('/'))
                    {
                        // Single-line comment
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else
                    {
                        AddToken(TokenType.Divide);
                    }
                    break;
                case ':':
                    if (Match('='))
                        AddToken(TokenType.Assign);
                    else
                        AddToken(TokenType.Colon);
                    break;
                case '=':
                    AddToken(TokenType.Equals);
                    break;
                case '!':
                    if (Match('='))
                        AddToken(TokenType.NotEquals);
                    else
                        AddToken(TokenType.Not);
                    break;
                case '<':
                    if (Match('='))
                        AddToken(TokenType.LessOrEqual);
                    else
                        AddToken(TokenType.LessThan);
                    break;
                case '>':
                    if (Match('='))
                        AddToken(TokenType.GreaterOrEqual);
                    else
                        AddToken(TokenType.GreaterThan);
                    break;
                case '&':
                    if (Match('&'))
                        AddToken(TokenType.And);
                    break;
                case '|':
                    if (Match('|'))
                        AddToken(TokenType.Or);
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace
                    break;
                case '\n':
                    _line++;
                    _column = 1;
                    break;
                default:
                    if (IsDigit(c))
                    {
                        Number();
                    }
                    else if (IsAlpha(c))
                    {
                        Identifier();
                    }
                    else
                    {
                        AddToken(TokenType.Unknown);
                    }
                    break;
            }
        }

        private void Number()
        {
            while (IsDigit(Peek())) Advance();

            string text = _source.Substring(_start, _current - _start);
            AddToken(TokenType.IntLiteral, int.Parse(text));
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek())) Advance();

            string text = _source.Substring(_start, _current - _start);
            TokenType type = _keywords.GetValueOrDefault(text.ToLower(), TokenType.Identifier);
            
            object? value = null;
            if (type == TokenType.BoolLiteral)
            {
                value = text.ToLower() == "true";
            }
            
            AddToken(type, value);
        }

        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (_source[_current] != expected) return false;

            _current++;
            _column++;
            return true;
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[_current];
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   c == '_';
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private bool IsAtEnd()
        {
            return _current >= _source.Length;
        }

        private char Advance()
        {
            _column++;
            return _source[_current++];
        }

        private void AddToken(TokenType type, object? value = null)
        {
            string text = _source.Substring(_start, _current - _start);
            _tokens.Add(new Token(type, text, _line, _column - text.Length, value));
        }
    }
}
