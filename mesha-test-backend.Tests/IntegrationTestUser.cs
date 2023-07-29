using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using mesha_test_backend.Data.Dtos;
using Task = System.Threading.Tasks.Task;

namespace mesha_test_backend.Tests;

public class IntegrationTestUser
{

    private static string _baseUrl = "/users";
    private static string _loginUrl = "/auth/login";

    private static CreateUserDto _user = new CreateUserDto()
    {
        Name = "User",
        Lastname = "Test",
        Email = "user@test.com",
        Password = "123456789"
    };
    
    [Test]
    public async Task TestCreateUser()
    {
        
        var client =  new Mesha_test_backend_application().CreateClient();
        var resp = await client.PostAsJsonAsync(_baseUrl, _user);
        
        Assert.AreEqual(HttpStatusCode.Created, resp.StatusCode);
    }
    
    [Test]
    public async Task TestValidLogin()
    {
        var client =  new Mesha_test_backend_application().CreateClient();
        await client.PostAsJsonAsync(_baseUrl, _user);

        var login = new LoginDto()
        {
            Email = _user.Email,
            Password = _user.Password
        };
        
        var respLogin = await client.PostAsJsonAsync(_loginUrl, login);
        Assert.AreEqual(HttpStatusCode.OK, respLogin.StatusCode);
    }
    
    [Test]
    public async Task TestInValidLogin()
    {
        var client =  new Mesha_test_backend_application().CreateClient();
        await client.PostAsJsonAsync(_baseUrl, _user);

        var login = new LoginDto()
        {
            Email = _user.Email,
            Password = "987654321"
        };
        
        var respLogin = await client.PostAsJsonAsync(_loginUrl, login);
        Assert.AreEqual(HttpStatusCode.BadRequest, respLogin.StatusCode);
    }
    
    [Test]
    public async Task GetOneUser()
    {
        var client =  new Mesha_test_backend_application().CreateClient();
        await client.PostAsJsonAsync(_baseUrl, _user);

        var login = new LoginDto()
        {
            Email = _user.Email,
            Password = _user.Password
        };
        
        var respLogin = await client.PostAsJsonAsync(_loginUrl, login);
        var loginData = await respLogin.Content.ReadFromJsonAsync<ReadLoginDataDto>();
        
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginData.Token);

        var url = $"{_baseUrl}/{loginData.User.Id}";
        
        var resp = await client.GetAsync(url);

        Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
    }

    [Test]
    public async Task TestRefreshToken()
    {
        var client =  new Mesha_test_backend_application().CreateClient();
        await client.PostAsJsonAsync(_baseUrl, _user);

        var login = new LoginDto()
        {
            Email = _user.Email,
            Password = _user.Password
        };
        
        var respLogin = await client.PostAsJsonAsync(_loginUrl, login);
        var loginData = await respLogin.Content.ReadFromJsonAsync<ReadLoginDataDto>();
        
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginData.Token);

        var url = $"/auth/refresh";
        
        var tokenDto = new GetNewTokenDto()
        {
            Token = loginData.RefreshToken
        };
        
        var respRefreshToken = await client.PostAsJsonAsync(url, tokenDto);
        var refreshTokenData = await respRefreshToken.Content.ReadFromJsonAsync<ReadLoginDataDto>();

        Assert.AreNotEqual(loginData.Token, refreshTokenData.RefreshToken);
    }
    
    [Test]
    public async Task GetUsers()
    {
        var client =  new Mesha_test_backend_application().CreateClient();
        await client.PostAsJsonAsync(_baseUrl, _user);

        var login = new LoginDto()
        {
            Email = _user.Email,
            Password = _user.Password
        };
        
        var respLogin = await client.PostAsJsonAsync(_loginUrl, login);
        var loginData = await respLogin.Content.ReadFromJsonAsync<ReadLoginDataDto>();
        
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginData.Token);
        
        var resp = await client.GetAsync(_baseUrl);

        Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
    }

    [Test]
    public async Task TestUpdateUser()
    {
        var client =  new Mesha_test_backend_application().CreateClient();

        await client.PostAsJsonAsync(_baseUrl, _user);

        var login = new LoginDto()
        {
            Email = _user.Email,
            Password = _user.Password
        };
        
        var respLogin = await client.PostAsJsonAsync(_loginUrl, login);
        var loginData = await respLogin.Content.ReadFromJsonAsync<ReadLoginDataDto>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginData.Token);

        var url = $"{_baseUrl}/{loginData.User.Id}";

        var updateUser = new UpdateUserDto()
        {
            Name = "Updated",
            Lastname = _user.Lastname,
            Email = _user.Email,
            Password = _user.Password
        };

        var respUpdateUser = await client.PutAsJsonAsync(url, updateUser); 
        Assert.AreEqual(HttpStatusCode.Created, respUpdateUser.StatusCode);
    }

    [Test]
    public async Task TestDeleteUser()
    {
        var client =  new Mesha_test_backend_application().CreateClient();
        await client.PostAsJsonAsync(_baseUrl, _user);

        var login = new LoginDto()
        {
            Email = _user.Email,
            Password = _user.Password
        };
        
        var respLogin = await client.PostAsJsonAsync(_loginUrl, login);
        var loginData = await respLogin.Content.ReadFromJsonAsync<ReadLoginDataDto>();
        
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginData.Token);

        var url = $"{_baseUrl}/{loginData.User.Id}";
        
        var resp = await client.DeleteAsync(url);

        Assert.AreEqual(HttpStatusCode.NoContent, resp.StatusCode);
    }
    
}