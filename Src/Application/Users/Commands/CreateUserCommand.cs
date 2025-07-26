using Application.Common.Interfaces;
using Domain.Entities.Users;

namespace Application.Users.Commands;

public record CreateUserCommand : IRequest<User>
{
    public required User User { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public CreateUserCommandHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<User> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        if (!_applicationDbContext.Users.Any(a=>a.Id == request.User.Id))
        {
            await _applicationDbContext.Users.AddAsync(request.User);
            await _applicationDbContext.SaveChangesAsync();
        }
        return await _applicationDbContext.Users
            .FirstAsync(a=>a.Id == request.User.Id);
    }
}