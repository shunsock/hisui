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
                        // それ以外はそのまま（※カナはここでは変換しない）
                        sb.Append(ch);
                        break;
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
            var stringBuilder = new StringBuilder(input.Length);
            foreach (var ch in input)
            {
                int code = ch;

                switch (code)
                {
                    // 半角英数字・基本記号（21〜7E）→ 全角（差分 FEE0）
                    case >= 0x21 and <= 0x7E:
                        stringBuilder.Append((char)(code + 0xFEE0));
                        break;
                    // 半角スペース U+0020 → 全角スペース U+3000
                    case 0x20:
                        stringBuilder.Append('\u3000');
                        break;
                    // 半角カナ FF61〜FF9F → NFKCで全角カナに合成
                    case >= 0xFF61 and <= 0xFF9F:
                        // 単独文字でNFKC正規化（濁点/半濁点の合成も行われる）
                        var s = new string(ch, 1);
                        stringBuilder.Append(s.Normalize(NormalizationForm.FormKC));
                        break;
                    default:
                        stringBuilder.Append(ch);
                        break;
                }
            }
            return stringBuilder.ToString();
        }
    }
}
