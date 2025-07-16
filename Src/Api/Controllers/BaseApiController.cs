using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.Api.Controllers;

public class BaseApiController : ControllerBase
{
    protected readonly IMapper _mapper;

    public BaseApiController(IMapper mapper)
    {
        _mapper = mapper;
    }
    protected ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}