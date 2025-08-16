using Cocona;

var app = CoconaApp.Create();
app.AddCommand(
    "hello",
    (string name) =>
    {
        Console.WriteLine("Hello {0}!", name);
    }
);
app.Run();
