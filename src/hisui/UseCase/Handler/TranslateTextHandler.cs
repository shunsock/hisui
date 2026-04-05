using Hisui.Domain.ValueObject;
using Hisui.UseCase.Port;
using Hisui.UseCase.Query;

namespace Hisui.UseCase.Handler;

/// <summary>
/// テキスト翻訳クエリのハンドラ実装。翻訳サービスポートに委譲する。
/// </summary>
public sealed class TranslateTextHandler : ITranslateTextHandler
{
    private readonly ITranslationService _translationService;

    public TranslateTextHandler(ITranslationService translationService)
    {
        _translationService = translationService;
    }

    public Task<UnicodeText> HandleAsync(ITranslateTextQuery query)
    {
        return _translationService.TranslateAsync(query.Input, query.TargetLanguage);
    }
}
