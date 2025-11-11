namespace MiniSquare25Compiler.AST
{
    // Base class for all AST nodes
    public abstract class ASTNode
    {
        public int Line { get; set; }
        public int Column { get; set; }
    }

    // Program node
    public class ProgramNode : ASTNode
    {
        public string Name { get; set; }
        public List<DeclarationNode> Declarations { get; set; } = new();
        public BlockNode Body { get; set; }

        public ProgramNode(string name, BlockNode body)
        {
            Name = name;
            Body = body;
        }
    }

    // Block node
    public class BlockNode : ASTNode
    {
        public List<StatementNode> Statements { get; set; } = new();
    }

    // Base for all declarations
    public abstract class DeclarationNode : ASTNode
    {
        public string Name { get; set; }
        public string Type { get; set; }

        protected DeclarationNode(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }

    // Variable declaration
    public class VarDeclarationNode : DeclarationNode
    {
        public VarDeclarationNode(string name, string type) : base(name, type) { }
    }

    // Constant declaration
    public class ConstDeclarationNode : DeclarationNode
    {
        public ExpressionNode Value { get; set; }

        public ConstDeclarationNode(string name, string type, ExpressionNode value) 
            : base(name, type)
        {
            Value = value;
        }
    }

    // Base for all statements
    public abstract class StatementNode : ASTNode { }

    // Assignment statement
    public class AssignmentNode : StatementNode
    {
        public string Variable { get; set; }
        public ExpressionNode Expression { get; set; }

        public AssignmentNode(string variable, ExpressionNode expression)
        {
            Variable = variable;
            Expression = expression;
        }
    }

    // If statement
    public class IfNode : StatementNode
    {
        public ExpressionNode Condition { get; set; }
        public BlockNode ThenBlock { get; set; }
        public BlockNode? ElseBlock { get; set; }

        public IfNode(ExpressionNode condition, BlockNode thenBlock, BlockNode? elseBlock = null)
        {
            Condition = condition;
            ThenBlock = thenBlock;
            ElseBlock = elseBlock;
        }
    }

    // While statement
    public class WhileNode : StatementNode
    {
        public ExpressionNode Condition { get; set; }
        public BlockNode Body { get; set; }

        public WhileNode(ExpressionNode condition, BlockNode body)
        {
            Condition = condition;
            Body = body;
        }
    }

    // Read statement
    public class ReadNode : StatementNode
    {
        public string Variable { get; set; }

        public ReadNode(string variable)
        {
            Variable = variable;
        }
    }

    // Write statement
    public class WriteNode : StatementNode
    {
        public ExpressionNode Expression { get; set; }

        public WriteNode(ExpressionNode expression)
        {
            Expression = expression;
        }
    }

    // Base for all expressions
    public abstract class ExpressionNode : ASTNode
    {
        public string? ExprType { get; set; } // Type of the expression (for semantic analysis)
    }

    // Binary expression
    public class BinaryExpressionNode : ExpressionNode
    {
        public ExpressionNode Left { get; set; }
        public string Operator { get; set; }
        public ExpressionNode Right { get; set; }

        public BinaryExpressionNode(ExpressionNode left, string op, ExpressionNode right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }

    // Unary expression
    public class UnaryExpressionNode : ExpressionNode
    {
        public string Operator { get; set; }
        public ExpressionNode Operand { get; set; }

        public UnaryExpressionNode(string op, ExpressionNode operand)
        {
            Operator = op;
            Operand = operand;
        }
    }

    // Integer literal
    public class IntLiteralNode : ExpressionNode
    {
        public int Value { get; set; }

        public IntLiteralNode(int value)
        {
            Value = value;
            ExprType = "integer";
        }
    }

    // Boolean literal
    public class BoolLiteralNode : ExpressionNode
    {
        public bool Value { get; set; }

        public BoolLiteralNode(bool value)
        {
            Value = value;
            ExprType = "boolean";
        }
    }

    // Identifier (variable reference)
    public class IdentifierNode : ExpressionNode
    {
        public string Name { get; set; }

        public IdentifierNode(string name)
        {
            Name = name;
        }
    }
}
