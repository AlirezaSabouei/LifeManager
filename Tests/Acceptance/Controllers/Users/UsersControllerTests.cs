using AcceptanceTests.Drivers;
using NUnit.Framework;
using System.Net.Http.Json;
using Shouldly;
using Application.Users.Dto;
using Api;

namespace AcceptanceTests.Controllers.Users;

public class UserControllerTests
{
    private UserDriver _userDriver;

    public UserControllerTests()
    {
        var factory = new IntegrationsWebApplicationFactory<Program>();        
        _userDriver = new UserDriver(factory);        
    }

    [SetUp]
    public async Task Setup()
    {
        await _userDriver.ClearDatabaseAsync();
    }

    #region GetUserById

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-10)]
    public async Task GetUserByIdAsync_IdIsNotValid_ExceptionIsRaised(int userId)
    {
        var response = await _userDriver.HttpClient.GetAsync($"api/v1/users/{userId}");

        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);
        response.Content.ShouldNotBeNull();
        (await response.Content.ReadAsStringAsync())
            .ShouldContain("One or more validation errors occurred");
    }

    [Test]
    public async Task GetUserByIdAsync_IdIsValidAndUserExists_UserIsReturned()
    {
        var users = await _userDriver.AddUsersToDatabaseAsync();
        var userId = users[0].Id;

        var response = await _userDriver.HttpClient.GetAsync($"api/v1/users/{userId}");

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        response.Content.ShouldNotBeNull();
        (await response.Content.ReadFromJsonAsync<UserDto>()).Id.ShouldBe(userId);
    }

    [Test]
    public async Task GetUserByIdAsync_IdIsValidButUserDoesNotExist_Returned()
    {
        var userId = 100;

        var response = await _userDriver.HttpClient.GetAsync($"api/v1/users/{userId}");

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);
    }

    #endregion
}

