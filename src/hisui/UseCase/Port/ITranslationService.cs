using Hisui.Domain.ValueObject;

namespace Hisui.UseCase.Port;

/// <summary>
/// 外部翻訳サービスへのポート。Infrastructure 層が実装する。
/// </summary>
public interface ITranslationService
{
    Task<UnicodeText> TranslateAsync(UnicodeText input, LanguageCode targetLanguage);
}
