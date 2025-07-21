using Application.Common.Interfaces;
using Telegram.Bot.Types;

public class Start : IState
{
    public string State => "/start";

    public async Task HandleAsync(Message message)
    {
        Console.WriteLine("Message is start");
    }
}