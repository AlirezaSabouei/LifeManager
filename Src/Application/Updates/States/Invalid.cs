using Application.Common.Interfaces;
using Application.Updates.Commands;

namespace Application.Updates.States;

public class Invalid(ITelegramBot telegramBot) : IState
{
    private readonly ITelegramBot _telegramBot = telegramBot;

    public string StateText => "invalid";

    public async Task HandleAsync(ProcessUpdateCommand request)
    {
        await SendErrorMessageAsync(request);
    }

    private async Task SendErrorMessageAsync(ProcessUpdateCommand request)
    {
        await _telegramBot.SendMessageAsync(request.ChatId, $"Some errors happened");
    }
}
