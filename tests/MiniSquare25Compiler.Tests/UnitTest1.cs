using MiniSquare25Compiler.Lexer;
using MiniSquare25Compiler.SemanticAnalysis;
using MiniSquare25Compiler.CodeGeneration;

namespace MiniSquare25Compiler.Tests;

public class LexerTests
{
    [Fact]
    public void Scanner_ShouldTokenizeSimpleProgram()
    {
        string source = "program Test; begin end;";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        
        Assert.Equal(7, tokens.Count); // program, Test, ;, begin, end, ;, EOF
        Assert.Equal(TokenType.Program, tokens[0].Type);
        Assert.Equal(TokenType.Identifier, tokens[1].Type);
        Assert.Equal(TokenType.Semicolon, tokens[2].Type);
        Assert.Equal(TokenType.Begin, tokens[3].Type);
        Assert.Equal(TokenType.End, tokens[4].Type);
        Assert.Equal(TokenType.Semicolon, tokens[5].Type);
        Assert.Equal(TokenType.EndOfFile, tokens[6].Type);
    }

    [Fact]
    public void Scanner_ShouldRecognizeKeywords()
    {
        string source = "var integer boolean if then else while do read write";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        
        Assert.Equal(TokenType.Var, tokens[0].Type);
        Assert.Equal(TokenType.Integer, tokens[1].Type);
        Assert.Equal(TokenType.Boolean, tokens[2].Type);
        Assert.Equal(TokenType.If, tokens[3].Type);
        Assert.Equal(TokenType.Then, tokens[4].Type);
        Assert.Equal(TokenType.Else, tokens[5].Type);
        Assert.Equal(TokenType.While, tokens[6].Type);
        Assert.Equal(TokenType.Do, tokens[7].Type);
        Assert.Equal(TokenType.Read, tokens[8].Type);
        Assert.Equal(TokenType.Write, tokens[9].Type);
    }

    [Fact]
    public void Scanner_ShouldRecognizeOperators()
    {
        string source = "+ - * / := = != < > <= >=";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        
        Assert.Equal(TokenType.Plus, tokens[0].Type);
        Assert.Equal(TokenType.Minus, tokens[1].Type);
        Assert.Equal(TokenType.Multiply, tokens[2].Type);
        Assert.Equal(TokenType.Divide, tokens[3].Type);
        Assert.Equal(TokenType.Assign, tokens[4].Type);
        Assert.Equal(TokenType.Equals, tokens[5].Type);
        Assert.Equal(TokenType.NotEquals, tokens[6].Type);
        Assert.Equal(TokenType.LessThan, tokens[7].Type);
        Assert.Equal(TokenType.GreaterThan, tokens[8].Type);
        Assert.Equal(TokenType.LessOrEqual, tokens[9].Type);
        Assert.Equal(TokenType.GreaterOrEqual, tokens[10].Type);
    }

    [Fact]
    public void Scanner_ShouldRecognizeLiterals()
    {
        string source = "42 true false identifier123";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        
        Assert.Equal(TokenType.IntLiteral, tokens[0].Type);
        Assert.Equal(42, tokens[0].Value);
        Assert.Equal(TokenType.BoolLiteral, tokens[1].Type);
        Assert.Equal(true, tokens[1].Value);
        Assert.Equal(TokenType.BoolLiteral, tokens[2].Type);
        Assert.Equal(false, tokens[2].Value);
        Assert.Equal(TokenType.Identifier, tokens[3].Type);
    }
}

public class ParserTests
{
    [Fact]
    public void Parser_ShouldParseSimpleProgram()
    {
        string source = "program Test; begin end;";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        var parser = new MiniSquare25Compiler.Parser.Parser(tokens);
        var ast = parser.Parse();
        
        Assert.Equal("Test", ast.Name);
        Assert.Empty(ast.Declarations);
        Assert.NotNull(ast.Body);
    }

    [Fact]
    public void Parser_ShouldParseVariableDeclaration()
    {
        string source = "program Test; var x: integer; begin end;";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        var parser = new MiniSquare25Compiler.Parser.Parser(tokens);
        var ast = parser.Parse();
        
        Assert.Single(ast.Declarations);
        Assert.Equal("x", ast.Declarations[0].Name);
        Assert.Equal("integer", ast.Declarations[0].Type);
    }

