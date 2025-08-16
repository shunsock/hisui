using hisui.TextWidth;

namespace hisui.Tests.TextWidth;

[TestFixture]
public class TextWidthConverterTests
{
    [TestFixture]
    public class ToHalfWidthTests
    {
        [Test]
        public void ToHalfWidth_FullWidthAlphanumeric_ConvertsToHalfWidth()
        {
            // Full-width letters
            Assert.That(TextWidthConverter.ToHalfWidth("Ａ"), Is.EqualTo("A"));
            Assert.That(TextWidthConverter.ToHalfWidth("ａ"), Is.EqualTo("a"));
            Assert.That(TextWidthConverter.ToHalfWidth("Ｚ"), Is.EqualTo("Z"));
            Assert.That(TextWidthConverter.ToHalfWidth("ｚ"), Is.EqualTo("z"));
            
            // Full-width numbers
            Assert.That(TextWidthConverter.ToHalfWidth("０"), Is.EqualTo("0"));
            Assert.That(TextWidthConverter.ToHalfWidth("１２３４５"), Is.EqualTo("12345"));
            Assert.That(TextWidthConverter.ToHalfWidth("９"), Is.EqualTo("9"));
        }

        [Test]
        public void ToHalfWidth_FullWidthSymbols_ConvertsToHalfWidth()
        {
            Assert.That(TextWidthConverter.ToHalfWidth("！"), Is.EqualTo("!"));
            Assert.That(TextWidthConverter.ToHalfWidth("＃"), Is.EqualTo("#"));
            Assert.That(TextWidthConverter.ToHalfWidth("＄"), Is.EqualTo("$"));
            Assert.That(TextWidthConverter.ToHalfWidth("％"), Is.EqualTo("%"));
            Assert.That(TextWidthConverter.ToHalfWidth("（）"), Is.EqualTo("()"));
            Assert.That(TextWidthConverter.ToHalfWidth("＝"), Is.EqualTo("="));
            Assert.That(TextWidthConverter.ToHalfWidth("＋"), Is.EqualTo("+"));
            Assert.That(TextWidthConverter.ToHalfWidth("－"), Is.EqualTo("-"));
            Assert.That(TextWidthConverter.ToHalfWidth("＊"), Is.EqualTo("*"));
            Assert.That(TextWidthConverter.ToHalfWidth("／"), Is.EqualTo("/"));
            Assert.That(TextWidthConverter.ToHalfWidth("＜＞"), Is.EqualTo("<>"));
            Assert.That(TextWidthConverter.ToHalfWidth("？"), Is.EqualTo("?"));
            Assert.That(TextWidthConverter.ToHalfWidth("＠"), Is.EqualTo("@"));
            Assert.That(TextWidthConverter.ToHalfWidth("｛｝"), Is.EqualTo("{}"));
            Assert.That(TextWidthConverter.ToHalfWidth("｜"), Is.EqualTo("|"));
            Assert.That(TextWidthConverter.ToHalfWidth("～"), Is.EqualTo("~"));
        }

        [Test]
        public void ToHalfWidth_FullWidthSpace_ConvertsToHalfWidthSpace()
        {
            Assert.That(TextWidthConverter.ToHalfWidth("　"), Is.EqualTo(" "));
            Assert.That(TextWidthConverter.ToHalfWidth("こんにちは　世界"), Is.EqualTo("こんにちは 世界"));
        }

        [Test]
        public void ToHalfWidth_HalfWidthCharacters_RemainsUnchanged()
        {
            Assert.That(TextWidthConverter.ToHalfWidth("A"), Is.EqualTo("A"));
            Assert.That(TextWidthConverter.ToHalfWidth("a"), Is.EqualTo("a"));
            Assert.That(TextWidthConverter.ToHalfWidth("123"), Is.EqualTo("123"));
            Assert.That(TextWidthConverter.ToHalfWidth("!@#$%"), Is.EqualTo("!@#$%"));
            Assert.That(TextWidthConverter.ToHalfWidth(" "), Is.EqualTo(" "));
        }

