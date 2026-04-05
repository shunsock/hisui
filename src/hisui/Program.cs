using Cocona;
using Hisui.Domain.ValueObject;
using Hisui.Infrastructure.Presenter;
using Hisui.Infrastructure.Translation;
using Hisui.UseCase.Handler;
using Hisui.UseCase.Port;
using Hisui.UseCase.Query;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<IConvertTextWidthHandler, ConvertTextWidthHandler>();
builder.Services.AddSingleton<IOutputPort, ConsoleOutputAdapter>();
builder.Services.AddSingleton<ITranslationService>(_ => TranslationServiceFactory.Create());
builder.Services.AddSingleton<ITranslateTextHandler, TranslateTextHandler>();

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

app.AddCommand("translation", async (
    [Option("to", Description = "Target language code (e.g. en, ja, fr)")] string to,
    [Argument(Description = "Text to translate")] string src,
    ITranslateTextHandler handler,
    IOutputPort output) =>
{
    var query = new TranslateTextQuery(
        new UnicodeText(src),
        new LanguageCode(to)
    );
    var result = await handler.HandleAsync(query);
    output.Present(result);
});

app.Run();
