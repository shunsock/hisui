using Hisui.Infrastructure.Translation;

namespace hisui.Tests.Infrastructure.Translation;

[TestFixture]
public class AzureOpenAiTranslationAdapterTests
{
    [Test]
    public void Constructor_MissingEndpoint_ThrowsInvalidOperationException()
    {
        var originalEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
        try
        {
            Environment.SetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", null);
            Assert.Throws<InvalidOperationException>(() => new AzureOpenAiTranslationAdapter());
        }
        finally
        {
            Environment.SetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", originalEndpoint);
        }
    }

    [Test]
    public void Constructor_MissingApiKey_ThrowsInvalidOperationException()
    {
        var originalEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
        var originalKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
        try
        {
            Environment.SetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", "https://test.openai.azure.com");
            Environment.SetEnvironmentVariable("AZURE_OPENAI_API_KEY", null);
            Assert.Throws<InvalidOperationException>(() => new AzureOpenAiTranslationAdapter());
        }
        finally
        {
            Environment.SetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", originalEndpoint);
            Environment.SetEnvironmentVariable("AZURE_OPENAI_API_KEY", originalKey);
        }
    }

    [Test]
    public void Constructor_MissingDeployment_ThrowsInvalidOperationException()
    {
        var originalEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
        var originalKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
        var originalDeployment = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT");
        try
        {
            Environment.SetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", "https://test.openai.azure.com");
            Environment.SetEnvironmentVariable("AZURE_OPENAI_API_KEY", "test-key");
            Environment.SetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT", null);
            Assert.Throws<InvalidOperationException>(() => new AzureOpenAiTranslationAdapter());
        }
        finally
        {
            Environment.SetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", originalEndpoint);
            Environment.SetEnvironmentVariable("AZURE_OPENAI_API_KEY", originalKey);
            Environment.SetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT", originalDeployment);
        }
    }
}