        [Test]
        public void ToHalfWidth_JapaneseCharacters_RemainsUnchanged()
        {
            Assert.That(TextWidthConverter.ToHalfWidth("こんにちは"), Is.EqualTo("こんにちは"));
            Assert.That(TextWidthConverter.ToHalfWidth("カタカナ"), Is.EqualTo("カタカナ"));
            Assert.That(TextWidthConverter.ToHalfWidth("漢字"), Is.EqualTo("漢字"));
            Assert.That(TextWidthConverter.ToHalfWidth("ガ"), Is.EqualTo("ガ"));
        }

        [Test]
        public void ToHalfWidth_HalfWidthKatakana_RemainsUnchanged()
        {
            Assert.That(TextWidthConverter.ToHalfWidth("ｱｲｳｴｵ"), Is.EqualTo("ｱｲｳｴｵ"));
            Assert.That(TextWidthConverter.ToHalfWidth("ｶﾞｷﾞ"), Is.EqualTo("ｶﾞｷﾞ"));
            Assert.That(TextWidthConverter.ToHalfWidth("ﾊﾟﾋﾟ"), Is.EqualTo("ﾊﾟﾋﾟ"));
        }

        [Test]
        public void ToHalfWidth_MixedContent_ConvertsOnlyFullWidthParts()
        {
            Assert.That(TextWidthConverter.ToHalfWidth("Ａｂｃ１２３！＃＄"), Is.EqualTo("Abc123!#$"));
            Assert.That(TextWidthConverter.ToHalfWidth("こんにちはＡＢＣ１２３"), Is.EqualTo("こんにちはABC123"));
            Assert.That(TextWidthConverter.ToHalfWidth("Hello　World！"), Is.EqualTo("Hello World!"));
        }

        [Test]
        public void ToHalfWidth_EmptyString_ReturnsEmptyString()
        {
            Assert.That(TextWidthConverter.ToHalfWidth(""), Is.EqualTo(""));
        }

