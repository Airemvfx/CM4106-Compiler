using MiniSquare25Compiler.AST;

namespace MiniSquare25Compiler.SemanticAnalysis
{
    public class SemanticAnalyzer
    {
        private readonly Dictionary<string, string> _symbolTable = new();
        private readonly List<string> _errors = new();

        public List<string> Analyze(ProgramNode program)
        {
            _errors.Clear();
            _symbolTable.Clear();

            // Add declarations to symbol table
            foreach (var decl in program.Declarations)
            {
                if (_symbolTable.ContainsKey(decl.Name))
                {
                    _errors.Add($"Variable '{decl.Name}' is already declared");
                }
                else
                {
                    _symbolTable[decl.Name] = decl.Type;
                }
            }

            // Analyze the program body
            AnalyzeBlock(program.Body);

            return _errors;
        }

        private void AnalyzeBlock(BlockNode block)
        {
            foreach (var statement in block.Statements)
            {
                AnalyzeStatement(statement);
            }
        }

        private void AnalyzeStatement(StatementNode statement)
        {
            switch (statement)
            {
                case AssignmentNode assignment:
                    AnalyzeAssignment(assignment);
                    break;
                case IfNode ifNode:
                    AnalyzeIf(ifNode);
                    break;
                case WhileNode whileNode:
                    AnalyzeWhile(whileNode);
                    break;
                case ReadNode readNode:
                    AnalyzeRead(readNode);
                    break;
                case WriteNode writeNode:
                    AnalyzeWrite(writeNode);
                    break;
            }
        }

        private void AnalyzeAssignment(AssignmentNode assignment)
        {
            if (!_symbolTable.ContainsKey(assignment.Variable))
            {
                _errors.Add($"Variable '{assignment.Variable}' is not declared");
                return;
            }

            string varType = _symbolTable[assignment.Variable];
            string exprType = GetExpressionType(assignment.Expression);

            if (varType != exprType)
            {
                _errors.Add($"Type mismatch in assignment to '{assignment.Variable}': expected {varType}, got {exprType}");
            }
        }

        private void AnalyzeIf(IfNode ifNode)
        {
            string condType = GetExpressionType(ifNode.Condition);
            if (condType != "boolean")
            {
                _errors.Add($"If condition must be boolean, got {condType}");
            }

            AnalyzeBlock(ifNode.ThenBlock);
            if (ifNode.ElseBlock != null)
            {
                AnalyzeBlock(ifNode.ElseBlock);
            }
        }

        private void AnalyzeWhile(WhileNode whileNode)
        {
            string condType = GetExpressionType(whileNode.Condition);
            if (condType != "boolean")
            {
                _errors.Add($"While condition must be boolean, got {condType}");
            }

            AnalyzeBlock(whileNode.Body);
        }

        private void AnalyzeRead(ReadNode readNode)
        {
            if (!_symbolTable.ContainsKey(readNode.Variable))
            {
                _errors.Add($"Variable '{readNode.Variable}' is not declared");
            }
        }

        private void AnalyzeWrite(WriteNode writeNode)
        {
            GetExpressionType(writeNode.Expression);
        }

        private string GetExpressionType(ExpressionNode expr)
        {
            switch (expr)
            {
                case IntLiteralNode _:
                    expr.ExprType = "integer";
                    return "integer";

                case BoolLiteralNode _:
                    expr.ExprType = "boolean";
                    return "boolean";

                case IdentifierNode identifier:
                    if (!_symbolTable.ContainsKey(identifier.Name))
                    {
                        _errors.Add($"Variable '{identifier.Name}' is not declared");
                        expr.ExprType = "error";
                        return "error";
                    }
                    expr.ExprType = _symbolTable[identifier.Name];
                    return expr.ExprType;

                case BinaryExpressionNode binary:
                    return AnalyzeBinaryExpression(binary);

                case UnaryExpressionNode unary:
                    return AnalyzeUnaryExpression(unary);

                default:
                    expr.ExprType = "error";
                    return "error";
            }
        }

        private string AnalyzeBinaryExpression(BinaryExpressionNode binary)
        {
            string leftType = GetExpressionType(binary.Left);
            string rightType = GetExpressionType(binary.Right);

            switch (binary.Operator)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                    if (leftType != "integer" || rightType != "integer")
                    {
                        _errors.Add($"Arithmetic operators require integer operands, got {leftType} and {rightType}");
                        binary.ExprType = "error";
                        return "error";
                    }
                    binary.ExprType = "integer";
                    return "integer";

                case "<":
                case ">":
                case "<=":
                case ">=":
                    if (leftType != "integer" || rightType != "integer")
                    {
                        _errors.Add($"Relational operators require integer operands, got {leftType} and {rightType}");
                        binary.ExprType = "error";
                        return "error";
                    }
                    binary.ExprType = "boolean";
                    return "boolean";

                case "=":
                case "!=":
                    if (leftType != rightType)
                    {
                        _errors.Add($"Equality operators require same type operands, got {leftType} and {rightType}");
                        binary.ExprType = "error";
                        return "error";
                    }
                    binary.ExprType = "boolean";
                    return "boolean";

                case "&&":
                case "||":
                    if (leftType != "boolean" || rightType != "boolean")
                    {
                        _errors.Add($"Logical operators require boolean operands, got {leftType} and {rightType}");
                        binary.ExprType = "error";
                        return "error";
                    }
                    binary.ExprType = "boolean";
                    return "boolean";

                default:
                    _errors.Add($"Unknown operator: {binary.Operator}");
                    binary.ExprType = "error";
                    return "error";
            }
        }

        private string AnalyzeUnaryExpression(UnaryExpressionNode unary)
        {
            string operandType = GetExpressionType(unary.Operand);

            switch (unary.Operator)
            {
                case "-":
                    if (operandType != "integer")
                    {
                        _errors.Add($"Unary minus requires integer operand, got {operandType}");
                        unary.ExprType = "error";
                        return "error";
                    }
                    unary.ExprType = "integer";
                    return "integer";

                case "!":
                    if (operandType != "boolean")
                    {
                        _errors.Add($"Logical not requires boolean operand, got {operandType}");
                        unary.ExprType = "error";
                        return "error";
                    }
                    unary.ExprType = "boolean";
                    return "boolean";

                default:
                    _errors.Add($"Unknown operator: {unary.Operator}");
                    unary.ExprType = "error";
                    return "error";
            }
        }
    }
}
