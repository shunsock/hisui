using System.Text;
using Hisui.Domain.ValueObject;

namespace Hisui.Domain.DomainService;

/// <summary>
/// Unicode テキストの全角・半角変換を行うドメインサービス。
/// </summary>
public static class TextWidthConversionService
{
    /// <summary>
    /// 指定された方向にテキスト幅を変換する。
    /// </summary>
    public static UnicodeText Convert(UnicodeText input, ConversionDirection direction)
    {
        var converted = direction switch
        {
            ConversionDirection.ToHalfWidth => ToHalfWidth(input.Value),
            ConversionDirection.ToFullWidth => ToFullWidth(input.Value),
            _ => throw new ArgumentOutOfRangeException(nameof(direction)),
        };
        return new UnicodeText(converted);
    }

    /// <summary>
    /// 全角→半角（英数字/基本記号/スペース）。カナは変更しない。
    /// </summary>
    private static string ToHalfWidth(string input)
    {
        var sb = new StringBuilder(input.Length);
        foreach (var ch in input)
        {
            int code = ch;

            switch (code)
            {
                // 全角英数字・基本記号（FF01〜FF5E）→ 半角（差分 FEE0）
                case >= 0xFF01 and <= 0xFF5E:
                    sb.Append((char)(code - 0xFEE0));
                    break;
                // 全角スペース U+3000 → 半角スペース U+0020
                case 0x3000:
                    sb.Append(' ');
                    break;
                default:
                    sb.Append(ch);
                    break;
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// 半角→全角（英数字/基本記号/スペース/半角カナ→全角カナ）。
    /// 半角カナはNFKCで合成して全角カナ化する。
    /// </summary>
    private static string ToFullWidth(string input)
    {
        var sb = new StringBuilder(input.Length);
        foreach (var ch in input)
        {
            int code = ch;

            switch (code)
            {
                // 半角英数字・基本記号（21〜7E）→ 全角（差分 FEE0）
                case >= 0x21 and <= 0x7E:
                    sb.Append((char)(code + 0xFEE0));
                    break;
                // 半角スペース U+0020 → 全角スペース U+3000
                case 0x20:
                    sb.Append('\u3000');
                    break;
                // 半角カナ FF61〜FF9F → NFKCで全角カナに合成
                case >= 0xFF61 and <= 0xFF9F:
                    var s = new string(ch, 1);
                    sb.Append(s.Normalize(NormalizationForm.FormKC));
                    break;
                default:
                    sb.Append(ch);
                    break;
            }
        }
        return sb.ToString();
    }
}
