using AcceptanceTests.Drivers;
using Api;
using Application.Users.Dto;
using AutoFixture;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System.Net.Http.Json;
using Telegram.Bot.Types;

namespace AcceptanceTests.Controllers.Updates;

public class UpdatesControllerTests
{
   // private ITelegramBotClient _telegramBotClient;
    private UserDriver _usersDriver;

    public UpdatesControllerTests()
    {
        var factory = new IntegrationsWebApplicationFactory<Program>();
        _usersDriver = new UserDriver(factory);
    }

    [SetUp]
    public async Task Setup()
    {
        //_telegramBotClient = Substitute.For<ITelegramBotClient>();
        await _usersDriver.ClearDatabaseAsync();
    }

    #region /Start

    [Test]
    public async Task UserSendsStartMessage_NewUserCreatedAndMessageIsSent()
    {
        var fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
        .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        var update = fixture.Build<Update>()
            .OmitAutoProperties()
            .With(a=>a.Message,fixture.Build<Message>()
                .Without(b=>b.ForwardOrigin)
                .Without(a => a.ExternalReply)
                .Without(a => a.PaidMedia)
                .Without(a => a.ChatBackgroundSet)
                .Create())            
            .With(a=>a.Id,fixture.Create<int>())
            .Create();
        update.Message.Text = "/start";

        var response = await _usersDriver.HttpClient.PostAsJsonAsync("api/v1/Updates", update);

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);
        var getResponse = await _usersDriver.HttpClient.GetAsync($"api/v1/users/{update.Message.From!.Id}");
        var user = await getResponse.Content.ReadFromJsonAsync<UserDto>();
        user!.Id.ShouldBe(update.Message.From!.Id);
        user.WaterIntake.CurrentIntake.ShouldBe(0);
        user.WaterIntake.LastDay.Date.Day.ShouldBe(DateTime.Now.Day);
    }

    #endregion

    #region /invalid

    [Test]
    public async Task UserSendsInvalidMessage_TelegramBotInvalidMessageIsSent()
    {
        var fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
        .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        var update = fixture.Build<Update>()
            .OmitAutoProperties()
            .With(a => a.Message, fixture.Build<Message>()
                .Without(b => b.ForwardOrigin)
                .Without(a => a.ExternalReply)
                .Without(a => a.PaidMedia)
                .Without(a => a.ChatBackgroundSet)
                .Create())
            .With(a => a.Id, fixture.Create<int>())
            .Create();
        update.Message.Text = "some invalid message";

        var response = await _usersDriver.HttpClient.PostAsJsonAsync("api/v1/Updates", update);

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);
        await _usersDriver.Factory.TelegramBotMock
            .Received(1)
            .SendMessageAsync(
                Arg.Is<long>(a => a == update.Message.Chat.Id),
                Arg.Is<string>(a => a == "Some errors happened"));
    }

    #endregion

    #region GetUserById

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-10)]
    public async Task GetUserByIdAsync_IdIsNotValid_ExceptionIsRaised(int userId)
    {
        var response = await _usersDriver.HttpClient.GetAsync($"api/v1/users/{userId}");

        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.InternalServerError);
        response.Content.ShouldNotBeNull();
        var errorMessage = await response.Content.ReadAsStringAsync();
        errorMessage!.ShouldContain($"One or more validation failures have occurred");
    }

    [Test]
    public async Task GetUserByIdAsync_IdIsValidAndUserExists_UserIsReturned()
    {
        var user1 = _usersDriver.CreateUserInstance();
        var user2 = _usersDriver.CreateUserInstance();
        await _usersDriver.AddUserToDatabaseAsync(user1);
        await _usersDriver.AddUserToDatabaseAsync(user2);

        var response = await _usersDriver.HttpClient.GetAsync($"api/v1/users/{user2.Id}");

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        response.Content.ShouldNotBeNull();
        (await response.Content.ReadFromJsonAsync<UserDto>())!.Id.ShouldBe(user2.Id);
    }

    [Test]
    public async Task GetUserByIdAsync_IdIsValidButUserDoesNotExist_Returned()
    {
        var userId = 100;

        var response = await _usersDriver.HttpClient.GetAsync($"api/v1/users/{userId}");

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);
    }

    #endregion

    #region CreateUser

    //[Test]
    //public async Task CreateUserAsync_TelegramIdNotUnique_ExceptionThrown()
    //{
    //    var user = _usersDriver.CreateUserInstance();
    //    user.TelegramId = "telegramid";
    //    await _usersDriver.AddUserToDatabaseAsync(user);
    //    var userDto = new UserDto()
    //    {
    //        Name = "name",
    //        TelegramId = user.TelegramId
    //    };
    //    var httpContent = new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json");

    //    var response = await _usersDriver.HttpClient.PostAsync($"api/v1/users", httpContent);

    //    response.IsSuccessStatusCode.ShouldBeFalse();
    //    response.StatusCode.ShouldBe(System.Net.HttpStatusCode.InternalServerError);
    //    (await response.Content.ReadAsStringAsync()).ShouldContain("One or more validation failures have occurred");
    //}

    //LOWER CASE SAVING

    //[Test]
    //public async Task CreateUserAsync_InputValidAndUserNotExist_UserCreatedAndReturned()
    //{
    //    var userToCreat = _usersDriver.CreateUserDtoInstance();
    //    var httpContent = new StringContent(JsonConvert.SerializeObject(userToCreat), Encoding.UTF8, "application/json");

    //    var response = await _usersDriver.HttpClient.PostAsync($"api/v1/users", httpContent);

    //    response.IsSuccessStatusCode.ShouldBeTrue();
    //    response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
    //    response.Content.ShouldNotBeNull();
    //    var returnedUser = await response.Content.ReadFromJsonAsync<UserDto>();
    //    var createdUser = await _usersDriver.HttpClient.GetAsync($"api/v1/users/{userId}");
    //    dbCreatedUser.ShouldNotBeNull();
    //}

    //[Test]
    //public async Task CreateUserAsync_UserCreated_TelegramIdShouldBeLowerCased()
    //{
    //    var userToCreat = _usersDriver.CreateUserDtoInstance();
    //    userToCreat.TelegramId = userToCreat.TelegramId!.ToUpper();
    //    var httpContent = new StringContent(JsonConvert.SerializeObject(userToCreat), Encoding.UTF8, "application/json");

    //    var response = await _usersDriver.HttpClient.PostAsync($"api/v1/users", httpContent);

    //    response.IsSuccessStatusCode.ShouldBeTrue();
    //    response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
    //    response.Content.ShouldNotBeNull();
    //    var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();
    //    var dbCreatedUser = _usersDriver.GetUserFromDatabase(createdUser!.Id);
    //    dbCreatedUser!.TelegramId.ShouldBe(dbCreatedUser!.TelegramId.ToLower());
    //    createdUser.TelegramId.ShouldBe(createdUser.TelegramId.ToLower());
    //}

    #endregion
}

