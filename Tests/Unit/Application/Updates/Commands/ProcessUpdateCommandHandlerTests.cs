using Application.Common.Interfaces;
using Application.Updates.Commands;
using Application.Updates.States;
using Application.Users.Commands;
using AutoFixture;
using Domain.Entities.Users;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using UnitTests.Common;

namespace UnitTests.Application.Updates.Commands;

public class ProcessUpdateCommandHandlerTests : HandlerTestsBase<ProcessUpdateCommandHandler, ProcessUpdateCommand>
{
    private ITelegramBot _telegramBot;
    private IServiceProvider _serviceProviderMock;
    private Start _startState;
    private Water _waterState;
    private Invalid _invalidState;

    protected override ProcessUpdateCommand CreateRequest()
    {
        return new Fixture().Build<ProcessUpdateCommand>().Create();
    }

    protected override ProcessUpdateCommandHandler CreateHandler()
    {
        return new(_serviceProviderMock!);
    }

    protected override void SetupDependencies()
    {
        _telegramBot = Substitute.For<ITelegramBot>();
        _serviceProviderMock = Substitute.For<IServiceProvider>();

        SetupStartState();
        SetupWaterState();
        SetupInvalidState(); 
    }

    private void SetupStartState()
    {
        var commandHandler = Substitute.For<IRequestHandler<CreateUserCommand, User>>();

        _startState = new Start(_telegramBot, commandHandler);
        _serviceProviderMock.GetService(typeof(Start)).Returns(_startState);
        _serviceProviderMock.GetRequiredService(typeof(Start)).Returns(_startState);
    }

    private void SetupWaterState()
    {
        _waterState = new Water(_telegramBot);
        _serviceProviderMock.GetService(typeof(Water)).Returns(_waterState);
        _serviceProviderMock.GetRequiredService(typeof(Water)).Returns(_waterState);
    }

    private void SetupInvalidState()
    {
        _invalidState = new Invalid(_telegramBot);
        _serviceProviderMock.GetService(typeof(Invalid)).Returns(_invalidState);
        _serviceProviderMock.GetRequiredService(typeof(Invalid)).Returns(_invalidState);
    }

    [TestCase("/start")]
    [TestCase("/water")]
    [TestCase("invalid")]
    [Ignore("I dunno how to test it")]
    public async Task Handle_CallsCorrectStateBasedOnText(string commandText)
    {
        // Arrange
        Request.Text = commandText;

        // Act
        await Handler.Handle(Request, CancellationToken.None);

        // Assert
        if (commandText == "/start")
            await _startState.Received(1).HandleAsync(Request);
        else if (commandText == "/water")
            await _waterState.Received(1).HandleAsync(Request);
        else
            await _invalidState.Received(1).HandleAsync(Request);
    }
}
