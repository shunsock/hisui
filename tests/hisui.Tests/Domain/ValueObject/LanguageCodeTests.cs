using Hisui.Domain.ValueObject;

namespace hisui.Tests.Domain.ValueObject;

[TestFixture]
public class LanguageCodeTests
{
    [Test]
    public void Constructor_ValidLowerCase_CreatesInstance()
    {
        var code = new LanguageCode("en");
        Assert.That(code.Value, Is.EqualTo("en"));
    }

    [Test]
    public void Constructor_UpperCase_NormalizesToLowerCase()
    {
        var code = new LanguageCode("EN");
        Assert.That(code.Value, Is.EqualTo("en"));
    }

    [Test]
    public void Constructor_MixedCase_NormalizesToLowerCase()
    {
        var code = new LanguageCode("Ja");
        Assert.That(code.Value, Is.EqualTo("ja"));
    }

    [Test]
    public void Constructor_Null_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new LanguageCode(null!));
    }

    [Test]
    public void Constructor_EmptyString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new LanguageCode(""));
    }

    [Test]
    public void Constructor_SingleChar_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new LanguageCode("e"));
    }

    [Test]
    public void Constructor_ThreeChars_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new LanguageCode("eng"));
    }

    [Test]
    public void Constructor_ContainsDigit_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new LanguageCode("e1"));
    }

    [Test]
    public void Equals_SameValue_ReturnsTrue()
    {
        var a = new LanguageCode("en");
        var b = new LanguageCode("EN");
        Assert.That(a.Equals(b), Is.True);
        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }

    [Test]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        var a = new LanguageCode("en");
        var b = new LanguageCode("ja");
        Assert.That(a.Equals(b), Is.False);
    }

    [Test]
    public void Equals_Null_ReturnsFalse()
    {
        var a = new LanguageCode("en");
        Assert.That(a.Equals(null), Is.False);
    }

    [Test]
    public void ToString_ReturnsValue()
    {
        var code = new LanguageCode("fr");
        Assert.That(code.ToString(), Is.EqualTo("fr"));
    }
}
