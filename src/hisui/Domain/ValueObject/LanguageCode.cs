using System.Text.RegularExpressions;

namespace Hisui.Domain.ValueObject;

/// <summary>
/// 翻訳先の言語を表現する値オブジェクト。
/// ISO 639-1 の2文字コード（例: "en", "ja"）を保持し、小文字に正規化する。
/// </summary>
public sealed partial class LanguageCode : IEquatable<LanguageCode>
{
    public string Value { get; }

    public LanguageCode(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (!Iso639Pattern().IsMatch(value))
        {
            throw new ArgumentException(
                $"Language code must be a 2-letter ISO 639-1 code, but got: \"{value}\"",
                nameof(value)
            );
        }
        Value = value.ToLowerInvariant();
    }

    public bool Equals(LanguageCode? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => Equals(obj as LanguageCode);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    [GeneratedRegex(@"^[a-zA-Z]{2}$")]
    private static partial Regex Iso639Pattern();
}
