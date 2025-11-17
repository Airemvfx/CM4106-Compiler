using Compiler.IO;
using Compiler.Tokenization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Console;

namespace Compiler
{
    /// <summary>
    /// Compiler for code in a source file
    /// </summary>
    public class Compiler
    {
        /// <summary>
        /// The error reporter
        /// </summary>
        public ErrorReporter Reporter { get; }

        /// <summary>
        /// The file reader
        /// </summary>
        public IFileReader Reader { get; }

        /// <summary>
        /// The tokenizer
        /// </summary>
        public Tokenizer Tokenizer { get; }

        /// <summary>
        /// Creates a new compiler
        /// </summary>
        /// <param name="inputFile">The file containing the source code</param>
        public Compiler(string inputFile)
        {
            Reporter = new ErrorReporter();
            Reader = new FileReader(inputFile);
            Tokenizer = new Tokenizer(Reader, Reporter);
        }

        /// <summary>
        /// Performs the compilation process
        /// </summary>
        public void Compile()
        {
            // Tokenize
            Write("\nTokenising...\n");
            List<Token> tokens = Tokenizer.GetAllTokens();
            if (Reporter.HasErrors) return;

            // Counts
            int totalScanned = tokens.Count;
            int accepted = tokens.Count(t => t.Type != TokenType.Error);

            // Print Number of Tokens for this .tri program
            WriteLine($"Tokens scanned: {totalScanned}");
            WriteLine($"Tokens accepted: {accepted}");

            // Compare if scanned and accepted tokens are the same value
            if(totalScanned == accepted)
                WriteLine("All tokens accepted.\n\nTokenizer Output:\n");
            else
                WriteLine($"{totalScanned - accepted} tokens were rejected.");

            WriteLine(string.Join("\n", tokens));
            WriteLine("\nTokenizing Done.\n");
        }

        /// <summary>
        /// Writes a message reporting on the success of compilation
        /// </summary>
        private void WriteFinalMessage()
        {
            // Write output to tell the user whether it worked or not here
        }

        /// <summary>
        /// Compiles the code in a file
        /// </summary>
        /// <param name="args">Should be one argument, the input file (*.tri)</param>
        public static void Main(string[] args)
        {
            if (args == null || args.Length != 1 || args[0] == null)
                WriteLine("ERROR: Must call the program with exactly one argument, the input file (*.tri)");
            else if (!File.Exists(args[0]))
                WriteLine($"ERROR: The input file \"{Path.GetFullPath(args[0])}\" does not exist");
            else
            {
                string inputFile = args[0];
                WriteLine("\nCM4106 MiniSquare-25 Compiler by Mateusz Borowicz\n");
                WriteLine("\nInitializing compiler...\n");
                Compiler compiler = new Compiler(inputFile);
                WriteLine("Initialized (Input File = " + inputFile + ")\n");
                compiler.Compile();
                compiler.WriteFinalMessage();
            }
        }
    }
}
