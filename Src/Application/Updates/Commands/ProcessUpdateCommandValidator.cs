namespace Application.Updates.Commands;

public class ProcessUpdateCommandValidator : AbstractValidator<ProcessUpdateCommand>
{
    public ProcessUpdateCommandValidator()
    {
        RuleFor(a => a.UpdateId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("'{PropertyName}' must be provided.");

        RuleFor(a => a.UserId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("'{PropertyName}' must be provided.");

        RuleFor(a => a.ChatId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("'{PropertyName}' must be provided.");
    }
}