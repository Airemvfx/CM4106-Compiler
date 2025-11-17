using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Compiler.Tokenization
{
    /// <summary>
    /// The types of token in the language
    /// </summary>
    public enum TokenType
    {
        // non-terminals
        IntLiteral, Identifier, Operator, CharLiteral,

        // reserved words - terminals
        Do, Else, If, In, Let, Then, While,
        Local, Pass, Repeat, Unless, Until, Var,  // New for MiniSquare-25

        // punctuation - terminals
        Semicolon, Assign, LeftBracket, RightBracket, LeftBrace, RightBrace,

        // special tokens
        EndOfText, Error
    }

    /// <summary>
    /// Utility functions for working with the tokens
    /// </summary>
    public static class TokenTypes
    {
        /// <summary>
        /// A mapping from keyword to the token type for that keyword
        /// </summary>
        public static ImmutableDictionary<string, TokenType> Keywords { get; } = new Dictionary<string, TokenType>()
        {
            { "do", TokenType.Do },
            { "else", TokenType.Else },
            { "if", TokenType.If },
            { "in", TokenType.In },
            { "let", TokenType.Let },
            { "then", TokenType.Then },
            { "while", TokenType.While },
            // New for MiniSquare-25
            { "local", TokenType.Local },
            { "pass", TokenType.Pass },
            { "repeat", TokenType.Repeat },
            { "unless", TokenType.Unless },
            { "until", TokenType.Until },
            { "var", TokenType.Var }
        }.ToImmutableDictionary();

        /// <summary>
        /// Checks whether a word is a keyword
        /// </summary>
        /// <param name="word">The word to check</param>
        /// <returns>True if and only if the word is a keyword</returns>
        public static bool IsKeyword(StringBuilder word)
        {
            return Keywords.ContainsKey(word.ToString());
        }

        /// <summary>
        /// Gets the token for a keyword
        /// </summary>
        /// <param name="word">The keyword to get the token for</param>
        /// <returns>The token associated with the given keyword</returns>
        /// <remarks>If the word is not a keyword then an exception is thrown</remarks>
        public static TokenType GetTokenForKeyword(StringBuilder word)
        {
            if (!IsKeyword(word)) throw new ArgumentException("Word is not a keyword");
            return Keywords[word.ToString()];
        }
    }
}