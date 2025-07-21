using Application.Users.Queries;
using Domain.Entities.Users;
using NUnit.Framework;
using Shouldly;

namespace UnitTests.Application.Users;

public class GetUserByIdQueryValidatorTests
{
    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-10)]
    public async Task Validation_InvalidUserId_ThrowsException(int userId)
    {
        var query = new GetUserByIdQuery()
        {
            Id = userId
        };
        GetUserByIdQueryValidator validator = new();

        var result = await validator.ValidateAsync(query);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(error => error.PropertyName == nameof(User.Id));
    }

    [Test]
    [TestCase(1)]
    [TestCase(10)]
    [TestCase(50)]
    public async Task Validation_ValidUserId_ReturnsTrue(int userId)
    {
        var query = new GetUserByIdQuery()
        {
            Id = userId
        };
        GetUserByIdQueryValidator validator = new();

        var result = await validator.ValidateAsync(query);

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }
}
