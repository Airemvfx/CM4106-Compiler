namespace Compiler.Nodes
{
    /// <summary>
    /// A node corresponding to an unless ... do command
    /// </summary>
    public class UnlessCommandNode : ICommandNode
    {
        /// <summary>
        /// The condition expression
        /// </summary>
        public IExpressionNode Expression { get; }

        /// <summary>
        /// The body executed when condition is false
        /// </summary>
        public ICommandNode Command { get; }

        /// <summary>
        /// The position in the code where the content associated with the node begins
        /// </summary>
        public Position Position { get; }

        /// <summary>
        /// Creates a new unless node with explicit position
        /// </summary>
        public UnlessCommandNode(IExpressionNode expression, ICommandNode command, Position position)
        {
            Expression = expression;
            Command = command;
            Position = position;
        }

        /// <summary>
        /// Creates a new unless node and derives position from children when not provided
        /// </summary>
        public UnlessCommandNode(IExpressionNode expression, ICommandNode command)
            : this(expression, command, expression?.Position ?? (command?.Position ?? Position.BuiltIn))
        {
        }
    }
}