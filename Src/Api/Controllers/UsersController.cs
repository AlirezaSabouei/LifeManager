using AutoMapper;
using Application.Users.Dto;
using Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities.Users;
using Application.Users.Commands;

namespace Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UsersController : BaseApiController
{
    public UsersController(IMapper mapper, ISender mediatr) 
        : base(mapper, mediatr)
    {
    }

    [HttpGet("{id}")]
    [ApiVersion("1.0")]
    public async Task<ActionResult<UserDto?>> GetUserByIdAsync(int id)
    {
        var query = new GetUserByIdQuery()
        {
            Id = id
        };
        var user = await Mediator!.Send(query);
        return _mapper.Map<UserDto?>(user);
    }

    [HttpPost]
    [ApiVersion("1.0")]
    public async Task<ActionResult<UserDto>> CreateUserAsync([FromBody]UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        var command = new CreateUserCommand()
        {
            User = user
        };
        var result = await Mediator!.Send(command);
        return _mapper.Map<UserDto>(result);
    }
}
