using AutoMapper;
using Application.Users.Dto;
using Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.Users;
using Application.Users.Commands;
using Telegram.Bot.Types;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TelegramController
{
    private readonly IStateManager _stateManager;

    public TelegramController(IStateManager stateManager) 
    {
        _stateManager = stateManager;
    }

    [HttpPost]
    [ApiVersion("1.0")]
    public async Task<ActionResult<UserDto>> ProcessMessageAsync([FromBody]Message message)
    {
        var state = _stateManager.ChooseState(message.Text);
        await state.HandleAsync(message);
        return null;
    }
}
