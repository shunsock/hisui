using Hisui.Domain.ValueObject;

namespace Hisui.Infrastructure.Presenter;

/// <summary>
/// 変換結果をコンソール（標準出力）に出力するアダプタ。
/// </summary>
public sealed class ConsoleOutputAdapter : IOutputPort
{
    public void Present(UnicodeText result)
    {
        Console.WriteLine(result.Value);
    }
}
