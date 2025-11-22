using Compiler.IO;

namespace Compiler.Nodes
{
    /// <summary>
    /// A node corresponding to a begin block `{ C }` which just wraps a command.
    /// </summary>
    public class BeginCommandNode : ICommandNode
    {
        /// <summary>
        /// The contained command.
        /// </summary>
        public ICommandNode Command { get; }

        /// <summary>
        /// Position where the block begins (derived from child when not supplied).
        /// </summary>
        public Position Position { get; }

        /// <summary>
        /// Creates a new begin block node with explicit position.
        /// </summary>
        public BeginCommandNode(ICommandNode command, Position position)
        {
            Command = command;
            Position = position;
        }

        /// <summary>
        /// Creates a new begin block node deriving the position from the contained command.
        /// </summary>
        public BeginCommandNode(ICommandNode command)
            : this(command, command?.Position ?? Position.BuiltIn)
        {
        }
    }
}