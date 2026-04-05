namespace Hisui.Domain.ValueObject;

/// <summary>
/// 変換対象のUnicodeテキストを表現する値オブジェクト。
/// null を排除し、ドメイン内で安全に扱えることを保証する。
/// </summary>
public sealed class UnicodeText : IEquatable<UnicodeText>
{
    public string Value { get; }

    public UnicodeText(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
    }

    public bool Equals(UnicodeText? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => Equals(obj as UnicodeText);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
