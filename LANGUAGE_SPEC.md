# MiniSquare25 Language Specification

## Overview
MiniSquare25 is a simple imperative programming language designed for educational purposes. It supports basic data types, control structures, and I/O operations.

## Program Structure

A MiniSquare25 program has the following structure:

```
program <name>;
[declarations]
begin
    [statements]
end;
```

## Lexical Elements

### Keywords
The following keywords are reserved and cannot be used as identifiers:
- `program` - Program declaration
- `begin` - Start of statement block
- `end` - End of statement block
- `var` - Variable declaration
- `const` - Constant declaration
- `integer` - Integer type
- `boolean` - Boolean type
- `if` - Conditional statement
- `then` - Conditional branch
- `else` - Alternative conditional branch
- `while` - Loop statement
- `do` - Loop body
- `read` - Input operation
- `write` - Output operation
- `true` - Boolean literal
- `false` - Boolean literal

### Identifiers
Identifiers must start with a letter or underscore, followed by any number of letters, digits, or underscores.

Pattern: `[a-zA-Z_][a-zA-Z0-9_]*`

Examples: `x`, `counter`, `my_var`, `value123`

### Literals
- **Integer literals**: Sequences of digits (e.g., `0`, `42`, `1000`)
- **Boolean literals**: `true` or `false`

### Operators

#### Arithmetic Operators
- `+` - Addition
- `-` - Subtraction
- `*` - Multiplication
- `/` - Division (integer division)

#### Comparison Operators
- `=` - Equal to
- `!=` - Not equal to
- `<` - Less than
- `>` - Greater than
- `<=` - Less than or equal to
- `>=` - Greater than or equal to

#### Logical Operators
- `&&` - Logical AND
- `||` - Logical OR
- `!` - Logical NOT

#### Assignment Operator
- `:=` - Assignment

### Delimiters
- `;` - Statement terminator
- `:` - Type separator
- `,` - List separator
- `(` `)` - Parentheses for grouping expressions

### Comments
Single-line comments start with `//` and continue to the end of the line.

```
// This is a comment
x := 42; // This is also a comment
```

## Declarations

### Variable Declarations
Variables must be declared before use. Multiple variables of the same type can be declared in a single statement.

Syntax:
```
var <name> [, <name>]* : <type>;
```

Examples:
```
var x: integer;
var i, j, k: integer;
var flag: boolean;
```

### Constant Declarations
Constants are declared with an initial value and cannot be modified.

Syntax:
```
const <name> : <type> = <expression>;
```

Examples:
```
const max: integer = 100;
const debug: boolean = false;
```

## Types

### Integer
The `integer` type represents signed integer values.

### Boolean
The `boolean` type represents logical values: `true` or `false`.

## Statements

### Assignment Statement
Assigns the value of an expression to a variable.

Syntax:
```
<identifier> := <expression>;
```

Example:
```
x := 42;
y := x + 10;
```

### If Statement
Conditionally executes statements based on a boolean expression.

Syntax:
```
if <condition> then
    [statements]
[else
    [statements]]
end;
```

Examples:
```
if x > 0 then
    y := 1;
end;

if x > 0 then
    y := 1;
else
    y := -1;
end;
```

### While Statement
Repeatedly executes statements while a condition is true.

Syntax:
```
while <condition> do
    [statements]
end;
```

Example:
```
while i < 10 do
    sum := sum + i;
    i := i + 1;
end;
```

### Read Statement
Reads an integer value from input and stores it in a variable.

Syntax:
```
read <identifier>;
```

Example:
```
read x;
```

### Write Statement
Writes the value of an expression to output.

Syntax:
```
write <expression>;
```

Example:
```
write x;
write x + y;
```

## Expressions

Expressions are evaluated according to standard operator precedence:

1. Parentheses `()`
2. Unary operators: `-` (negation), `!` (logical not)
3. Multiplicative: `*`, `/`
4. Additive: `+`, `-`
5. Relational: `<`, `>`, `<=`, `>=`
6. Equality: `=`, `!=`
7. Logical AND: `&&`
8. Logical OR: `||`

### Type Rules

- Arithmetic operators (`+`, `-`, `*`, `/`) require integer operands and produce integer results.
- Comparison operators (`<`, `>`, `<=`, `>=`) require integer operands and produce boolean results.
- Equality operators (`=`, `!=`) require operands of the same type and produce boolean results.
- Logical operators (`&&`, `||`, `!`) require boolean operands and produce boolean results.
- The unary minus (`-`) requires an integer operand and produces an integer result.

## Semantic Rules

1. All variables must be declared before use.
2. Variable names must be unique within a program.
3. Variables cannot be redeclared.
4. The type of the expression in an assignment must match the type of the variable.
5. The condition in an `if` or `while` statement must be of boolean type.
6. Only integer variables can be used with the `read` statement.
7. Constants cannot be assigned after declaration.

## Example Programs

### Simple Program
```
program Simple;
var x: integer;
begin
    x := 42;
    write x;
end;
```

### Factorial
```
program Factorial;
var n, result, i: integer;
begin
    read n;
    result := 1;
    i := 1;
    while i <= n do
        result := result * i;
        i := i + 1;
    end;
    write result;
end;
```

### Maximum of Two Numbers
```
program Maximum;
var a, b, max: integer;
begin
    read a;
    read b;
    if a > b then
        max := a;
    else
        max := b;
    end;
    write max;
end;
```

## Triangle Abstract Machine Output

The compiler generates code for the Triangle Abstract Machine (TAM), which is a stack-based virtual machine. The generated code consists of the following instructions:

- `PUSH n` - Allocate n words on the stack
- `LOADL n` - Load literal value n onto the stack
- `LOAD addr` - Load value from address onto the stack
- `STORE addr` - Store top of stack to address
- `ADD`, `SUB`, `MULT`, `DIV` - Arithmetic operations
- `LSS`, `GTR`, `LEQ`, `GEQ`, `EQL`, `NEQ` - Comparison operations
- `AND`, `OR`, `NOT` - Logical operations
- `NEG` - Negation
- `JUMP label` - Unconditional jump
- `JUMPIF(0) label` - Jump if top of stack is false (0)
- `GETINT` - Read integer from input
- `PUTINT` - Write integer to output
- `HALT` - Stop execution

## Compiler Phases

The MiniSquare25 compiler operates in four phases:

1. **Lexical Analysis**: Scans the source code and produces a sequence of tokens.
2. **Syntax Analysis**: Parses the token sequence and builds an Abstract Syntax Tree (AST).
3. **Semantic Analysis**: Performs type checking and validates semantic correctness.
4. **Code Generation**: Traverses the AST and generates Triangle Abstract Machine code.
