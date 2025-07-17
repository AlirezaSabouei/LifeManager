using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class BaseApiController : ControllerBase
{
    protected readonly IMapper _mapper;
    protected ISender? Mediator;

    public BaseApiController(IMapper mapper, ISender mediator)
    {
        Mediator = mediator;
        _mapper = mapper;
    }
}