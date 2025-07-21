using Application.Common.Interfaces;
using Telegram.Bot.Types;

namespace Application.Telegram.States;

public class Invalid : IState
{
    public string State => "";

    public async Task HandleAsync(Message message)
    {
        Console.WriteLine("Message is invalid");
    }
}
