# CM4106-Compiler
Simple C# Mini-Square Compiler Solution 


# Working Log:

### 09/11/2025
Work Done:
  + Imported Lab8Solution model and studied the overall code structure.
  + Started implementing Tokenizer based on rules from doc specs.

To-Do:
  + Finish Tokenizer then move on to next steps
-----------------------------------

### 16/11/2025
Work Done:
  + Tokenizer is now completed, handling token types requested in the docs.
  + Started on Parser. Some commands still have to be adjusted. I also need to research on parsing new RepeatCommand and Unless Command.
    
To-Do:
  + Finish Parser (Synctatic Analysis), then move to Semantic.
-----------------------------------

### 19/11/2025
Work Done:
  + Tokenizer and Parser are now completed.
    
To-Do:
  + Perhaps minor issues present in Parser. I need to generate more code examples and check how my compiler behaves, however I will move on to creating AST now due to deadline.
-----------------------------------

### 21/11/2025
Work Done:
  + Semantic Analysis now includes Identification. There are a lot of issues and nodes are not identified properly. For example, instead of Var/Const Declarations, it shows SingleDeclarationNode.
  + AST Tree is now printable in terminal.
    
To-Do:
  + Fix Identification Issues
  + Implement Type Checking
-----------------------------------

### 22/11/2025
Work Done:
  + Added Type Checking template to Semantic Analysis. Changes to speceific methods are still required.

To-Do:
  + Fix Identification/Type Checking Issues
-----------------------------------

### 23/11/2025 19:44
Work Done:
  + Missing BeginCommand. The AST Tree connected LetCommand directly to SequentialCommand. This is now Fixed.
  + Wrong Node Names. The compiler now defines abstract nodes (VarDeclaration and ConstDeclaration), Instead of Passing everything to SingleDeclarationNode.
  + Minor Naming Conventions. Changed IntegerExpression -> IntExpression.

To-Do:
  + Study the compiler further for any Identification and Type Checking errors.
  + Implement more code examples for compiler.
  + Implement Code Generation.
-----------------------------------

### 24/11/2025 
Work Done:
  + Lab8Model Solution spelling changes (TypeDenoter -> TypeName)

To-Do:
-----------------------------------
