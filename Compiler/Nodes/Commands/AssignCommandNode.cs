namespace Compiler.Nodes
{
    public sealed class AssignCommandNode : ICommandNode
    {
        public IdentifierNode Identifier { get; }
        public IExpressionNode Expression { get; }

        public Position Position { get { return Identifier.Position; } }

        public AssignCommandNode(IdentifierNode id, IExpressionNode expr)
        {
            Identifier = id;
            Expression = expr;
        }
    }
}