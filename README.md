# CM4106-Compiler
Simple C# MiniSquare25 Compiler Solution

## Overview
This project implements a working compiler for the MiniSquare25 programming language that generates Triangle Abstract Machine (TAM) code.

## Features
- **Lexical Analysis**: Tokenizes MiniSquare25 source code
- **Syntax Analysis**: Parses tokens into an Abstract Syntax Tree (AST)
- **Semantic Analysis**: Validates type correctness and variable declarations
- **Code Generation**: Produces Triangle Abstract Machine instructions

## MiniSquare25 Language

### Keywords
- `program`, `begin`, `end` - Program structure
- `var`, `const` - Declarations
- `integer`, `boolean` - Types
- `if`, `then`, `else` - Conditional execution
- `while`, `do` - Loops
- `read`, `write` - I/O operations
- `true`, `false` - Boolean literals

### Operators
- Arithmetic: `+`, `-`, `*`, `/`
- Comparison: `=`, `!=`, `<`, `>`, `<=`, `>=`
- Logical: `&&`, `||`, `!`
- Assignment: `:=`

### Example Program
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

## Building the Project

### Prerequisites
- .NET 9.0 SDK or later

### Build
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

## Usage

### Compile a MiniSquare25 program
```bash
dotnet run --project src/MiniSquare25Compiler/MiniSquare25Compiler.csproj <source-file> [output-file]
```

### Example
```bash
dotnet run --project src/MiniSquare25Compiler/MiniSquare25Compiler.csproj examples/hello.ms25
```

This will generate `examples/hello.tam` containing the Triangle Abstract Machine code.

## Project Structure
```
.
├── src/
│   └── MiniSquare25Compiler/
│       ├── Lexer/              # Lexical analyzer
│       ├── Parser/             # Syntax analyzer
│       ├── AST/                # Abstract Syntax Tree definitions
│       ├── SemanticAnalysis/   # Semantic analyzer
│       ├── CodeGeneration/     # TAM code generator
│       └── Program.cs          # Main compiler driver
├── tests/
│   └── MiniSquare25Compiler.Tests/
│       └── UnitTest1.cs        # Unit tests
└── examples/                   # Sample MiniSquare25 programs
    ├── hello.ms25
    ├── factorial.ms25
    └── ifelse.ms25
```

## Triangle Abstract Machine Instructions

The compiler generates the following TAM instructions:

- `PUSH n` - Allocate n words on stack
- `LOADL n` - Load literal value n
- `LOAD addr` - Load value from address
- `STORE addr` - Store value to address
- `ADD`, `SUB`, `MULT`, `DIV` - Arithmetic operations
- `LSS`, `GTR`, `LEQ`, `GEQ`, `EQL`, `NEQ` - Comparison operations
- `AND`, `OR`, `NOT` - Logical operations
- `NEG` - Negation
- `JUMP label` - Unconditional jump
- `JUMPIF(0) label` - Jump if top of stack is false
- `GETINT` - Read integer from input
- `PUTINT` - Write integer to output
- `HALT` - Stop execution

## License
Educational project for CM4106 course.
 
