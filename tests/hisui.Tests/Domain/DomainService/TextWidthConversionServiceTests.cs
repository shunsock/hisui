using Hisui.Domain.DomainService;
using Hisui.Domain.ValueObject;

namespace hisui.Tests.Domain.DomainService;

[TestFixture]
public class TextWidthConversionServiceTests
{
    [TestFixture]
    public class ToHalfWidthTests
    {
        private static UnicodeText ConvertToHalf(string input) =>
            TextWidthConversionService.Convert(new UnicodeText(input), ConversionDirection.ToHalfWidth);

        [Test]
        public void FullWidthAlphanumeric_ConvertsToHalfWidth()
        {
            Assert.That(ConvertToHalf("Ａ").Value, Is.EqualTo("A"));
            Assert.That(ConvertToHalf("ａ").Value, Is.EqualTo("a"));
            Assert.That(ConvertToHalf("Ｚ").Value, Is.EqualTo("Z"));
            Assert.That(ConvertToHalf("ｚ").Value, Is.EqualTo("z"));
            Assert.That(ConvertToHalf("０").Value, Is.EqualTo("0"));
            Assert.That(ConvertToHalf("１２３４５").Value, Is.EqualTo("12345"));
            Assert.That(ConvertToHalf("９").Value, Is.EqualTo("9"));
        }

        [Test]
        public void FullWidthSymbols_ConvertsToHalfWidth()
        {
            Assert.That(ConvertToHalf("！").Value, Is.EqualTo("!"));
            Assert.That(ConvertToHalf("＃").Value, Is.EqualTo("#"));
            Assert.That(ConvertToHalf("＄").Value, Is.EqualTo("$"));
            Assert.That(ConvertToHalf("％").Value, Is.EqualTo("%"));
            Assert.That(ConvertToHalf("（）").Value, Is.EqualTo("()"));
            Assert.That(ConvertToHalf("＝").Value, Is.EqualTo("="));
            Assert.That(ConvertToHalf("＋").Value, Is.EqualTo("+"));
            Assert.That(ConvertToHalf("－").Value, Is.EqualTo("-"));
            Assert.That(ConvertToHalf("＊").Value, Is.EqualTo("*"));
            Assert.That(ConvertToHalf("／").Value, Is.EqualTo("/"));
            Assert.That(ConvertToHalf("＜＞").Value, Is.EqualTo("<>"));
            Assert.That(ConvertToHalf("？").Value, Is.EqualTo("?"));
            Assert.That(ConvertToHalf("＠").Value, Is.EqualTo("@"));
            Assert.That(ConvertToHalf("｛｝").Value, Is.EqualTo("{}"));
            Assert.That(ConvertToHalf("｜").Value, Is.EqualTo("|"));
            Assert.That(ConvertToHalf("～").Value, Is.EqualTo("~"));
        }

        [Test]
        public void FullWidthSpace_ConvertsToHalfWidthSpace()
        {
            Assert.That(ConvertToHalf("　").Value, Is.EqualTo(" "));
            Assert.That(ConvertToHalf("こんにちは　世界").Value, Is.EqualTo("こんにちは 世界"));
        }

        [Test]
        public void HalfWidthCharacters_RemainsUnchanged()
        {
            Assert.That(ConvertToHalf("A").Value, Is.EqualTo("A"));
            Assert.That(ConvertToHalf("a").Value, Is.EqualTo("a"));
            Assert.That(ConvertToHalf("123").Value, Is.EqualTo("123"));
            Assert.That(ConvertToHalf("!@#$%").Value, Is.EqualTo("!@#$%"));
            Assert.That(ConvertToHalf(" ").Value, Is.EqualTo(" "));
        }

        [Test]
        public void JapaneseCharacters_RemainsUnchanged()
        {
            Assert.That(ConvertToHalf("こんにちは").Value, Is.EqualTo("こんにちは"));
            Assert.That(ConvertToHalf("カタカナ").Value, Is.EqualTo("カタカナ"));
            Assert.That(ConvertToHalf("漢字").Value, Is.EqualTo("漢字"));
            Assert.That(ConvertToHalf("ガ").Value, Is.EqualTo("ガ"));
        }

