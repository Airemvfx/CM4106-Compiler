# CM4106-Compiler
Simple C# Mini-Square Compiler Solution 


# Fixes Log:

### 23/11/2025 19:44
Fixes:
  + Missing BeginCommand. The AST Tree connected LetCommand directly to SequentialCommand. This is now Fixed.
  + Wrong Node Names. The compiler now defines abstract nodes (VarDeclaration and ConstDeclaration), Instead of Passing everything to SingleDeclarationNode.
  + Minor Naming Conventions. Changed IntegerExpression -> IntExpression.

To-Do:
  + Study the compiler further for any Identification and Type Checking errors. Tokenizer and Parser are now fully functional.
  + Implement Code Generation
