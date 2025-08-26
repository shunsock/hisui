# hisui

hisui (翡翠) is a .NET 9.0 console application for text width conversion, particularly useful for Japanese text processing. The name comes from the green ornamental stone valued for its beauty and cultural significance.

## Features

- **Full-width to Half-width conversion** (`f2h`): Convert full-width characters to half-width
- **Half-width to Full-width conversion** (`h2f`): Convert half-width characters to full-width

Supported character conversions:
- English letters, numbers, and basic symbols (FF01–FF5E ⇔ 21–7E)
- Spaces (U+3000 ⇔ U+0020)
- Half-width Katakana to full-width Katakana (using NFKC normalization)

## Installation

### Prerequisites

- .NET 9.0 SDK
- Task (optional, for using Taskfile commands)
- Nix (optional, for reproducible development environment)

### Building from source

#### Using Nix (Recommended)

```bash
# Clone the repository
git clone <repository-url>
cd hisui

# Enter the development environment
nix develop

# Build and run the application
task build
dotnet run --project src/hisui
```

#### Manual setup

```bash
# Clone the repository
git clone <repository-url>
cd hisui

# Build the application
task build
# or
dotnet build

# Run the application
dotnet run --project src/hisui
```

## Usage

### Command Line Interface

Convert full-width text to half-width:
```bash
dotnet run --project src/hisui f2h "ｈｅｌｌｏ　ｗｏｒｌｄ"
# Output: hello world
```

Convert half-width text to full-width:
```bash
dotnet run --project src/hisui h2f "hello world"
# Output: ｈｅｌｌｏ　ｗｏｒｌｄ
```

### Examples with Japanese Text

Half-width Katakana to full-width Katakana:
```bash
dotnet run --project src/hisui h2f "ｶﾀｶﾅ"
# Output: カタカナ
```

Full-width alphanumeric to half-width:
```bash
dotnet run --project src/hisui f2h "１２３ＡＢＣ"
# Output: 123ABC
```

## Development

### Development Environment

This project includes a Nix flake for reproducible development environments. The Nix environment provides:

- .NET 9.0 SDK
- Task runner (go-task)
- tree command for directory visualization
- Properly configured DOTNET_ROOT environment variable

To enter the development environment:
```bash
nix develop
```

The shell will display version information for all included tools upon entry.

### Available Commands

Using Task (recommended):
- `task build` - Build the application
- `task test` - Run tests
- `task format` - Format code
- `task init <project_name>` - Initialize new .NET project

Using dotnet directly:
- `dotnet build` - Build the solution
- `dotnet test` - Run all tests
- `dotnet format` - Format code according to .NET standards
- `dotnet run --project src/hisui` - Run the main application

### Architecture

The project is structured as follows:
- `src/hisui/` - Main console application using Cocona framework
- `src/hisui/TextWidth/TextWidthConverter.cs` - Core text width conversion utilities
- `tests/hisui.Tests/` - Unit tests using NUnit framework
- `script/` - Build and utility scripts

### Dependencies

**Main Application:**
- Cocona 2.2.0 - CLI framework for command handling
- .NET 9.0 - Target framework with nullable reference types enabled

**Test Dependencies:**
- NUnit 4.2.2 - Unit testing framework
- Microsoft.NET.Test.Sdk 17.12.0 - .NET test SDK
- NUnit3TestAdapter 4.6.0 - Test adapter for running NUnit tests
- coverlet.collector 6.0.2 - Code coverage collection

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Run tests: `task test`
5. Format code: `task format`
6. Submit a pull request

## License

See [LICENSE](LICENSE) file for details.