        [Test]
        public void HalfWidthKatakana_RemainsUnchanged()
        {
            Assert.That(ConvertToHalf("ｱｲｳｴｵ").Value, Is.EqualTo("ｱｲｳｴｵ"));
            Assert.That(ConvertToHalf("ｶﾞｷﾞ").Value, Is.EqualTo("ｶﾞｷﾞ"));
            Assert.That(ConvertToHalf("ﾊﾟﾋﾟ").Value, Is.EqualTo("ﾊﾟﾋﾟ"));
        }

        [Test]
        public void MixedContent_ConvertsOnlyFullWidthParts()
        {
            Assert.That(ConvertToHalf("Ａｂｃ１２３！＃＄").Value, Is.EqualTo("Abc123!#$"));
            Assert.That(ConvertToHalf("こんにちはＡＢＣ１２３").Value, Is.EqualTo("こんにちはABC123"));
            Assert.That(ConvertToHalf("Hello　World！").Value, Is.EqualTo("Hello World!"));
        }

        [Test]
        public void EmptyString_ReturnsEmptyString()
        {
            Assert.That(ConvertToHalf("").Value, Is.EqualTo(""));
        }

        [Test]
        public void BoundaryCharacters_ConvertsCorrectly()
        {
            Assert.That(ConvertToHalf("！").Value, Is.EqualTo("!")); // FF01 -> 21
            Assert.That(ConvertToHalf("～").Value, Is.EqualTo("~")); // FF5E -> 7E
        }
    }

    [TestFixture]
    public class ToFullWidthTests
    {
        private static UnicodeText ConvertToFull(string input) =>
            TextWidthConversionService.Convert(new UnicodeText(input), ConversionDirection.ToFullWidth);

        [Test]
        public void HalfWidthAlphanumeric_ConvertsToFullWidth()
        {
            Assert.That(ConvertToFull("A").Value, Is.EqualTo("Ａ"));
            Assert.That(ConvertToFull("a").Value, Is.EqualTo("ａ"));
            Assert.That(ConvertToFull("Z").Value, Is.EqualTo("Ｚ"));
            Assert.That(ConvertToFull("z").Value, Is.EqualTo("ｚ"));
            Assert.That(ConvertToFull("0").Value, Is.EqualTo("０"));
            Assert.That(ConvertToFull("12345").Value, Is.EqualTo("１２３４５"));
            Assert.That(ConvertToFull("9").Value, Is.EqualTo("９"));
        }

        [Test]
        public void HalfWidthSymbols_ConvertsToFullWidth()
        {
            Assert.That(ConvertToFull("!").Value, Is.EqualTo("！"));
            Assert.That(ConvertToFull("#").Value, Is.EqualTo("＃"));
            Assert.That(ConvertToFull("$").Value, Is.EqualTo("＄"));
            Assert.That(ConvertToFull("%").Value, Is.EqualTo("％"));
            Assert.That(ConvertToFull("()").Value, Is.EqualTo("（）"));
            Assert.That(ConvertToFull("=").Value, Is.EqualTo("＝"));
            Assert.That(ConvertToFull("+").Value, Is.EqualTo("＋"));
            Assert.That(ConvertToFull("-").Value, Is.EqualTo("－"));
            Assert.That(ConvertToFull("*").Value, Is.EqualTo("＊"));
            Assert.That(ConvertToFull("/").Value, Is.EqualTo("／"));
            Assert.That(ConvertToFull("<>").Value, Is.EqualTo("＜＞"));
            Assert.That(ConvertToFull("?").Value, Is.EqualTo("？"));
            Assert.That(ConvertToFull("@").Value, Is.EqualTo("＠"));
            Assert.That(ConvertToFull("{}").Value, Is.EqualTo("｛｝"));
            Assert.That(ConvertToFull("|").Value, Is.EqualTo("｜"));
            Assert.That(ConvertToFull("~").Value, Is.EqualTo("～"));
        }

        [Test]
        public void HalfWidthSpace_ConvertsToFullWidthSpace()
        {
            Assert.That(ConvertToFull(" ").Value, Is.EqualTo("　"));
            Assert.That(ConvertToFull("Hello World").Value, Is.EqualTo("Ｈｅｌｌｏ　Ｗｏｒｌｄ"));
        }

