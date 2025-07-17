using AutoMapper;
using Application.Users.Dto;
using Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;

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
}