    [Fact]
    public void Parser_ShouldParseMultipleVariableDeclarations()
    {
        string source = "program Test; var x, y, z: integer; begin end;";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        var parser = new MiniSquare25Compiler.Parser.Parser(tokens);
        var ast = parser.Parse();
        
        Assert.Equal(3, ast.Declarations.Count);
        Assert.Equal("x", ast.Declarations[0].Name);
        Assert.Equal("y", ast.Declarations[1].Name);
        Assert.Equal("z", ast.Declarations[2].Name);
    }

    [Fact]
    public void Parser_ShouldParseAssignment()
    {
        string source = "program Test; var x: integer; begin x := 42; end;";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        var parser = new MiniSquare25Compiler.Parser.Parser(tokens);
        var ast = parser.Parse();
        
        Assert.Single(ast.Body.Statements);
        var assignment = ast.Body.Statements[0] as MiniSquare25Compiler.AST.AssignmentNode;
        Assert.NotNull(assignment);
        Assert.Equal("x", assignment.Variable);
    }
}

public class SemanticAnalyzerTests
{
    [Fact]
    public void SemanticAnalyzer_ShouldDetectUndeclaredVariable()
    {
        string source = "program Test; begin x := 42; end;";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        var parser = new MiniSquare25Compiler.Parser.Parser(tokens);
        var ast = parser.Parse();
        var analyzer = new SemanticAnalyzer();
        var errors = analyzer.Analyze(ast);
        
        Assert.NotEmpty(errors);
        Assert.Contains("not declared", errors[0]);
    }

    [Fact]
    public void SemanticAnalyzer_ShouldDetectTypeMismatch()
    {
        string source = "program Test; var x: integer; var y: boolean; begin x := y; end;";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        var parser = new MiniSquare25Compiler.Parser.Parser(tokens);
        var ast = parser.Parse();
        var analyzer = new SemanticAnalyzer();
        var errors = analyzer.Analyze(ast);
        
        Assert.NotEmpty(errors);
        Assert.Contains("Type mismatch", errors[0]);
    }

    [Fact]
    public void SemanticAnalyzer_ShouldAcceptCorrectProgram()
    {
        string source = "program Test; var x: integer; begin x := 42; end;";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        var parser = new MiniSquare25Compiler.Parser.Parser(tokens);
        var ast = parser.Parse();
        var analyzer = new SemanticAnalyzer();
        var errors = analyzer.Analyze(ast);
        
        Assert.Empty(errors);
    }
}

public class CodeGeneratorTests
{
    [Fact]
    public void CodeGenerator_ShouldGenerateSimpleProgram()
    {
        string source = "program Test; var x: integer; begin x := 42; write x; end;";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        var parser = new MiniSquare25Compiler.Parser.Parser(tokens);
        var ast = parser.Parse();
        var generator = new TAMCodeGenerator();
        var code = generator.Generate(ast);
        
        Assert.NotNull(code);
        Assert.Contains("PUSH 1", code);
        Assert.Contains("LOADL 42", code);
        Assert.Contains("STORE 0", code);
        Assert.Contains("LOAD 0", code);
        Assert.Contains("PUTINT", code);
        Assert.Contains("HALT", code);
    }

    [Fact]
    public void CodeGenerator_ShouldGenerateArithmeticOperations()
    {
        string source = "program Test; var x, y, z: integer; begin x := 10; y := 5; z := x + y; end;";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        var parser = new MiniSquare25Compiler.Parser.Parser(tokens);
        var ast = parser.Parse();
        var generator = new TAMCodeGenerator();
        var code = generator.Generate(ast);
        
        Assert.Contains("ADD", code);
    }

    [Fact]
    public void CodeGenerator_ShouldGenerateIfStatement()
    {
        string source = "program Test; var x: integer; begin if x > 0 then x := 1; end; end;";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        var parser = new MiniSquare25Compiler.Parser.Parser(tokens);
        var ast = parser.Parse();
        var generator = new TAMCodeGenerator();
        var code = generator.Generate(ast);
        
        Assert.Contains("GTR", code);
        Assert.Contains("JUMPIF(0)", code);
        Assert.Contains("JUMP", code);
    }

    [Fact]
    public void CodeGenerator_ShouldGenerateWhileLoop()
    {
        string source = "program Test; var x: integer; begin x := 5; while x > 0 do x := x - 1; end; end;";
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        var parser = new MiniSquare25Compiler.Parser.Parser(tokens);
        var ast = parser.Parse();
        var generator = new TAMCodeGenerator();
        var code = generator.Generate(ast);
        
        Assert.Contains("while_", code);
        Assert.Contains("endwhile_", code);
        Assert.Contains("GTR", code);
        Assert.Contains("SUB", code);
    }
}

