using Application.Updates.Commands;
using AutoFixture;
using NUnit.Framework;
using Shouldly;
using UnitTests.Common;

namespace UnitTests.Application.Updates.Commands;

public class ProcessUpdateCommandValidatorTests : ValidatorTestsBase<ProcessUpdateCommandValidator, ProcessUpdateCommand>
{
    protected override ProcessUpdateCommand CreateRequest()
    {
        return new Fixture().Build<ProcessUpdateCommand>().Create();
    }

    protected override ProcessUpdateCommandValidator CreateValidator()
    {
        return new ProcessUpdateCommandValidator();
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-10)]
    public async Task Validation_InvalidUpdateId_ThrowsException(int updateId)
    {
        Request.UpdateId = updateId;

        var result = await Validator.ValidateAsync(Request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(error => error.PropertyName == nameof(ProcessUpdateCommand.UpdateId));
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-10)]
    public async Task Validation_InvalidUserId_ThrowsException(int userId)
    {
        Request.UserId = userId;

        var result = await Validator.ValidateAsync(Request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(error => error.PropertyName == nameof(ProcessUpdateCommand.UserId));
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-10)]
    public async Task Validation_ChatId_ThrowsException(int chatId)
    {
        Request.ChatId = chatId;

        var result = await Validator.ValidateAsync(Request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(error => error.PropertyName == nameof(ProcessUpdateCommand.ChatId));
    }
}
