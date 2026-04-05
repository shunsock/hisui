using Hisui.Domain.ValueObject;

namespace Hisui.Infrastructure.Presenter;

/// <summary>
/// 変換結果の出力先を抽象化するポート。
/// </summary>
public interface IOutputPort
{
    void Present(UnicodeText result);
}
