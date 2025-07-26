namespace Application.Common.Interfaces;
//TODO : How about changing it to an abstract
public interface ITelegramBot
{
    Task SendMessageAsync(long chatId, string message);
    Task SendMessageAsync(long chatId, string message, Dictionary<string, string> keyBoardButtons);
}
