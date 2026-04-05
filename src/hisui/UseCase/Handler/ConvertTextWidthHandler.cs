using Hisui.Domain.DomainService;
using Hisui.Domain.ValueObject;
using Hisui.UseCase.Query;

namespace Hisui.UseCase.Handler;

/// <summary>
/// テキスト幅変換クエリのハンドラ実装。
/// </summary>
public sealed class ConvertTextWidthHandler : IConvertTextWidthHandler
{
    public UnicodeText Handle(IConvertTextWidthQuery query)
    {
        return TextWidthConversionService.Convert(query.Input, query.Direction);
    }
}
