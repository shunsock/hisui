using System.CommandLine;
using System.CommandLine.Parsing;

namespace hisui;

class Program
{
    static int Main(string[] args)
    {
        Option<string> nameOption = new("--name")
        {
            Description = "The name to read and display on the console."
        };

        // RootCommand
        RootCommand rootCommand = new("Sample app for System.CommandLine");
        rootCommand.Options.Add(nameOption);

        // Parse
        ParseResult parseResult = rootCommand.Parse(args);
        if (parseResult.Errors.Count > 0)
        {
            foreach (ParseError parseError in parseResult.Errors)
            {
                Console.Error.WriteLine(parseError.Message);
                rootCommand.Parse("-h").Invoke();
            }
            return 1;
        }

        string? name = parseResult.GetValue(nameOption);
        if (name == null) {
          Console.Error.WriteLine("Error: empty name found. --name [string]");
          rootCommand.Parse("-h").Invoke();
          return 1;
        }
        Console.WriteLine($"Hello, {name}");
        return 0;
    }
}
