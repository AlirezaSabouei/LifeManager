using Application.Users.Commands;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using Telegram.Bot;

namespace UnitTests.Application.Telegram.States;

//public class StartTests
//{
//    private ITelegramBotClient _telegramBotClientMock;
//    private IRequestHandler<CreateUserCommand, Domain.Entities.Users.User> _createUserCommandHandler;

//    [OneTimeSetUp]
//    public void InitDependencies()
//    {
//        _telegramBotClientMock = Substitute.For<ITelegramBotClient> ();
//        _createUserCommandHandler = Substitute.For<IRequestHandler<CreateUserCommand, Domain.Entities.Users.User>>();
//    }

//    //[Test]
//    //public Task HandleAsync_ValidInput_CreateUserCommand()
//    //{
//    //    var state = new Start(_telegramBotClientMock, _createUserCommandHandler);

//    //    state.HandleAsync()
//    //}

//    //private Message CreateMessage()
//    //{
//    //    return new AutoFixture
//    //}
//}
