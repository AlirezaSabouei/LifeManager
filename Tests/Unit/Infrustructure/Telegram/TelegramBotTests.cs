using Application.Common.Interfaces;
using Application.Updates.Commands;
using Application.Updates.States;
using Application.Users.Commands;
using AutoFixture;
using Domain.Entities.Users;
using Infrustructure.Telegram;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using UnitTests.Common;

namespace UnitTests.Infrustructure.Telegram;

public class TelegramBotTests
{
    private ITelegramBotClient _telegramBotClientMock;

    private TelegramBot _telegramBot;

    [OneTimeSetUp]
    public void SetupDependencies()
    {
        _telegramBotClientMock = Substitute.For<ITelegramBotClient>();
        _telegramBot = new TelegramBot(_telegramBotClientMock);
    }

    [Test]
    public async Task SendMessageAsync_SimpleMessageSend_TelegramBotClientIsCalled()
    {
        long chatId = 123;
        string message = "my test message";

        await _telegramBot.SendMessageAsync(chatId, message);

        await _telegramBotClientMock
            .Received(1)
            .SendMessage(chatId, message);
    }

    [Test]
    public async Task SendMessageAsync_MessageHasKeyboard_TelegramBotClientIsCalled()
    {
        long chatId = 123;
        string message = "my test message";
        Dictionary<string, string> keyboard = [];
        keyboard.Add("button1", "Val1");

        await _telegramBot.SendMessageAsync(chatId, message);

        await _telegramBotClientMock
            .Received(1)
            .SendMessage(chatId, message, replyMarkup: Arg.Any<InlineKeyboardMarkup>());
    }
}
