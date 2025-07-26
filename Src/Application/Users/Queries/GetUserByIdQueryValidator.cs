namespace Application.Users.Queries;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(v => v.Id)
            .Must(BeGreaterThanZero)
                .WithMessage("'{PropertyName}' must be provided.");
    }

    public bool BeGreaterThanZero(long id)
    {
        return id > 0;
    }
}
