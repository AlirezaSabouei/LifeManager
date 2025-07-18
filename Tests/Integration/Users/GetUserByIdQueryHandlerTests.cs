using Application.Users.Queries;
using AutoFixture;
using Domain.Entities.Users;
using Infrustructure.Data;
using NUnit.Framework;
using Shouldly;

namespace IntegrationTests.Users;

public class GetUserByIdQueryHandlerTests
{
    [TestCase]
    public async Task HandleAsync_ValidId_ReturnsUser()
    {
        using ApplicationDbContext context = TestDbContextFactory.CreateDbContext();
        var user = await CreateUserInDatabaseAsync(context);
        GetUserByIdQueryHandler handler = new(context);
        GetUserByIdQuery query = new()
        {
            Id = user.Id,
        };

        var result = await handler.Handle(query, new CancellationToken());

        result.ShouldNotBeNull();
        result.Id.ShouldBe(user.Id);
        result.Name.ShouldBe(user.Name);

    }

    private async Task<User> CreateUserInDatabaseAsync(ApplicationDbContext context)
    {
        var user = new Fixture().Build<User>().Create();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    [TestCase]
    public async Task HandleAsync_InvalidId_RetunsNull()
    {
        using ApplicationDbContext context = TestDbContextFactory.CreateDbContext();        
        GetUserByIdQueryHandler handler = new(context);
        GetUserByIdQuery query = new()
        {
            Id = 10,
        };

        var result = await handler.Handle(query, new CancellationToken());

        result.ShouldBeNull();
    }
}
