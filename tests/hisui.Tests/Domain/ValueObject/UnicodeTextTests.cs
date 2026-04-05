using Hisui.Domain.ValueObject;

namespace hisui.Tests.Domain.ValueObject;

[TestFixture]
public class UnicodeTextTests
{
    [Test]
    public void Constructor_ValidString_CreatesInstance()
    {
        var text = new UnicodeText("hello");
        Assert.That(text.Value, Is.EqualTo("hello"));
    }

    [Test]
    public void Constructor_EmptyString_CreatesInstance()
    {
        var text = new UnicodeText("");
        Assert.That(text.Value, Is.EqualTo(""));
    }

    [Test]
    public void Constructor_Null_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new UnicodeText(null!));
    }

    [Test]
    public void Equals_SameValue_ReturnsTrue()
    {
        var a = new UnicodeText("hello");
        var b = new UnicodeText("hello");
        Assert.That(a.Equals(b), Is.True);
        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }

    [Test]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        var a = new UnicodeText("hello");
        var b = new UnicodeText("world");
        Assert.That(a.Equals(b), Is.False);
    }

    [Test]
    public void Equals_Null_ReturnsFalse()
    {
        var a = new UnicodeText("hello");
        Assert.That(a.Equals(null), Is.False);
    }

    [Test]
    public void ToString_ReturnsValue()
    {
        var text = new UnicodeText("テスト");
        Assert.That(text.ToString(), Is.EqualTo("テスト"));
    }
}
