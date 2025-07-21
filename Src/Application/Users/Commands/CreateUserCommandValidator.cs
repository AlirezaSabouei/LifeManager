using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Users.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public CreateUserCommandValidator(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;

        RuleFor(a => a.User.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("'{PropertyName}' must be provided.");

        RuleFor(a => a.User.TelegramId)
            .MustAsync((x, cancellation) => BeUniqueInDataBase(x))
            .WithMessage("'{PropertyName}' already registered.");
    }

    public async Task<bool> BeUniqueInDataBase(string? telegramId)
    {
        if (!string.IsNullOrWhiteSpace(telegramId))
        {
            return !await _applicationDbContext.Users
                .AnyAsync(a=> a.TelegramId == telegramId);
        }
        return true;
    }
}