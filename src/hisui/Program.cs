using Cocona;
using hisui.TextWidth;

var app = CoconaApp.Create();
app.AddCommand(
    "f2h",
    (string src) =>
    {
        Console.WriteLine(TextWidthConverter.ToHalfWidth(src));
    }
);
app.AddCommand(
    "h2f",
    (string src) =>
    {
        Console.WriteLine(TextWidthConverter.ToFullWidth(src));
    }
);
app.Run();
