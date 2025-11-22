using Compiler.IO;
using Compiler.Tokenization;

namespace Compiler.Nodes
{
    /// <summary>
    /// Represents a single variable or constant declaration node in the AST.
    /// </summary>
    public class SingleDeclarationNode : IDeclarationNode
    {
        public TypeDenoterNode TypeDenoter { get; }
        public IdentifierNode Identifier { get; }
        public IExpressionNode Initializer { get; }
        public Position Position { get; }

        public SingleDeclarationNode(TypeDenoterNode typeDenoter, IdentifierNode identifier, IExpressionNode initializer)
        {
            TypeDenoter = typeDenoter;
            Identifier = identifier;
            //// If parser passed null (allowed in C# 7.3), convert to an explicit BlankExpressionNode
            //Initializer = initializer  new BlankExpressionNode(typeDenoter.Position);
            Position = typeDenoter.Position;
        }
    }
}