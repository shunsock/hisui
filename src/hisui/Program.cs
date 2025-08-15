using System.CommandLine;
using System.CommandLine.Parsing;

namespace hisui;

class Program
{
    static int Main(string[] args)
    {
        Option<string> nameOption = new("--name")
        {
            Description = "The name to read and display on the console.",
            Required = true
        };

        // RootCommand
        RootCommand rootCommand = new("Sample app for System.CommandLine");
        rootCommand.Options.Add(nameOption);

        // Parse
        ParseResult parseResult = rootCommand.Parse(args);
        if (parseResult.Errors.Count > 0)
        {
            rootCommand.Parse("-h").Invoke();
            foreach (ParseError parseError in parseResult.Errors)
            {
                Console.Error.WriteLine(parseError.Message);
            }
            return 1;
        }

        string? name = parseResult.GetValue(nameOption);
        Console.WriteLine($"Hello, {name}");
        return 0;
    }
}
