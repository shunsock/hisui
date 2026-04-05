using Hisui.Domain.ValueObject;
using Hisui.UseCase.Handler;
using Hisui.UseCase.Query;

namespace hisui.Tests.UseCase.Handler;

[TestFixture]
public class ConvertTextWidthHandlerTests
{
    private ConvertTextWidthHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _handler = new ConvertTextWidthHandler();
    }

    [Test]
    public void Handle_ToHalfWidth_DelegatesToDomainService()
    {
        var query = new ConvertTextWidthQuery(
            new UnicodeText("Ａｂｃ１２３"),
            ConversionDirection.ToHalfWidth
        );

        var result = _handler.Handle(query);

        Assert.That(result.Value, Is.EqualTo("Abc123"));
    }

    [Test]
    public void Handle_ToFullWidth_DelegatesToDomainService()
    {
        var query = new ConvertTextWidthQuery(
            new UnicodeText("Abc123"),
            ConversionDirection.ToFullWidth
        );

        var result = _handler.Handle(query);

        Assert.That(result.Value, Is.EqualTo("Ａｂｃ１２３"));
    }

    [Test]
    public void Handle_EmptyInput_ReturnsEmptyResult()
    {
        var query = new ConvertTextWidthQuery(
            new UnicodeText(""),
            ConversionDirection.ToHalfWidth
        );

        var result = _handler.Handle(query);

        Assert.That(result.Value, Is.EqualTo(""));
    }
}
