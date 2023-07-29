using System.Net.Http.Headers;
using System.Text;
using mesha_test_backend.Data.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace mesha_test_backend.test;

[TestClass]
public class UnitTest1
{
    
    [TestMethod]
    public async Task TestCreatUser()
    {
        // var uri = new Uri("https://localhost:44306/users/ok");
        var webAppFactory = new WebApplicationFactory<Program>();
        var httpClient = webAppFactory.CreateDefaultClient();

        var response = await httpClient.GetAsync("https://localhost:44306/users/ok");
        Assert.AreEqual(200, response.StatusCode);

        // var user = new CreateUserDto()
        // {
        //     Name = "Test",
        //     Lastname = "User",
        //     Email = "unit@tests.com",
        //     Password = "123456789"
        // };

        // var payload = "";
        // var content = new StringContent("", Encoding.UTF8, "application/json");
        // var response = await httpClient.PostAsync(uri, content);
        // Assert.AreEqual(400, response.StatusCode);
    }

    [TestMethod]
    public void TestSomething()
    {
        Assert.IsTrue(true);
    }
}