using Hisui.Domain.ValueObject;

namespace Hisui.UseCase.Query;

/// <summary>
/// テキスト翻訳のクエリ。入力テキストと翻訳先言語を保持する。
/// </summary>
public sealed class TranslateTextQuery : ITranslateTextQuery
{
    public UnicodeText Input { get; }
    public LanguageCode TargetLanguage { get; }

    public TranslateTextQuery(UnicodeText input, LanguageCode targetLanguage)
    {
        Input = input;
        TargetLanguage = targetLanguage;
    }
}
