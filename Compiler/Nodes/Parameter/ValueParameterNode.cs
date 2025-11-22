namespace Compiler.Nodes
{
    /// <summary>
    /// A node corresponding to a value (expression) parameter
    /// </summary>
    public class ValueParameterNode : IParameterNode
    {
        /// <summary>
        /// The expression supplied as the parameter value
        /// </summary>
        public IExpressionNode Expression { get; }

        /// <summary>
        /// The type of the parameter
        /// </summary>
        public SimpleTypeDeclarationNode Type { get; set; }

        /// <summary>
        /// The position in the code where the content associated with the node begins
        /// </summary>
        public Position Position { get; }

        /// <summary>
        /// Creates a new value parameter node
        /// </summary>
        /// <param name="expression">The expression supplying the value</param>
        public ValueParameterNode(IExpressionNode expression)
        {
            Expression = expression;
            Position = expression.Position;
        }
    }
}