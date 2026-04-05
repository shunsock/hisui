using System.ClientModel;
using Azure.AI.OpenAI;
using Hisui.Domain.ValueObject;
using Hisui.UseCase.Port;
using OpenAI.Chat;

namespace Hisui.Infrastructure.Translation;

/// <summary>
/// Azure OpenAI を使った翻訳サービスアダプタ。
/// 環境変数 AZURE_OPENAI_ENDPOINT, AZURE_OPENAI_API_KEY, AZURE_OPENAI_DEPLOYMENT を使用する。
/// </summary>
public sealed class AzureOpenAiTranslationAdapter : ITranslationService
{
    private readonly ChatClient _chatClient;

    public AzureOpenAiTranslationAdapter()
    {
        var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT")
            ?? throw new InvalidOperationException(
                "Environment variable AZURE_OPENAI_ENDPOINT is not set.");

        var apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY")
            ?? throw new InvalidOperationException(
                "Environment variable AZURE_OPENAI_API_KEY is not set.");

        var deployment = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT")
            ?? throw new InvalidOperationException(
                "Environment variable AZURE_OPENAI_DEPLOYMENT is not set.");

        var client = new AzureOpenAIClient(new Uri(endpoint), new ApiKeyCredential(apiKey));
        _chatClient = client.GetChatClient(deployment);
    }

    public async Task<UnicodeText> TranslateAsync(UnicodeText input, LanguageCode targetLanguage)
    {
        var systemMessage = ChatMessage.CreateSystemMessage(
            $"You are a translator. Translate the following text to {targetLanguage.Value}. Output only the translated text with no explanation.");
        var userMessage = ChatMessage.CreateUserMessage(input.Value);

        ChatCompletion completion = await _chatClient.CompleteChatAsync([systemMessage, userMessage]);

        var translatedText = completion.Content[0].Text.Trim();
        return new UnicodeText(translatedText);
    }
}
