using Hisui.Infrastructure.Translation;

namespace hisui.Tests.Infrastructure.Translation;

[TestFixture]
public class OpenAiTranslationAdapterTests
{
    [Test]
    public void Constructor_MissingApiKey_ThrowsInvalidOperationException()
    {
        // OPENAI_API_KEY が未設定の状態でインスタンス生成を試みる
        var original = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        try
        {
            Environment.SetEnvironmentVariable("OPENAI_API_KEY", null);
            Assert.Throws<InvalidOperationException>(() => new OpenAiTranslationAdapter());
        }
        finally
        {
            Environment.SetEnvironmentVariable("OPENAI_API_KEY", original);
        }
    }
}
