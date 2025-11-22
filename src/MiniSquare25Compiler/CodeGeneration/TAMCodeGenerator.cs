using System.Text;
using MiniSquare25Compiler.AST;

namespace MiniSquare25Compiler.CodeGeneration
{
    public class TAMCodeGenerator
    {
        private readonly StringBuilder _code = new();
        private readonly Dictionary<string, int> _variableAddresses = new();
        private int _nextAddress = 0;
        private int _labelCounter = 0;

        public string Generate(ProgramNode program)
        {
            _code.Clear();
            _variableAddresses.Clear();
            _nextAddress = 0;
            _labelCounter = 0;

            // Generate header comment
            _code.AppendLine($"// Triangle Abstract Machine code for program {program.Name}");
            _code.AppendLine();

            // Allocate space for variables
            foreach (var decl in program.Declarations)
            {
                if (decl is VarDeclarationNode)
                {
                    _variableAddresses[decl.Name] = _nextAddress;
                    _nextAddress++;
                }
                else if (decl is ConstDeclarationNode constDecl)
                {
                    // For constants, we can inline their values, but we'll still track them
                    _variableAddresses[decl.Name] = _nextAddress;
                    _nextAddress++;
                }
            }

            // Push space for all variables
            if (_nextAddress > 0)
            {
                _code.AppendLine($"PUSH {_nextAddress}");
            }

            // Initialize constants
            foreach (var decl in program.Declarations)
            {
                if (decl is ConstDeclarationNode constDecl)
                {
                    GenerateExpression(constDecl.Value);
                    int address = _variableAddresses[constDecl.Name];
                    _code.AppendLine($"STORE {address}");
                }
            }

            // Generate code for program body
            GenerateBlock(program.Body);

            // Halt the program
            _code.AppendLine("HALT");

            return _code.ToString();
        }

        private void GenerateBlock(BlockNode block)
        {
            foreach (var statement in block.Statements)
            {
                GenerateStatement(statement);
            }
        }

        private void GenerateStatement(StatementNode statement)
        {
            switch (statement)
            {
                case AssignmentNode assignment:
                    GenerateAssignment(assignment);
                    break;
                case IfNode ifNode:
                    GenerateIf(ifNode);
                    break;
                case WhileNode whileNode:
                    GenerateWhile(whileNode);
                    break;
                case ReadNode readNode:
                    GenerateRead(readNode);
                    break;
                case WriteNode writeNode:
                    GenerateWrite(writeNode);
                    break;
            }
        }

        private void GenerateAssignment(AssignmentNode assignment)
        {
            GenerateExpression(assignment.Expression);
            int address = _variableAddresses[assignment.Variable];
            _code.AppendLine($"STORE {address}");
        }

        private void GenerateIf(IfNode ifNode)
        {
            string elseLabel = GenerateLabel("else");
            string endLabel = GenerateLabel("endif");

            // Generate condition
            GenerateExpression(ifNode.Condition);

            // Jump to else if false
            _code.AppendLine($"JUMPIF(0) {elseLabel}");

            // Generate then block
            GenerateBlock(ifNode.ThenBlock);
            _code.AppendLine($"JUMP {endLabel}");

            // Generate else block
            _code.AppendLine($"{elseLabel}:");
            if (ifNode.ElseBlock != null)
            {
                GenerateBlock(ifNode.ElseBlock);
            }

            _code.AppendLine($"{endLabel}:");
        }

        private void GenerateWhile(WhileNode whileNode)
        {
            string startLabel = GenerateLabel("while");
            string endLabel = GenerateLabel("endwhile");

            _code.AppendLine($"{startLabel}:");

            // Generate condition
            GenerateExpression(whileNode.Condition);

            // Jump to end if false
            _code.AppendLine($"JUMPIF(0) {endLabel}");

            // Generate body
            GenerateBlock(whileNode.Body);

            // Jump back to start
            _code.AppendLine($"JUMP {startLabel}");

            _code.AppendLine($"{endLabel}:");
        }

        private void GenerateRead(ReadNode readNode)
        {
            _code.AppendLine("GETINT");
            int address = _variableAddresses[readNode.Variable];
            _code.AppendLine($"STORE {address}");
        }

        private void GenerateWrite(WriteNode writeNode)
        {
            GenerateExpression(writeNode.Expression);
            _code.AppendLine("PUTINT");
        }

        private void GenerateExpression(ExpressionNode expr)
        {
            switch (expr)
            {
                case IntLiteralNode intLit:
                    _code.AppendLine($"LOADL {intLit.Value}");
                    break;

                case BoolLiteralNode boolLit:
                    _code.AppendLine($"LOADL {(boolLit.Value ? 1 : 0)}");
                    break;

                case IdentifierNode identifier:
                    int address = _variableAddresses[identifier.Name];
                    _code.AppendLine($"LOAD {address}");
                    break;

                case BinaryExpressionNode binary:
                    GenerateBinaryExpression(binary);
                    break;

                case UnaryExpressionNode unary:
                    GenerateUnaryExpression(unary);
                    break;
            }
        }

        private void GenerateBinaryExpression(BinaryExpressionNode binary)
        {
            GenerateExpression(binary.Left);
            GenerateExpression(binary.Right);

            switch (binary.Operator)
            {
                case "+":
                    _code.AppendLine("ADD");
                    break;
                case "-":
                    _code.AppendLine("SUB");
                    break;
                case "*":
                    _code.AppendLine("MULT");
                    break;
                case "/":
                    _code.AppendLine("DIV");
                    break;
                case "<":
                    _code.AppendLine("LSS");
                    break;
                case ">":
                    _code.AppendLine("GTR");
                    break;
                case "<=":
                    _code.AppendLine("LEQ");
                    break;
                case ">=":
                    _code.AppendLine("GEQ");
                    break;
                case "=":
                    _code.AppendLine("EQL");
                    break;
                case "!=":
                    _code.AppendLine("NEQ");
                    break;
                case "&&":
                    _code.AppendLine("AND");
                    break;
                case "||":
                    _code.AppendLine("OR");
                    break;
            }
        }

        private void GenerateUnaryExpression(UnaryExpressionNode unary)
        {
            GenerateExpression(unary.Operand);

            switch (unary.Operator)
            {
                case "-":
                    _code.AppendLine("NEG");
                    break;
                case "!":
                    _code.AppendLine("NOT");
                    break;
            }
        }

        private string GenerateLabel(string prefix)
        {
            return $"{prefix}_{_labelCounter++}";
        }
    }
}
