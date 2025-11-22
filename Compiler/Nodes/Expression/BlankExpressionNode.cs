namespace Compiler.Nodes
{
    /// <summary>
    /// Represents the absence of an expression (explicit blank expression).
    /// Used as an explicit initializer placeholder where the grammar allows no initializer.
    /// </summary>
    public class BlankExpressionNode
    {
        //public SimpleTypeDeclarationNode Type { get; set; }

        public Position Position { get; }

        public BlankExpressionNode(Position position)
        {
            Position = position;
        }
    }
}