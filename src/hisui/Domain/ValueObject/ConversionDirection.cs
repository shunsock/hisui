namespace Hisui.Domain.ValueObject;

/// <summary>
/// テキスト幅の変換方向を示す。
/// </summary>
public enum ConversionDirection
{
    /// <summary>全角 → 半角</summary>
    ToHalfWidth,

    /// <summary>半角 → 全角</summary>
    ToFullWidth,
}
