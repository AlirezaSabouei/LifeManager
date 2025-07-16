using LifeManager.Application.Users.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UsersController : ControllerBase
{
    [HttpGet("User/{id}")]
    public Task<ActionResult> GetUserByIdAsync(Guid userId)
    {
        var query = new GetUserByIdQuery()
        {
            Id = userId
        };

    }
}
