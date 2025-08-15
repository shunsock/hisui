# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Hisui is a .NET 9.0 console application built with the Cocona CLI framework. The project includes text width conversion utilities for handling full-width/half-width character transformations (commonly used in Japanese text processing).

## Development Commands

This project uses Task (Taskfile.yml) as the primary task runner:

- **Build (development)**: `task build:dev` (runs `dotnet build`)
- **Build (production)**: `task build:prod -- <runtime>` (AOT compilation for osx-arm64|linux-x64|linux-arm64|win-x64)
- **Test**: `task test` (runs `dotnet test`)
- **Format**: `task format` (runs `dotnet format`)

Direct dotnet commands also work:
- `dotnet build` - Build the solution
- `dotnet test` - Run all tests
- `dotnet format` - Format code according to .NET standards
- `dotnet run --project src/hisui` - Run the main application

## Architecture

- **Main application**: `src/hisui/` - Console CLI application using Cocona framework
- **Tests**: `tests/hisui.Tests/` - Unit tests using .NET test framework
- **Utilities**: 
  - `TextWidthConverter.cs` - Handles full-width/half-width character conversions for Japanese text

The application is structured as a simple CLI with command-based architecture using Cocona for command parsing and routing.

## Key Dependencies

- **Cocona (2.2.0)**: CLI framework for command handling
- **.NET 9.0**: Target framework with nullable reference types enabled

## Build Scripts

The `script/` directory contains bash scripts for common tasks:
- `build_dotnet_project.sh` - Wrapper for dotnet build
- `test_dotnet_application.sh` - Wrapper for dotnet test  
- `format_dotnet_project.sh` - Wrapper for dotnet format
- `aot_compile_dotnet_application.sh` - AOT compilation script