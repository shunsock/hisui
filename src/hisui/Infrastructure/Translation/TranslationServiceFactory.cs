using Hisui.UseCase.Port;

namespace Hisui.Infrastructure.Translation;

/// <summary>
/// 環境変数に基づいて適切な翻訳サービスアダプタを生成するファクトリ。
/// AZURE_OPENAI_ENDPOINT が設定されていれば Azure OpenAI、なければ OpenAI API を使う。
/// </summary>
public static class TranslationServiceFactory
{
    public static ITranslationService Create()
    {
        var azureEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
        if (!string.IsNullOrEmpty(azureEndpoint))
        {
            return new AzureOpenAiTranslationAdapter();
        }
        return new OpenAiTranslationAdapter();
    }
}
