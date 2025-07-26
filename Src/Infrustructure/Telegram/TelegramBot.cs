using Application.Common.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrustructure.Telegram;

public class TelegramBot(ITelegramBotClient client) : ITelegramBot
{
    private readonly ITelegramBotClient _client = client;

    public async Task SendMessageAsync(long chatId, string message)
    {
        await _client.SendMessage(chatId, message);
    }

    public async Task SendMessageAsync(long chatId, string message, Dictionary<string, string> keyBoardButtons)
    {
        var keyBoard = CreateKeyBoard(keyBoardButtons);
        await _client.SendMessage(chatId, message, replyMarkup: keyBoard);

    }

    private InlineKeyboardMarkup CreateKeyBoard(Dictionary<string, string> keyBoardButtons)
    {
        InlineKeyboardMarkup inlineKeyboardMarkup = new();
        foreach (var button in keyBoardButtons)
        {
            inlineKeyboardMarkup.AddNewRow();
            inlineKeyboardMarkup.AddButton(new InlineKeyboardButton(button.Key,button.Value));
        }
        
        return inlineKeyboardMarkup;
    }
}
