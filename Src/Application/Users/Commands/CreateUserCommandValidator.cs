using Application.Common.Interfaces;

namespace Application.Users.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(IApplicationDbContext applicationDbContext)
    {
        RuleFor(a => a.User.Id)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("'{PropertyName}' must be provided.");
    }
}