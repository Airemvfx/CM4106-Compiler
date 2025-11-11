using MiniSquare25Compiler.Lexer;
using MiniSquare25Compiler.AST;

namespace MiniSquare25Compiler.Parser
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _current = 0;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public ProgramNode Parse()
        {
            return ParseProgram();
        }

        private ProgramNode ParseProgram()
        {
            Consume(TokenType.Program, "Expected 'program' keyword");
            Token nameToken = Consume(TokenType.Identifier, "Expected program name");
            Consume(TokenType.Semicolon, "Expected ';' after program name");

            var declarations = new List<DeclarationNode>();
            while (Check(TokenType.Var) || Check(TokenType.Const))
            {
                if (Check(TokenType.Var))
                {
                    declarations.AddRange(ParseVarDeclaration());
                }
                else
                {
                    declarations.Add(ParseConstDeclaration());
                }
            }

            Consume(TokenType.Begin, "Expected 'begin'");
            var body = ParseBlock();
            Consume(TokenType.End, "Expected 'end'");
            Consume(TokenType.Semicolon, "Expected ';' after 'end'");

            var program = new ProgramNode(nameToken.Lexeme, body);
            program.Declarations = declarations;
            return program;
        }

        private List<VarDeclarationNode> ParseVarDeclaration()
        {
            Consume(TokenType.Var, "Expected 'var'");
            var declarations = new List<VarDeclarationNode>();
            var names = new List<string>();

            // Collect all variable names
            do
            {
                Token nameToken = Consume(TokenType.Identifier, "Expected variable name");
                names.Add(nameToken.Lexeme);

                if (!Check(TokenType.Comma)) break;
                Advance();
            } while (!Check(TokenType.Colon));

            Consume(TokenType.Colon, "Expected ':' after variable name(s)");
            Token typeToken = ConsumeType("Expected type");

            // Create a declaration for each variable with the same type
            foreach (var name in names)
            {
                declarations.Add(new VarDeclarationNode(name, typeToken.Lexeme));
            }

            Consume(TokenType.Semicolon, "Expected ';' after variable declaration");
            return declarations;
        }

        private ConstDeclarationNode ParseConstDeclaration()
        {
            Consume(TokenType.Const, "Expected 'const'");
            Token nameToken = Consume(TokenType.Identifier, "Expected constant name");
            Consume(TokenType.Colon, "Expected ':' after constant name");
            Token typeToken = ConsumeType("Expected type");
            Consume(TokenType.Equals, "Expected '=' after type");
            var value = ParseExpression();
            Consume(TokenType.Semicolon, "Expected ';' after constant declaration");

            return new ConstDeclarationNode(nameToken.Lexeme, typeToken.Lexeme, value);
        }

        private Token ConsumeType(string message)
        {
            if (Check(TokenType.Integer) || Check(TokenType.Boolean))
            {
                return Advance();
            }
            throw new Exception($"{message} at line {Peek().Line}");
        }

        private BlockNode ParseBlock()
        {
            var block = new BlockNode();

            while (!Check(TokenType.End) && !Check(TokenType.Else) && !IsAtEnd())
            {
                block.Statements.Add(ParseStatement());
            }

            return block;
        }

        private StatementNode ParseStatement()
        {
            if (Check(TokenType.Identifier))
            {
                return ParseAssignment();
            }
            else if (Check(TokenType.If))
            {
                return ParseIfStatement();
            }
            else if (Check(TokenType.While))
            {
                return ParseWhileStatement();
            }
            else if (Check(TokenType.Read))
            {
                return ParseReadStatement();
            }
            else if (Check(TokenType.Write))
            {
                return ParseWriteStatement();
            }
            else
            {
                throw new Exception($"Unexpected token {Peek().Type} at line {Peek().Line}");
            }
        }

        private AssignmentNode ParseAssignment()
        {
            Token nameToken = Consume(TokenType.Identifier, "Expected variable name");
            Consume(TokenType.Assign, "Expected ':=' in assignment");
            var expr = ParseExpression();
            Consume(TokenType.Semicolon, "Expected ';' after assignment");

            return new AssignmentNode(nameToken.Lexeme, expr);
        }

        private IfNode ParseIfStatement()
        {
            Consume(TokenType.If, "Expected 'if'");
            var condition = ParseExpression();
            Consume(TokenType.Then, "Expected 'then'");
            var thenBlock = ParseBlock();

            BlockNode? elseBlock = null;
            if (Match(TokenType.Else))
            {
                elseBlock = ParseBlock();
            }

            Consume(TokenType.End, "Expected 'end' after if statement");
            Consume(TokenType.Semicolon, "Expected ';' after if statement");

            return new IfNode(condition, thenBlock, elseBlock);
        }

        private WhileNode ParseWhileStatement()
        {
            Consume(TokenType.While, "Expected 'while'");
            var condition = ParseExpression();
            Consume(TokenType.Do, "Expected 'do'");
            var body = ParseBlock();
            Consume(TokenType.End, "Expected 'end' after while statement");
            Consume(TokenType.Semicolon, "Expected ';' after while statement");

            return new WhileNode(condition, body);
        }

        private ReadNode ParseReadStatement()
        {
            Consume(TokenType.Read, "Expected 'read'");
            Token varToken = Consume(TokenType.Identifier, "Expected variable name");
            Consume(TokenType.Semicolon, "Expected ';' after read statement");

            return new ReadNode(varToken.Lexeme);
        }

        private WriteNode ParseWriteStatement()
        {
            Consume(TokenType.Write, "Expected 'write'");
            var expr = ParseExpression();
            Consume(TokenType.Semicolon, "Expected ';' after write statement");

            return new WriteNode(expr);
        }

        private ExpressionNode ParseExpression()
        {
            return ParseOrExpression();
        }

        private ExpressionNode ParseOrExpression()
        {
            var expr = ParseAndExpression();

            while (Match(TokenType.Or))
            {
                string op = Previous().Lexeme;
                var right = ParseAndExpression();
                expr = new BinaryExpressionNode(expr, op, right);
            }

            return expr;
        }

        private ExpressionNode ParseAndExpression()
        {
            var expr = ParseEqualityExpression();

            while (Match(TokenType.And))
            {
                string op = Previous().Lexeme;
                var right = ParseEqualityExpression();
                expr = new BinaryExpressionNode(expr, op, right);
            }

            return expr;
        }

        private ExpressionNode ParseEqualityExpression()
        {
            var expr = ParseRelationalExpression();

            while (Match(TokenType.Equals, TokenType.NotEquals))
            {
                string op = Previous().Lexeme;
                var right = ParseRelationalExpression();
                expr = new BinaryExpressionNode(expr, op, right);
            }

            return expr;
        }

        private ExpressionNode ParseRelationalExpression()
        {
            var expr = ParseAdditiveExpression();

            while (Match(TokenType.LessThan, TokenType.GreaterThan, 
                        TokenType.LessOrEqual, TokenType.GreaterOrEqual))
            {
                string op = Previous().Lexeme;
                var right = ParseAdditiveExpression();
                expr = new BinaryExpressionNode(expr, op, right);
            }

            return expr;
        }

        private ExpressionNode ParseAdditiveExpression()
        {
            var expr = ParseMultiplicativeExpression();

            while (Match(TokenType.Plus, TokenType.Minus))
            {
                string op = Previous().Lexeme;
                var right = ParseMultiplicativeExpression();
                expr = new BinaryExpressionNode(expr, op, right);
            }

            return expr;
        }

        private ExpressionNode ParseMultiplicativeExpression()
        {
            var expr = ParseUnaryExpression();

            while (Match(TokenType.Multiply, TokenType.Divide))
            {
                string op = Previous().Lexeme;
                var right = ParseUnaryExpression();
                expr = new BinaryExpressionNode(expr, op, right);
            }

            return expr;
        }

        private ExpressionNode ParseUnaryExpression()
        {
            if (Match(TokenType.Not, TokenType.Minus))
            {
                string op = Previous().Lexeme;
                var operand = ParseUnaryExpression();
                return new UnaryExpressionNode(op, operand);
            }

            return ParsePrimaryExpression();
        }

        private ExpressionNode ParsePrimaryExpression()
        {
            if (Match(TokenType.IntLiteral))
            {
                return new IntLiteralNode((int)Previous().Value!);
            }

            if (Match(TokenType.BoolLiteral))
            {
                return new BoolLiteralNode((bool)Previous().Value!);
            }

            if (Match(TokenType.Identifier))
            {
                return new IdentifierNode(Previous().Lexeme);
            }

            if (Match(TokenType.LeftParen))
            {
                var expr = ParseExpression();
                Consume(TokenType.RightParen, "Expected ')' after expression");
                return expr;
            }

            throw new Exception($"Unexpected token {Peek().Type} at line {Peek().Line}");
        }

        private bool Match(params TokenType[] types)
        {
            foreach (var type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            throw new Exception($"{message} at line {Peek().Line}");
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) _current++;
            return Previous();
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EndOfFile;
        }

        private Token Peek()
        {
            return _tokens[_current];
        }

        private Token Previous()
        {
            return _tokens[_current - 1];
        }
    }
}