        [Test]
        public void ToHalfWidth_NullString_ThrowsNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => TextWidthConverter.ToHalfWidth(null!));
        }

        [Test]
        public void ToHalfWidth_BoundaryCharacters_ConvertsCorrectly()
        {
            // Test boundary characters of the conversion range FF01-FF5E
            Assert.That(TextWidthConverter.ToHalfWidth("！"), Is.EqualTo("!")); // FF01 -> 21
            Assert.That(TextWidthConverter.ToHalfWidth("～"), Is.EqualTo("~")); // FF5E -> 7E
        }
    }

    [TestFixture]
    public class ToFullWidthTests
    {
        [Test]
        public void ToFullWidth_HalfWidthAlphanumeric_ConvertsToFullWidth()
        {
            // Half-width letters
            Assert.That(TextWidthConverter.ToFullWidth("A"), Is.EqualTo("Ａ"));
            Assert.That(TextWidthConverter.ToFullWidth("a"), Is.EqualTo("ａ"));
            Assert.That(TextWidthConverter.ToFullWidth("Z"), Is.EqualTo("Ｚ"));
            Assert.That(TextWidthConverter.ToFullWidth("z"), Is.EqualTo("ｚ"));
            
            // Half-width numbers
            Assert.That(TextWidthConverter.ToFullWidth("0"), Is.EqualTo("０"));
            Assert.That(TextWidthConverter.ToFullWidth("12345"), Is.EqualTo("１２３４５"));
            Assert.That(TextWidthConverter.ToFullWidth("9"), Is.EqualTo("９"));
        }

        [Test]
        public void ToFullWidth_HalfWidthSymbols_ConvertsToFullWidth()
        {
            Assert.That(TextWidthConverter.ToFullWidth("!"), Is.EqualTo("！"));
            Assert.That(TextWidthConverter.ToFullWidth("#"), Is.EqualTo("＃"));
            Assert.That(TextWidthConverter.ToFullWidth("$"), Is.EqualTo("＄"));
            Assert.That(TextWidthConverter.ToFullWidth("%"), Is.EqualTo("％"));
            Assert.That(TextWidthConverter.ToFullWidth("()"), Is.EqualTo("（）"));
            Assert.That(TextWidthConverter.ToFullWidth("="), Is.EqualTo("＝"));
            Assert.That(TextWidthConverter.ToFullWidth("+"), Is.EqualTo("＋"));
            Assert.That(TextWidthConverter.ToFullWidth("-"), Is.EqualTo("－"));
            Assert.That(TextWidthConverter.ToFullWidth("*"), Is.EqualTo("＊"));
            Assert.That(TextWidthConverter.ToFullWidth("/"), Is.EqualTo("／"));
            Assert.That(TextWidthConverter.ToFullWidth("<>"), Is.EqualTo("＜＞"));
            Assert.That(TextWidthConverter.ToFullWidth("?"), Is.EqualTo("？"));
            Assert.That(TextWidthConverter.ToFullWidth("@"), Is.EqualTo("＠"));
            Assert.That(TextWidthConverter.ToFullWidth("{}"), Is.EqualTo("｛｝"));
            Assert.That(TextWidthConverter.ToFullWidth("|"), Is.EqualTo("｜"));
            Assert.That(TextWidthConverter.ToFullWidth("~"), Is.EqualTo("～"));
        }

        [Test]
        public void ToFullWidth_HalfWidthSpace_ConvertsToFullWidthSpace()
        {
            Assert.That(TextWidthConverter.ToFullWidth(" "), Is.EqualTo("　"));
            Assert.That(TextWidthConverter.ToFullWidth("Hello World"), Is.EqualTo("Ｈｅｌｌｏ　Ｗｏｒｌｄ"));
        }

        [Test]
        public void ToFullWidth_HalfWidthKatakana_ConvertsToFullWidthKatakana()
        {
            // Basic katakana
            Assert.That(TextWidthConverter.ToFullWidth("ｱ"), Is.EqualTo("ア"));
            Assert.That(TextWidthConverter.ToFullWidth("ｲ"), Is.EqualTo("イ"));
            Assert.That(TextWidthConverter.ToFullWidth("ｳ"), Is.EqualTo("ウ"));
            Assert.That(TextWidthConverter.ToFullWidth("ｴ"), Is.EqualTo("エ"));
            Assert.That(TextWidthConverter.ToFullWidth("ｵ"), Is.EqualTo("オ"));
            
            Assert.That(TextWidthConverter.ToFullWidth("ｶ"), Is.EqualTo("カ"));
            Assert.That(TextWidthConverter.ToFullWidth("ｷ"), Is.EqualTo("キ"));
            Assert.That(TextWidthConverter.ToFullWidth("ｸ"), Is.EqualTo("ク"));
            Assert.That(TextWidthConverter.ToFullWidth("ｹ"), Is.EqualTo("ケ"));
            Assert.That(TextWidthConverter.ToFullWidth("ｺ"), Is.EqualTo("コ"));
            
            Assert.That(TextWidthConverter.ToFullWidth("ﾊ"), Is.EqualTo("ハ"));
            Assert.That(TextWidthConverter.ToFullWidth("ﾋ"), Is.EqualTo("ヒ"));
            Assert.That(TextWidthConverter.ToFullWidth("ﾌ"), Is.EqualTo("フ"));
            Assert.That(TextWidthConverter.ToFullWidth("ﾍ"), Is.EqualTo("ヘ"));
            Assert.That(TextWidthConverter.ToFullWidth("ﾎ"), Is.EqualTo("ホ"));
            
            Assert.That(TextWidthConverter.ToFullWidth("ﾔ"), Is.EqualTo("ヤ"));
            Assert.That(TextWidthConverter.ToFullWidth("ﾕ"), Is.EqualTo("ユ"));
            Assert.That(TextWidthConverter.ToFullWidth("ﾖ"), Is.EqualTo("ヨ"));
            
            Assert.That(TextWidthConverter.ToFullWidth("ﾜ"), Is.EqualTo("ワ"));
            Assert.That(TextWidthConverter.ToFullWidth("ﾝ"), Is.EqualTo("ン"));
            
            // Small characters
            Assert.That(TextWidthConverter.ToFullWidth("ｧ"), Is.EqualTo("ァ"));
            Assert.That(TextWidthConverter.ToFullWidth("ｨ"), Is.EqualTo("ィ"));
            Assert.That(TextWidthConverter.ToFullWidth("ｩ"), Is.EqualTo("ゥ"));
            Assert.That(TextWidthConverter.ToFullWidth("ｪ"), Is.EqualTo("ェ"));
            Assert.That(TextWidthConverter.ToFullWidth("ｫ"), Is.EqualTo("ォ"));
            Assert.That(TextWidthConverter.ToFullWidth("ｯ"), Is.EqualTo("ッ"));
            Assert.That(TextWidthConverter.ToFullWidth("ｬ"), Is.EqualTo("ャ"));
            Assert.That(TextWidthConverter.ToFullWidth("ｭ"), Is.EqualTo("ュ"));
            Assert.That(TextWidthConverter.ToFullWidth("ｮ"), Is.EqualTo("ョ"));
        }

        [Test]
        public void ToFullWidth_HalfWidthKatakanaWithDiacritics_ConvertsWithNormalization()
        {
            // Note: These tests depend on NFKC normalization behavior
            // The half-width katakana + dakuten/handakuten should combine into single full-width characters
            
            // Dakuten (voiced marks)
            Assert.That(TextWidthConverter.ToFullWidth("ｶﾞ"), Is.EqualTo("ガ")); // May normalize to ガ depending on NFKC
            Assert.That(TextWidthConverter.ToFullWidth("ｷﾞ"), Is.EqualTo("ギ")); // May normalize to ギ depending on NFKC
            
            // Handakuten (semi-voiced marks)
            Assert.That(TextWidthConverter.ToFullWidth("ﾊﾟ"), Is.EqualTo("パ")); // May normalize to パ depending on NFKC
            Assert.That(TextWidthConverter.ToFullWidth("ﾋﾟ"), Is.EqualTo("ピ")); // May normalize to ピ depending on NFKC
        }

        [Test]
        public void ToFullWidth_FullWidthCharacters_RemainsUnchanged()
        {
            Assert.That(TextWidthConverter.ToFullWidth("Ａ"), Is.EqualTo("Ａ"));
            Assert.That(TextWidthConverter.ToFullWidth("１２３"), Is.EqualTo("１２３"));
            Assert.That(TextWidthConverter.ToFullWidth("！＠＃"), Is.EqualTo("！＠＃"));
            Assert.That(TextWidthConverter.ToFullWidth("　"), Is.EqualTo("　"));
        }

        [Test]
        public void ToFullWidth_JapaneseCharacters_RemainsUnchanged()
        {
            Assert.That(TextWidthConverter.ToFullWidth("こんにちは"), Is.EqualTo("こんにちは"));
            Assert.That(TextWidthConverter.ToFullWidth("カタカナ"), Is.EqualTo("カタカナ"));
            Assert.That(TextWidthConverter.ToFullWidth("漢字"), Is.EqualTo("漢字"));
            Assert.That(TextWidthConverter.ToFullWidth("ガギグゲゴ"), Is.EqualTo("ガギグゲゴ"));
        }

        [Test]
        public void ToFullWidth_MixedContent_ConvertsOnlyHalfWidthParts()
        {
            Assert.That(TextWidthConverter.ToFullWidth("abc123!#$"), Is.EqualTo("ａｂｃ１２３！＃＄"));
            Assert.That(TextWidthConverter.ToFullWidth("こんにちはABC123"), Is.EqualTo("こんにちはＡＢＣ１２３"));
            Assert.That(TextWidthConverter.ToFullWidth("Hello World!"), Is.EqualTo("Ｈｅｌｌｏ　Ｗｏｒｌｄ！"));
        }

        [Test]
        public void ToFullWidth_EmptyString_ReturnsEmptyString()
        {
            Assert.That(TextWidthConverter.ToFullWidth(""), Is.EqualTo(""));
        }

        [Test]
        public void ToFullWidth_NullString_ThrowsNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => TextWidthConverter.ToFullWidth(null!));
        }

        [Test]
        public void ToFullWidth_BoundaryCharacters_ConvertsCorrectly()
        {
            // Test boundary characters of the conversion range 21-7E
            Assert.That(TextWidthConverter.ToFullWidth("!"), Is.EqualTo("！")); // 21 -> FF01
            Assert.That(TextWidthConverter.ToFullWidth("~"), Is.EqualTo("～")); // 7E -> FF5E
            
            // Test boundary characters of half-width katakana range FF61-FF9F
            Assert.That(TextWidthConverter.ToFullWidth("｡"), Is.EqualTo("。")); // FF61
            Assert.That(TextWidthConverter.ToFullWidth("ﾟ"), Is.EqualTo("゚")); // FF9F
        }
    }

    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public void RoundTrip_HalfToFullToHalf_AlphanumericAndSymbols()
        {
            var original = "Hello World! 123 @#$%^&*()";
            var fullWidth = TextWidthConverter.ToFullWidth(original);
            var backToHalf = TextWidthConverter.ToHalfWidth(fullWidth);
            
            Assert.That(backToHalf, Is.EqualTo(original));
        }

        [Test]
        public void RoundTrip_FullToHalfToFull_AlphanumericAndSymbols()
        {
            var original = "Ｈｅｌｌｏ　Ｗｏｒｌｄ！　１２３　＠＃＄％＾＆＊（）";
            var halfWidth = TextWidthConverter.ToHalfWidth(original);
            var backToFull = TextWidthConverter.ToFullWidth(halfWidth);
            
            Assert.That(backToFull, Is.EqualTo(original));
        }

        [Test]
        public void RealWorldExample_MixedJapaneseText()
        {
            var input = "こんにちはＡＢＣ１２３！　半角ｱｲｳエオと全角アイウエオ";
            var expectedHalf = "こんにちはABC123! 半角ｱｲｳエオと全角アイウエオ";
            var expectedFull = "こんにちはＡＢＣ１２３！　半角アイウエオと全角アイウエオ";
            
            Assert.That(TextWidthConverter.ToHalfWidth(input), Is.EqualTo(expectedHalf));
            Assert.That(TextWidthConverter.ToFullWidth(input), Is.EqualTo(expectedFull));
        }

        [Test]
        public void EdgeCase_OnlySpaces()
        {
            Assert.That(TextWidthConverter.ToHalfWidth("　　　"), Is.EqualTo("   "));
            Assert.That(TextWidthConverter.ToFullWidth("   "), Is.EqualTo("　　　"));
        }

        [Test]
        public void EdgeCase_MixedSpaces()
        {
            var input = "Hello　World こんにちは　世界";
            var expectedHalf = "Hello World こんにちは 世界";
            var expectedFull = "Ｈｅｌｌｏ　Ｗｏｒｌｄ　こんにちは　世界";
            
            Assert.That(TextWidthConverter.ToHalfWidth(input), Is.EqualTo(expectedHalf));
            Assert.That(TextWidthConverter.ToFullWidth(input), Is.EqualTo(expectedFull));
        }

        [Test]
        public void Performance_LargeString()
        {
            var largeInput = string.Concat(Enumerable.Repeat("Ａｂｃ１２３！＃＄", 1000));
            
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var result = TextWidthConverter.ToHalfWidth(largeInput);
            sw.Stop();
            
            Assert.That(result.Length, Is.EqualTo(largeInput.Length));
            Assert.That(sw.ElapsedMilliseconds, Is.LessThan(100)); // Should complete quickly
        }
    }
}