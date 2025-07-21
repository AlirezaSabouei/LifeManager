using Telegram.Bot.Types;

namespace Application.Common.Interfaces;

public interface IState
{
    public string State { get; }
    public Task HandleAsync(Message message);
}