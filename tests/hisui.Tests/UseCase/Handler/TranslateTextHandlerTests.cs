using Hisui.Domain.ValueObject;
using Hisui.UseCase.Handler;
using Hisui.UseCase.Port;
using Hisui.UseCase.Query;

namespace hisui.Tests.UseCase.Handler;

[TestFixture]
public class TranslateTextHandlerTests
{
    private sealed class StubTranslationService : ITranslationService
    {
        public UnicodeText? ReceivedInput { get; private set; }
        public LanguageCode? ReceivedTargetLanguage { get; private set; }
        public UnicodeText Result { get; set; } = new("translated");

        public Task<UnicodeText> TranslateAsync(UnicodeText input, LanguageCode targetLanguage)
        {
            ReceivedInput = input;
            ReceivedTargetLanguage = targetLanguage;
            return Task.FromResult(Result);
        }
    }

    [Test]
    public async Task HandleAsync_DelegatesToTranslationService()
    {
        var stub = new StubTranslationService { Result = new UnicodeText("Hello") };
        var handler = new TranslateTextHandler(stub);
        var query = new TranslateTextQuery(new UnicodeText("こんにちは"), new LanguageCode("en"));

        var result = await handler.HandleAsync(query);

        Assert.That(result.Value, Is.EqualTo("Hello"));
        Assert.That(stub.ReceivedInput!.Value, Is.EqualTo("こんにちは"));
        Assert.That(stub.ReceivedTargetLanguage!.Value, Is.EqualTo("en"));
    }

    [Test]
    public async Task HandleAsync_ReturnsServiceResultUnmodified()
    {
        var expected = new UnicodeText("Bonjour le monde");
        var stub = new StubTranslationService { Result = expected };
        var handler = new TranslateTextHandler(stub);
        var query = new TranslateTextQuery(new UnicodeText("Hello world"), new LanguageCode("fr"));

        var result = await handler.HandleAsync(query);

        Assert.That(result, Is.EqualTo(expected));
    }
}
