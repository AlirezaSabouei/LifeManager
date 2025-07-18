using Api;
using AutoFixture;
using Domain.Entities.Users;
using System.Net.Http.Json;

namespace AcceptanceTests.Drivers;

public class UserDriver(IntegrationsWebApplicationFactory<Program> factory) : DriverBase(factory)
{
    public async Task<List<User>> AddUsersToDatabaseAsync()
    {
        Fixture f = new();
        var user1 = f.Build<User>().Create();
        var user2 = f.Build<User>().Create();
        await DbContext!.Users.AddAsync(user1);
        await DbContext!.Users.AddAsync(user2);
        await DbContext.SaveChangesAsync();
        return DbContext.Users.ToList();
    }

    public async Task ClearDatabaseAsync()
    {
        DbContext!.Users.RemoveRange(DbContext.Users);
        await DbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetUserAsync(int userId)
    {
        var response = await HttpClient.GetAsync($"Users/{userId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<User>>();
    }
}
