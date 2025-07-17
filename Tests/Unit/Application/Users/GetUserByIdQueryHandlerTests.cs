using Application.Common.Interfaces;
using Application.Users.Queries;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System.Linq.Expressions;

namespace UnitTests.Application.Users;

public class GetUserByIdQueryHandlerTests
{
    private IApplicationDbContext? _applicationDbContextMock;

    private User? _user = null;

    [OneTimeSetUp]
    public void InitDependencies()
    {
        _user = new() { Id = 1, Name = "Alireza" };
        var users = new List<User> { _user };
        var mock = users.AsQueryable().BuildMockDbSet();
        _applicationDbContextMock = Substitute.For<IApplicationDbContext>();
        _applicationDbContextMock.Users.Returns(mock);
    }

    [Test]
    public async Task Handle_UserExists_ReturnsUser()
    {
        var query = new GetUserByIdQuery()
        {
            Id = _user!.Id
        };
        GetUserByIdQueryHandler handler = new(_applicationDbContextMock!);

        var result = await handler.Handle(query, new CancellationToken());

        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(_user);
    }

    [Test]
    public async Task Handle_UserDoesNotExists_ReturnsNull()
    {
        var query = new GetUserByIdQuery()
        {
            Id = 112312
        };
        GetUserByIdQueryHandler handler = new(_applicationDbContextMock!);

        var result = await handler.Handle(query, new CancellationToken());

        result.ShouldBeNull();
    }
}
