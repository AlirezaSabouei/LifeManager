using Application.Common.Interfaces;
using Application.Updates.Commands;

namespace Application.Updates.States;

public class Water(ITelegramBot telegramBot) : IState
{
    private readonly ITelegramBot _telegramBot = telegramBot;

    public string StateText => "/water";

    public async Task HandleAsync(ProcessUpdateCommand processUpdateCommand)
    {
        try
        {
            await SendAnswerAsync(processUpdateCommand);
        }
        catch (Exception ex)
        {
            await SendErrorMessageAsync(processUpdateCommand);
        }
    }

    private async Task SendAnswerAsync(ProcessUpdateCommand processUpdateCommand)
    {
        await _telegramBot.SendMessageAsync(
            processUpdateCommand.ChatId,
            CreateAnswerText(processUpdateCommand),
            CreateKeyboard(processUpdateCommand));
    }

    private string CreateAnswerText(ProcessUpdateCommand processUpdateCommand)
    {
        return $"It is important to monitor your daily water intake {processUpdateCommand.FirstName} 🥤" +
            Environment.NewLine +
            Environment.NewLine +
            $"You have drank {3} glasses until now 🥳";
    }

    private Dictionary<string, string> CreateKeyboard(ProcessUpdateCommand processUpdateCommand)
    {
        Dictionary<string, string> keyboard = new()
    {
        { "Button1", "button1" },
        { "Button2", "button2" },
        { "Button3", "button3" }
    };
        return keyboard;
    }

    private async Task SendErrorMessageAsync(ProcessUpdateCommand processUpdateCommand)
    {
        await _telegramBot.SendMessageAsync(processUpdateCommand.ChatId, $"Some errors happened");
    }
}