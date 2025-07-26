using Application.Updates.Commands;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UpdatesController(IMapper mapper, ISender mediator) : BaseApiController(mapper, mediator)
{
    [HttpPost]
    [ApiVersion("1.0")]
    public async Task<ActionResult> ProcessMessageAsync([FromBody] Update update)
    {
        if (update.Type != Telegram.Bot.Types.Enums.UpdateType.Message &&
            update.Type != Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
        {
            return BadRequest();
        }
        var command = CreateProcessUpdateCommand(update);
        await Mediator!.Send(command);
        return NoContent();
    }

    private ProcessUpdateCommand CreateProcessUpdateCommand(Update update)
    {
        return update.Message != null ? CreateCommandFromMessage(update) : CreateCommandFromCallBackQUery(update);

    }

    private ProcessUpdateCommand CreateCommandFromMessage(Update update)
    {
        return new ProcessUpdateCommand()
        {
            ChatId = update.Message!.Chat.Id,
            FirstName = update.Message!.From!.FirstName,
            IsBot = update.Message!.From.IsBot,
            LanguageCode = update.Message!.From!.LanguageCode!,
            Text = update.Message!.Text!,
            UpdateId = update.Id,
            UserId = update.Message!.From.Id,
            UserName = update.Message!.From!.Username!
        };
    }

    private ProcessUpdateCommand CreateCommandFromCallBackQUery(Update update)
    {
        return new ProcessUpdateCommand()
        {
            ChatId = update.CallbackQuery!.Message!.Chat.Id,
            FirstName = update.CallbackQuery!.From!.FirstName,
            IsBot = update.CallbackQuery!.From.IsBot,
            LanguageCode = update.CallbackQuery!.From!.LanguageCode!,
            Text = update.CallbackQuery!.Data!,
            UpdateId = update.Id,
            UserId = update.CallbackQuery!.From.Id,
            UserName = update.CallbackQuery!.From!.Username!
        };
    }
}
