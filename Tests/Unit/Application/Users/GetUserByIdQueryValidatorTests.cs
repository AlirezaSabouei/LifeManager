using Application.Users.Queries;
using Domain.Entities.Users;
using NUnit.Framework;
using Shouldly;
using UnitTests.Common;

namespace UnitTests.Application.Users;

public class GetUserByIdQueryValidatorTests : ValidatorTestsBase<GetUserByIdQueryValidator, GetUserByIdQuery>
{
    protected override GetUserByIdQuery CreateRequest()
    {
        return new GetUserByIdQuery()
        {
            Id = 123,
        };
    }

    protected override GetUserByIdQueryValidator CreateValidator()
    {
        return new GetUserByIdQueryValidator();
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-10)]
    public async Task Validation_InvalidUserId_ThrowsException(int userId)
    {
        Request.Id = userId;

        var result = await Validator.ValidateAsync(Request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(error => error.PropertyName == nameof(User.Id));
    }
}
