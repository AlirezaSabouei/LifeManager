using Application.Common.Interfaces;
using Application.Users.Queries;
using Domain.Entities.Users;
using MockQueryable.NSubstitute;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using UnitTests.Common;

namespace UnitTests.Application.Users;

public class GetUserByIdQueryHandlerTests : HandlerTestsBase<GetUserByIdQueryHandler, GetUserByIdQuery, User>
{
    private IApplicationDbContext? _applicationDbContextMock;

    private User? _user = null;

    protected override GetUserByIdQuery CreateRequest()
    {
        return new GetUserByIdQuery()
        {
            Id = _user!.Id
        };
    }

    protected override GetUserByIdQueryHandler CreateHandler()
    {
        return new(_applicationDbContextMock!);
    }

    protected override void SetupDependencies()
    {
        _user = new User() { Id = 1, Name = "Name" };
        var users = new List<User> { _user };
        var mock = users.AsQueryable().BuildMockDbSet();
        _applicationDbContextMock = Substitute.For<IApplicationDbContext>();
        _applicationDbContextMock.Users.Returns(mock);
    }

    [Test]
    public async Task Handle_UserExists_ReturnsUser()
    {
        var result = await Handler.Handle(Request, new CancellationToken());

        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(_user);
    }

    [Test]
    public async Task Handle_UserDoesNotExists_ReturnsNull()
    {
        Request.Id = 123123;

        var result = await Handler.Handle(Request, new CancellationToken());

        result.ShouldBeNull();
    }
}