        [Test]
        public void HalfWidthKatakana_ConvertsToFullWidthKatakana()
        {
            Assert.That(ConvertToFull("ｱ").Value, Is.EqualTo("ア"));
            Assert.That(ConvertToFull("ｲ").Value, Is.EqualTo("イ"));
            Assert.That(ConvertToFull("ｳ").Value, Is.EqualTo("ウ"));
            Assert.That(ConvertToFull("ｴ").Value, Is.EqualTo("エ"));
            Assert.That(ConvertToFull("ｵ").Value, Is.EqualTo("オ"));
            Assert.That(ConvertToFull("ｶ").Value, Is.EqualTo("カ"));
            Assert.That(ConvertToFull("ｷ").Value, Is.EqualTo("キ"));
            Assert.That(ConvertToFull("ｸ").Value, Is.EqualTo("ク"));
            Assert.That(ConvertToFull("ｹ").Value, Is.EqualTo("ケ"));
            Assert.That(ConvertToFull("ｺ").Value, Is.EqualTo("コ"));
            Assert.That(ConvertToFull("ﾊ").Value, Is.EqualTo("ハ"));
            Assert.That(ConvertToFull("ﾋ").Value, Is.EqualTo("ヒ"));
            Assert.That(ConvertToFull("ﾌ").Value, Is.EqualTo("フ"));
            Assert.That(ConvertToFull("ﾍ").Value, Is.EqualTo("ヘ"));
            Assert.That(ConvertToFull("ﾎ").Value, Is.EqualTo("ホ"));
            Assert.That(ConvertToFull("ﾔ").Value, Is.EqualTo("ヤ"));
            Assert.That(ConvertToFull("ﾕ").Value, Is.EqualTo("ユ"));
            Assert.That(ConvertToFull("ﾖ").Value, Is.EqualTo("ヨ"));
            Assert.That(ConvertToFull("ﾜ").Value, Is.EqualTo("ワ"));
            Assert.That(ConvertToFull("ﾝ").Value, Is.EqualTo("ン"));

            // Small characters
            Assert.That(ConvertToFull("ｧ").Value, Is.EqualTo("ァ"));
            Assert.That(ConvertToFull("ｨ").Value, Is.EqualTo("ィ"));
            Assert.That(ConvertToFull("ｩ").Value, Is.EqualTo("ゥ"));
            Assert.That(ConvertToFull("ｪ").Value, Is.EqualTo("ェ"));
            Assert.That(ConvertToFull("ｫ").Value, Is.EqualTo("ォ"));
            Assert.That(ConvertToFull("ｯ").Value, Is.EqualTo("ッ"));
            Assert.That(ConvertToFull("ｬ").Value, Is.EqualTo("ャ"));
            Assert.That(ConvertToFull("ｭ").Value, Is.EqualTo("ュ"));
            Assert.That(ConvertToFull("ｮ").Value, Is.EqualTo("ョ"));
        }

        [Test]
        public void HalfWidthKatakanaWithDiacritics_ConvertsWithNormalization()
        {
            // 半角カナは1文字ずつNFKC正規化されるため、濁点・半濁点は合成用記号として残る
            // ｶ(FF76)→カ(30AB), ﾞ(FF9E)→゙(3099) のように個別に全角化される
            Assert.That(ConvertToFull("ｶﾞ").Value, Is.EqualTo("\u30AB\u3099")); // カ + 合成用濁点
            Assert.That(ConvertToFull("ｷﾞ").Value, Is.EqualTo("\u30AD\u3099")); // キ + 合成用濁点
            Assert.That(ConvertToFull("ﾊﾟ").Value, Is.EqualTo("\u30CF\u309A")); // ハ + 合成用半濁点
            Assert.That(ConvertToFull("ﾋﾟ").Value, Is.EqualTo("\u30D2\u309A")); // ヒ + 合成用半濁点
        }

        [Test]
        public void FullWidthCharacters_RemainsUnchanged()
        {
            Assert.That(ConvertToFull("Ａ").Value, Is.EqualTo("Ａ"));
            Assert.That(ConvertToFull("１２３").Value, Is.EqualTo("１２３"));
            Assert.That(ConvertToFull("！＠＃").Value, Is.EqualTo("！＠＃"));
            Assert.That(ConvertToFull("　").Value, Is.EqualTo("　"));
        }

        [Test]
        public void JapaneseCharacters_RemainsUnchanged()
        {
            Assert.That(ConvertToFull("こんにちは").Value, Is.EqualTo("こんにちは"));
            Assert.That(ConvertToFull("カタカナ").Value, Is.EqualTo("カタカナ"));
            Assert.That(ConvertToFull("漢字").Value, Is.EqualTo("漢字"));
            Assert.That(ConvertToFull("ガギグゲゴ").Value, Is.EqualTo("ガギグゲゴ"));
        }

