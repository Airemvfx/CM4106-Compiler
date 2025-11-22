namespace Compiler.Nodes
{
    /// <summary>
    /// A node corresponding to a repeat ... until command
    /// </summary>
    public class RepeatCommandNode : ICommandNode
    {
        /// <summary>
        /// The body of the repeat
        /// </summary>
        public ICommandNode Command { get; }

        /// <summary>
        /// The until expression
        /// </summary>
        public IExpressionNode Expression { get; }

        /// <summary>
        /// The position in the code where the content associated with the node begins
        /// </summary>
        public Position Position { get; }

        /// <summary>
        /// Creates a new repeat node with explicit position
        /// </summary>
        public RepeatCommandNode(ICommandNode command, IExpressionNode expression, Position position)
        {
            Command = command;
            Expression = expression;
            Position = position;
        }

        /// <summary>
        /// Creates a new repeat node and derives position from children when not provided
        /// </summary>
        public RepeatCommandNode(ICommandNode command, IExpressionNode expression)
            : this(command, expression, command?.Position ?? (expression?.Position ?? Position.BuiltIn))
        {
        }
    }
}