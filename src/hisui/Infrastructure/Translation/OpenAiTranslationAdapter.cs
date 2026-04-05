using System.ClientModel;
using Hisui.Domain.ValueObject;
using Hisui.UseCase.Port;
using OpenAI;
using OpenAI.Chat;

namespace Hisui.Infrastructure.Translation;

/// <summary>
/// OpenAI API (api.openai.com) を使った翻訳サービスアダプタ。
/// 環境変数 OPENAI_API_KEY（必須）と OPENAI_MODEL（任意）を使用する。
/// </summary>
public sealed class OpenAiTranslationAdapter : ITranslationService
{
    private readonly ChatClient _chatClient;

    public OpenAiTranslationAdapter()
    {
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            ?? throw new InvalidOperationException(
                "Environment variable OPENAI_API_KEY is not set.");

        var model = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini";

        var client = new OpenAIClient(new ApiKeyCredential(apiKey));
        _chatClient = client.GetChatClient(model);
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
