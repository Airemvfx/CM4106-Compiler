namespace Compiler.Nodes
{
    /// <summary>
    /// A node corresponding to a built in function declaration
    /// </summary>
    public class FunctionDeclarationNode : ITypeDeclarationNode
    {
        /// <summary>
        /// The name of the function
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The type signature of the function
        /// </summary>
        public FunctionTypeDeclarationNode Type { get; }


        /// <summary>
        /// The position in the code where the content associated with the node begins
        /// </summary>
        public Position Position { get { return Position.BuiltIn; } }

        /// <summary>
        /// Creates a new function declaration node
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="primitive">The built-in function that the declaration refers to</param>
        /// <param name="returnType">The return type of the function</param>
        /// <param name="parameters">The parameters taken by the function</param>
        public FunctionDeclarationNode(string name, SimpleTypeDeclarationNode returnType, params (SimpleTypeDeclarationNode type, bool byRef)[] parameters)
        {
            Name = name;
            Type = new FunctionTypeDeclarationNode(returnType, parameters);
        }

        /// <inheritDoc />
        public override string ToString()
        {
            return Name + Type;
        }
    }
}