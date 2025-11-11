using MiniSquare25Compiler.Lexer;
using MiniSquare25Compiler.Parser;
using MiniSquare25Compiler.SemanticAnalysis;
using MiniSquare25Compiler.CodeGeneration;

namespace MiniSquare25Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("MiniSquare25 Compiler");
                Console.WriteLine("Usage: MiniSquare25Compiler <source-file> [output-file]");
                Console.WriteLine();
                Console.WriteLine("Compiles MiniSquare25 source code to Triangle Abstract Machine code.");
                return;
            }

            string sourceFile = args[0];
            string outputFile = args.Length > 1 ? args[1] : Path.ChangeExtension(sourceFile, ".tam");

            try
            {
                // Read source file
                if (!File.Exists(sourceFile))
                {
                    Console.WriteLine($"Error: Source file '{sourceFile}' not found.");
                    return;
                }

                string sourceCode = File.ReadAllText(sourceFile);
                Console.WriteLine($"Compiling {sourceFile}...");

                // Lexical Analysis
                Console.WriteLine("Phase 1: Lexical Analysis...");
                var scanner = new Scanner(sourceCode);
                var tokens = scanner.ScanTokens();
                Console.WriteLine($"  Generated {tokens.Count} tokens");

                // Syntax Analysis
                Console.WriteLine("Phase 2: Syntax Analysis...");
                var parser = new Parser.Parser(tokens);
                var ast = parser.Parse();
                Console.WriteLine($"  Parsed program '{ast.Name}' with {ast.Declarations.Count} declarations");

                // Semantic Analysis
                Console.WriteLine("Phase 3: Semantic Analysis...");
                var semanticAnalyzer = new SemanticAnalyzer();
                var errors = semanticAnalyzer.Analyze(ast);

                if (errors.Count > 0)
                {
                    Console.WriteLine("Semantic errors found:");
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"  Error: {error}");
                    }
                    return;
                }
                Console.WriteLine("  No semantic errors");

                // Code Generation
                Console.WriteLine("Phase 4: Code Generation...");
                var codeGenerator = new TAMCodeGenerator();
                string tamCode = codeGenerator.Generate(ast);
                
                // Write output
                File.WriteAllText(outputFile, tamCode);
                Console.WriteLine($"  Generated Triangle Abstract Machine code");
                Console.WriteLine();
                Console.WriteLine($"Compilation successful!");
                Console.WriteLine($"Output written to: {outputFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Compilation failed: {ex.Message}");
                if (ex.StackTrace != null)
                {
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                }
            }
        }
    }
}
