using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Entities.Users;

namespace LifeManager.Application.Users.Queries;

public record GetUserByIdQuery : IRequest<User>
{
    public int Id { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public GetUserByIdQueryHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _applicationDbContext.Users.FirstAsync(a => a.Id == request.Id);
    }
}
