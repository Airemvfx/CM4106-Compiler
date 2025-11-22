using Compiler.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Tokenization
{
    /// <summary>
    /// A tokenizer for the MiniSquare-25 language
    /// </summary>
    public class Tokenizer
    {
        /// <summary>
        /// The error reporter
        /// </summary>
        public ErrorReporter Reporter { get; }

        /// <summary>
        /// The reader getting the characters from the file
        /// </summary>
        private IFileReader Reader { get; }

        /// <summary>
        /// The characters currently in the token
        /// </summary>
        private StringBuilder TokenSpelling { get; } = new StringBuilder();

        /// <summary>
        /// Creates a new tokenizer
        /// </summary>
        /// <param name="reader">The reader to get characters from the file</param>
        /// <param name="reporter">The error reporter to use</param>
        public Tokenizer(IFileReader reader, ErrorReporter reporter)
        {
            Reader = reader;
            Reporter = reporter;
        }

        /// <summary>
        /// Gets all the tokens from the file
        /// </summary>
        /// <returns>A list of all the tokens in the file in the order they appear</returns>
        public List<Token> GetAllTokens()
        {
            List<Token> tokens = new List<Token>();
            Token token = GetNextToken();
            while (token.Type != TokenType.EndOfText)
            {
                tokens.Add(token);
                token = GetNextToken();
            }
            tokens.Add(token);
            Reader.Close();
            return tokens;
        }

        /// <summary>
        /// Scan the next token
        /// </summary>
        /// <returns>The next token in the file</returns>
        private Token GetNextToken()
        {
            // Skip forward over any white space and comments
            SkipSeparators();

            // Remember the starting position of the token
            Position tokenStartPosition = Reader.CurrentPosition;

            // Scan the token and work out its type
            TokenType tokenType = ScanToken();

            // Create the token
            Token token = new Token(tokenType, TokenSpelling.ToString(), tokenStartPosition);
            Debugger.Write($"Scanned {token}");

            // Report an error if necessary
            if (tokenType == TokenType.Error)
            {
                // Report the error here
            }

            return token;
        }

        /// <summary>
        /// Skip forward until the next character is not whitespace or a comment
        /// </summary>
        private void SkipSeparators()
        {
            while (Reader.Current == '!' || IsWhiteSpace(Reader.Current))
            {
                if (Reader.Current == '!')
                    Reader.SkipRestOfLine();
                else
                    Reader.MoveNext();
            }
        }

        /// <summary>
        /// Find the next token
        /// </summary>
        /// <returns>The type of the next token</returns>
        /// <remarks>Sets tokenSpelling to be the characters in the token</remarks>
        private TokenType ScanToken()
        {
            TokenSpelling.Clear();
            if (char.IsLetter(Reader.Current) || Reader.Current == '_')
            {
                // Reading an identifier: (letter|_)(letter|_|-)*
                TakeIt();
                while (char.IsLetter(Reader.Current) || Reader.Current == '_' || Reader.Current == '-')
                    TakeIt();
                if (TokenTypes.IsKeyword(TokenSpelling))
                    return TokenTypes.GetTokenForKeyword(TokenSpelling);
                else
                    return TokenType.Identifier;
            }
            else if (char.IsDigit(Reader.Current))
            {
                // Reading an integer literal: digit digit*
                TakeIt();
                while (char.IsDigit(Reader.Current))
                    TakeIt();
                return TokenType.IntLiteral;
            }
            else if (IsOperator(Reader.Current))
            {
                // Read a single-char operator
                TakeIt();
                return TokenType.Operator;
            }
            else if (Reader.Current == '=')
            {
                // Read = or ==
                TakeIt();
                if (Reader.Current == '=')
                {
                    TakeIt();
                    return TokenType.Assign;
                }
                else
                {
                    return TokenType.Operator;  // Single =
                }
            }
            else if (Reader.Current == ';')
            {
                // Read a ;
                TakeIt();
                return TokenType.Semicolon;
            }
            else if (Reader.Current == '(')
            {
                // Read a (
                TakeIt();
                return TokenType.LeftBracket;
            }
            else if (Reader.Current == ')')
            {
                // Read a )
                TakeIt();
                return TokenType.RightBracket;
            }
            else if (Reader.Current == '{')
            {
                // Read a {
                TakeIt();
                return TokenType.LeftBrace;
            }
            else if (Reader.Current == '}')
            {
                // Read a }
                TakeIt();
                return TokenType.RightBrace;
            }
            else if (Reader.Current == '"')
            {
                // Read a char literal: "graphic"
                TakeIt();
                // Take the graphic character (any visible char)
                if (IsGraphic(Reader.Current))
                    TakeIt();
                else
                    return TokenType.Error;  // Invalid graphic
                if (Reader.Current == '"')
                {
                    TakeIt();
                    return TokenType.CharLiteral;
                }
                else
                {
                    // Could do better error handling here but we weren't asked to
                    return TokenType.Error;
                }
            }
            else if (Reader.Current == default(char))
            {
                // Read the end of the file
                TakeIt();
                return TokenType.EndOfText;
            }
            else
            {
                // Encountered a character we weren't expecting
                TakeIt();
                return TokenType.Error;
            }
        }

        /// <summary>
        /// Appends the current character to the current token then moves to the next character
        /// </summary>
        private void TakeIt()
        {
            TokenSpelling.Append(Reader.Current);
            Reader.MoveNext();
        }

        /// <summary>
        /// Checks whether a character is white space
        /// </summary>
        /// <param name="c">The character to check</param>
        /// <returns>True if and only if c is a whitespace character</returns>
        private static bool IsWhiteSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n';
        }

        /// <summary>
        /// Checks whether a character is an operator
        /// </summary>
        /// <param name="c">The character to check</param>
        /// <returns>True if and only if the character is an operator in the language</returns>
        private static bool IsOperator(char c)
        {
            switch (c)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                case '<':
                case '>':
                case '\\':
                case '&':
                case '|':
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks whether a character is a graphic (visible on screen)
        /// </summary>
        /// <param name="c">The character to check</param>
        /// <returns>True if and only if c is a printable graphic character</returns>
        private static bool IsGraphic(char c)
        {
            return char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsSymbol(c) || c == ' ';
        }
    }
}