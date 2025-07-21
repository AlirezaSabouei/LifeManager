using AcceptanceTests.Drivers;
using NUnit.Framework;
using System.Net.Http.Json;
using Shouldly;
using Application.Users.Dto;
using Api;
using Newtonsoft.Json;
using System.Text;
using Application.Common.Exceptions;
using Domain.Entities.Users;

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
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.InternalServerError);
        response.Content.ShouldNotBeNull();
        var errorMessage = await response.Content.ReadAsStringAsync();
        errorMessage!.ShouldContain($"One or more validation failures have occurred");
    }

    [Test]
    public async Task GetUserByIdAsync_IdIsValidAndUserExists_UserIsReturned()
    {
        var user1 = _userDriver.CreateUserInstance();
        var user2 = _userDriver.CreateUserInstance();
        await _userDriver.AddUserToDatabaseAsync(user1);
        await _userDriver.AddUserToDatabaseAsync(user2);

        var response = await _userDriver.HttpClient.GetAsync($"api/v1/users/{user2.Id}");

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        response.Content.ShouldNotBeNull();
        (await response.Content.ReadFromJsonAsync<UserDto>())!.Id.ShouldBe(user2.Id);
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

    #region CreateUser

    [Test]
    public async Task CreateUserAsync_TelegramIdNotUnique_ExceptionThrown()
    {
        var user = _userDriver.CreateUserInstance();
        user.TelegramId = "telegramid";
        await _userDriver.AddUserToDatabaseAsync(user);
        var userDto = new UserDto()
        {
            Name = "name",
            TelegramId = user.TelegramId
        };
        var httpContent = new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json");

        var response = await _userDriver.HttpClient.PostAsync($"api/v1/users", httpContent);

        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.InternalServerError);
        (await response.Content.ReadAsStringAsync()).ShouldContain("One or more validation failures have occurred");
    }

    //LOWER CASE SAVING

    [Test]
    public async Task CreateUserAsync_InputValidAndUserNotExist_UserCreatedAndReturned()
    {
        var userToCreat = _userDriver.CreateUserDtoInstance();
        var httpContent = new StringContent(JsonConvert.SerializeObject(userToCreat), Encoding.UTF8, "application/json");

        var response = await _userDriver.HttpClient.PostAsync($"api/v1/users", httpContent);

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        response.Content.ShouldNotBeNull();
        var returnedUser = await response.Content.ReadFromJsonAsync<UserDto>();
        var createdUser = await _userDriver.HttpClient.GetAsync($"api/v1/users/{userId}");
        dbCreatedUser.ShouldNotBeNull();
    }

    [Test]
    public async Task CreateUserAsync_UserCreated_TelegramIdShouldBeLowerCased()
    {
        var userToCreat = _userDriver.CreateUserDtoInstance();
        userToCreat.TelegramId = userToCreat.TelegramId!.ToUpper();
        var httpContent = new StringContent(JsonConvert.SerializeObject(userToCreat), Encoding.UTF8, "application/json");

        var response = await _userDriver.HttpClient.PostAsync($"api/v1/users", httpContent);

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        response.Content.ShouldNotBeNull();
        var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();
        var dbCreatedUser = _userDriver.GetUserFromDatabase(createdUser!.Id);
        dbCreatedUser!.TelegramId.ShouldBe(dbCreatedUser!.TelegramId.ToLower());
        createdUser.TelegramId.ShouldBe(createdUser.TelegramId.ToLower());
    }

    #endregion
}

