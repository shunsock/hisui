using Hisui.Domain.ValueObject;

namespace Hisui.UseCase.Query;

/// <summary>
/// テキスト幅変換クエリの契約。
/// </summary>
public interface IConvertTextWidthQuery
{
    UnicodeText Input { get; }
    ConversionDirection Direction { get; }
}
