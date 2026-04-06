# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Hisui is a .NET 10.0 console application built with the NextCocona CLI framework. The project includes text width conversion utilities for handling full-width/half-width character transformations (commonly used in Japanese text processing).

## Development Commands

This project uses Task (Taskfile.yml) as the primary task runner:

- **Build**: `task build` (runs `dotnet build`)
- **Test**: `task test` (runs `dotnet test`)
- **Format**: `task format` (runs `dotnet format`)
- **Initialize**: `task init <project_name>` (creates new .NET project)

Direct dotnet commands also work:
- `dotnet build` - Build the solution
- `dotnet test` - Run all tests
- `dotnet format` - Format code according to .NET standards
- `dotnet run --project src/hisui` - Run the main application

## Architecture

- **Main application**: `src/hisui/` - Console CLI application using NextCocona framework
- **Tests**: `tests/hisui.Tests/` - Unit tests using NUnit framework
- **Utilities**: 
  - `TextWidth/TextWidthConverter.cs` - Handles full-width/half-width character conversions for Japanese text

The application is structured as a simple CLI with command-based architecture using NextCocona for command parsing and routing.

## Key Dependencies

### Main Application
- **NextCocona (1.0.0)**: CLI framework for command handling
- **.NET 10.0**: Target framework with nullable reference types enabled

### Test Dependencies
- **NUnit (4.2.2)**: Unit testing framework
- **Microsoft.NET.Test.Sdk (17.12.0)**: .NET test SDK
- **NUnit3TestAdapter (4.6.0)**: Test adapter for running NUnit tests
- **coverlet.collector (6.0.2)**: Code coverage collection

## Build Scripts

The `script/` directory contains bash scripts for common tasks:
- `build_dotnet_project.sh` - Wrapper for dotnet build
- `test_dotnet_application.sh` - Wrapper for dotnet test  
- `format_dotnet_project.sh` - Wrapper for dotnet format
- `initialize_dotnet_project.sh` - Script for initializing new .NET projects