        [Test]
        public void MixedContent_ConvertsOnlyHalfWidthParts()
        {
            Assert.That(ConvertToFull("abc123!#$").Value, Is.EqualTo("ａｂｃ１２３！＃＄"));
            Assert.That(ConvertToFull("こんにちはABC123").Value, Is.EqualTo("こんにちはＡＢＣ１２３"));
            Assert.That(ConvertToFull("Hello World!").Value, Is.EqualTo("Ｈｅｌｌｏ　Ｗｏｒｌｄ！"));
        }

        [Test]
        public void EmptyString_ReturnsEmptyString()
        {
            Assert.That(ConvertToFull("").Value, Is.EqualTo(""));
        }

        [Test]
        public void BoundaryCharacters_ConvertsCorrectly()
        {
            Assert.That(ConvertToFull("!").Value, Is.EqualTo("！")); // 21 -> FF01
            Assert.That(ConvertToFull("~").Value, Is.EqualTo("～")); // 7E -> FF5E
            Assert.That(ConvertToFull("｡").Value, Is.EqualTo("。")); // FF61
            Assert.That(ConvertToFull("ﾟ").Value, Is.EqualTo("゚")); // FF9F
        }
    }

    [TestFixture]
    public class IntegrationTests
    {
        private static UnicodeText ConvertToHalf(string input) =>
            TextWidthConversionService.Convert(new UnicodeText(input), ConversionDirection.ToHalfWidth);

        private static UnicodeText ConvertToFull(string input) =>
            TextWidthConversionService.Convert(new UnicodeText(input), ConversionDirection.ToFullWidth);

        [Test]
        public void RoundTrip_HalfToFullToHalf_AlphanumericAndSymbols()
        {
            var original = "Hello World! 123 @#$%^&*()";
            var fullWidth = ConvertToFull(original);
            var backToHalf = ConvertToHalf(fullWidth.Value);

            Assert.That(backToHalf.Value, Is.EqualTo(original));
        }

        [Test]
        public void RoundTrip_FullToHalfToFull_AlphanumericAndSymbols()
        {
            var original = "Ｈｅｌｌｏ　Ｗｏｒｌｄ！　１２３　＠＃＄％＾＆＊（）";
            var halfWidth = ConvertToHalf(original);
            var backToFull = ConvertToFull(halfWidth.Value);

            Assert.That(backToFull.Value, Is.EqualTo(original));
        }

        [Test]
        public void RealWorldExample_MixedJapaneseText()
        {
            var input = "こんにちはＡＢＣ１２３！　半角ｱｲｳエオと全角アイウエオ";
            Assert.That(ConvertToHalf(input).Value, Is.EqualTo("こんにちはABC123! 半角ｱｲｳエオと全角アイウエオ"));
            Assert.That(ConvertToFull(input).Value, Is.EqualTo("こんにちはＡＢＣ１２３！　半角アイウエオと全角アイウエオ"));
        }

        [Test]
        public void EdgeCase_OnlySpaces()
        {
            Assert.That(ConvertToHalf("　　　").Value, Is.EqualTo("   "));
            Assert.That(ConvertToFull("   ").Value, Is.EqualTo("　　　"));
        }

        [Test]
        public void EdgeCase_MixedSpaces()
        {
            var input = "Hello　World こんにちは　世界";
            Assert.That(ConvertToHalf(input).Value, Is.EqualTo("Hello World こんにちは 世界"));
            Assert.That(ConvertToFull(input).Value, Is.EqualTo("Ｈｅｌｌｏ　Ｗｏｒｌｄ　こんにちは　世界"));
        }

        [Test]
        public void Performance_LargeString()
        {
            var largeInput = string.Concat(Enumerable.Repeat("Ａｂｃ１２３！＃＄", 1000));

            var sw = System.Diagnostics.Stopwatch.StartNew();
            var result = ConvertToHalf(largeInput);
            sw.Stop();

            Assert.That(result.Value.Length, Is.EqualTo(largeInput.Length));
            Assert.That(sw.ElapsedMilliseconds, Is.LessThan(100));
        }
    }
}
