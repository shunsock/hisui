using Hisui.Domain.ValueObject;
using Hisui.Infrastructure.Presenter;

namespace hisui.Tests.Infrastructure.Presenter;

[TestFixture]
public class ConsoleOutputAdapterTests
{
    [Test]
    public void Present_WritesToConsoleOut()
    {
        var adapter = new ConsoleOutputAdapter();
        var text = new UnicodeText("テスト出力");

        using var sw = new StringWriter();
        Console.SetOut(sw);

        adapter.Present(text);

        var standardOut = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true };
        Console.SetOut(standardOut);

        Assert.That(sw.ToString().TrimEnd(), Is.EqualTo("テスト出力"));
    }
}
