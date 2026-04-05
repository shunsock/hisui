using Hisui.Domain.ValueObject;

namespace Hisui.UseCase.Query;

/// <summary>
/// テキスト幅変換のクエリ。入力テキストと変換方向を保持する。
/// </summary>
public sealed class ConvertTextWidthQuery : IConvertTextWidthQuery
{
    public UnicodeText Input { get; }
    public ConversionDirection Direction { get; }

    public ConvertTextWidthQuery(UnicodeText input, ConversionDirection direction)
    {
        Input = input;
        Direction = direction;
    }
}
