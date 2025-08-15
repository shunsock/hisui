using System;
using System.Text;

namespace hisui.TextWidth
{

/// <summary>
/// 全角・半角の相互変換ユーティリティ。
/// 対象:
/// - 英数字/基本記号: FF01–FF5E ⇔ 21–7E（差分FEE0）
/// - スペース: U+3000 ⇔ U+0020
/// - 半角カナ: FF61–FF9F → 全角カナ（NFKC）
/// 逆方向のカナ（全角→半角）は対象外。
/// </summary>
public static class TextWidthConverter
{
    /// <summary>
    /// 全角→半角（英数字/基本記号/スペース）。カナは変更しません。
    /// </summary>
    public static string ToHalfWidth(string input)
    {
        var sb = new StringBuilder(input.Length);
        foreach (var ch in input)
        {
            int code = ch;

            // 全角英数字・基本記号（FF01〜FF5E）→ 半角（差分 FEE0）
            if (code >= 0xFF01 && code <= 0xFF5E)
            {
                sb.Append((char)(code - 0xFEE0));
            }
            // 全角スペース U+3000 → 半角スペース U+0020
            else if (code == 0x3000)
            {
                sb.Append(' ');
            }
            else
            {
                // それ以外はそのまま（※カナはここでは変換しない）
                sb.Append(ch);
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// 半角→全角（英数字/基本記号/スペース/半角カナ→全角カナ）。
    /// 半角カナはNFKCで合成して全角カナ化します。
    /// </summary>
    public static string ToFullWidth(string input)
    {
        var sb = new StringBuilder(input.Length);
        foreach (var ch in input)
        {
            int code = ch;

            // 半角英数字・基本記号（21〜7E）→ 全角（差分 FEE0）
            if (code >= 0x21 && code <= 0x7E)
            {
                sb.Append((char)(code + 0xFEE0));
            }
            // 半角スペース U+0020 → 全角スペース U+3000
            else if (code == 0x20)
            {
                sb.Append('\u3000');
            }
            // 半角カナ FF61〜FF9F → NFKCで全角カナに合成
            else if (code >= 0xFF61 && code <= 0xFF9F)
            {
                // 単独文字でNFKC正規化（濁点/半濁点の合成も行われる）
                string s = new string(ch, 1);
                sb.Append(s.Normalize(NormalizationForm.FormKC));
            }
            else
            {
                sb.Append(ch);
            }
        }
        return sb.ToString();
    }
  }
}
