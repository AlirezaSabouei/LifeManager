using RestSharp;
using NUnit;
using NUnit.Framework;

namespace LifeManager.AcceptanceTests.Controllers.Users;

public class UserControllerTests
{
    private RestClient _restClient;

    [SetUp]
    public void Setup()
    {
        _restClient = new RestClient("http://localhost/api/users");
    }

    // [Test]
    // public async Task CreateUserAsync_InputIsValid_UserIsCreated()
    // {
    //     var request = new RestRequest(Method.Post);
    //     request.AddJsonBody(new
    //     {
    //         Username = "testuser",
    //         Email = "testuser@example.com",
    //         Password = "TestPassword123!"
    //     });

    //     var response = await _restClient.ExecuteAsync(request);

    //     Assert.That(response.IsSuccessful, Is.True);
    //     Assert.That((int)response.StatusCode, Is.EqualTo(201));
    // }

    
    [Test]
    public async Task GetUserByIdAsync_IdIsValid_UserIsReturned()
    {        
        var userId = 1; // Replace with a valid user ID as needed
        var request = new RestRequest($"{userId}", Method.Get);

        var response = await _restClient.ExecuteAsync(request);

        Assert.That(response.IsSuccessful, Is.True);
        Assert.That((int)response.StatusCode, Is.EqualTo(200));
        Assert.That(response.Content, Is.Not.Null.And.Not.Empty);
    }
}

