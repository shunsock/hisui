using Cocona;
using Hisui.Domain.ValueObject;
using Hisui.Infrastructure.Presenter;
using Hisui.UseCase.Handler;
using Hisui.UseCase.Query;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<IConvertTextWidthHandler, ConvertTextWidthHandler>();
builder.Services.AddSingleton<IOutputPort, ConsoleOutputAdapter>();

var app = builder.Build();

app.AddCommand("f2h", (string src, IConvertTextWidthHandler handler, IOutputPort output) =>
{
    var query = new ConvertTextWidthQuery(
        new UnicodeText(src),
        ConversionDirection.ToHalfWidth
    );
    var result = handler.Handle(query);
    output.Present(result);
});

app.AddCommand("h2f", (string src, IConvertTextWidthHandler handler, IOutputPort output) =>
{
    var query = new ConvertTextWidthQuery(
        new UnicodeText(src),
        ConversionDirection.ToFullWidth
    );
    var result = handler.Handle(query);
    output.Present(result);
});

app.Run();
