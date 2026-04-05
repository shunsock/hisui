using Hisui.Domain.ValueObject;

namespace Hisui.UseCase.Query;

/// <summary>
/// テキスト翻訳クエリの契約。
/// </summary>
public interface ITranslateTextQuery
{
    UnicodeText Input { get; }
    LanguageCode TargetLanguage { get; }
}
