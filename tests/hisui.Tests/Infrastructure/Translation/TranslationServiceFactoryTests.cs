using Hisui.Infrastructure.Translation;

namespace hisui.Tests.Infrastructure.Translation;

[TestFixture]
public class TranslationServiceFactoryTests
{
    [Test]
    public void Create_WithAzureEndpoint_ReturnsAzureAdapter()
    {
        var originalEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
        var originalKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
        var originalDeployment = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT");
        try
        {
            Environment.SetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", "https://test.openai.azure.com");
            Environment.SetEnvironmentVariable("AZURE_OPENAI_API_KEY", "test-key");
            Environment.SetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT", "gpt-4o");

            var service = TranslationServiceFactory.Create();

            Assert.That(service, Is.InstanceOf<AzureOpenAiTranslationAdapter>());
        }
        finally
        {
            Environment.SetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", originalEndpoint);
            Environment.SetEnvironmentVariable("AZURE_OPENAI_API_KEY", originalKey);
            Environment.SetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT", originalDeployment);
        }
    }

    [Test]
    public void Create_WithoutAzureEndpoint_ReturnsOpenAiAdapter()
    {
        var originalEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
        var originalKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        try
        {
            Environment.SetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", null);
            Environment.SetEnvironmentVariable("OPENAI_API_KEY", "sk-test-key");

            var service = TranslationServiceFactory.Create();

            Assert.That(service, Is.InstanceOf<OpenAiTranslationAdapter>());
        }
        finally
        {
            Environment.SetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", originalEndpoint);
            Environment.SetEnvironmentVariable("OPENAI_API_KEY", originalKey);
        }
    }
}
