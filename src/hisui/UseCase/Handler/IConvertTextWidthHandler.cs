using Hisui.Domain.ValueObject;
using Hisui.UseCase.Query;

namespace Hisui.UseCase.Handler;

/// <summary>
/// テキスト幅変換クエリを処理するハンドラの契約。
/// </summary>
public interface IConvertTextWidthHandler
{
    UnicodeText Handle(IConvertTextWidthQuery query);
}
