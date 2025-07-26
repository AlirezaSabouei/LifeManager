using Application.Common.Interfaces;
using Domain.Entities.Users;

namespace Application.Users.Queries;

public record GetUserByIdQuery : IRequest<User?>
{
    public long Id { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User?>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public GetUserByIdQueryHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _applicationDbContext.Users.FirstOrDefaultAsync(a => a.Id == request.Id);
    }
}
