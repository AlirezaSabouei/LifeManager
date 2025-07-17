using AutoMapper;
using LifeManager.Application.Users.Dto;
using LifeManager.Application.Users.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UsersController : BaseApiController
{
    public UsersController(IMapper mapper) : base(mapper)
    {
    }

    [HttpGet("{id}")]
    [ApiVersion("1.0")]
    public async Task<ActionResult<UserDto>> GetUserByIdAsync(int id)
    {
        var query = new GetUserByIdQuery()
        {
            Id = id
        };
        var user = await _mediator!.Send(query);
        return _mapper.Map<UserDto>(user);
    }
}
