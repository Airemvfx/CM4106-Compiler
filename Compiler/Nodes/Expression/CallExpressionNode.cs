namespace Compiler.Nodes
{
    /// <summary>
    /// A node corresponding to a call expression: identifier(<parameter>)
    /// </summary>
    public class CallExpressionNode : IExpressionNode
    {
        public IdentifierNode Identifier { get; }
        public IParameterNode Parameter { get; }

        public Position Position { get { return Identifier.Position; } }

        public SimpleTypeDeclarationNode Type { get; set; }

        public CallExpressionNode(IdentifierNode identifier, IParameterNode parameter)
        {
            Identifier = identifier;
            Parameter = parameter;
        }
    }
}