using Api;
using Application.Users.Dto;
using AutoFixture;
using Domain.Entities.Users;

namespace AcceptanceTests.Drivers;

public class UserDriver(IntegrationsWebApplicationFactory<Program> factory) : DriverBase(factory)
{
    public async Task<User> AddUserToDatabaseAsync(User user)
    {
        await DbContext!.Users.AddAsync(user);
        await DbContext.SaveChangesAsync();
        return (await DbContext.Users.FindAsync(user.Id))!;
    }

    public User CreateUserInstance()
    {
        Fixture fixture = new();
        return fixture.Build<User>().Create();
    }

    public UserDto CreateUserDtoInstance()
    {
        Fixture fixture = new();
        return fixture.Build<UserDto>().Create();
    }

    public async Task ClearDatabaseAsync()
    {
        DbContext!.Users.RemoveRange(DbContext.Users);
        await DbContext.SaveChangesAsync();
    }

    public User? GetUserFromDatabase(int userId)
    {
        return DbContext!.Users.FirstOrDefault(a => a.Id == userId);
    }
}
