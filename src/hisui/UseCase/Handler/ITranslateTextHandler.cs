using Hisui.Domain.ValueObject;
using Hisui.UseCase.Query;

namespace Hisui.UseCase.Handler;

/// <summary>
/// テキスト翻訳クエリを処理するハンドラの契約。
/// </summary>
public interface ITranslateTextHandler
{
    Task<UnicodeText> HandleAsync(ITranslateTextQuery query);
}
