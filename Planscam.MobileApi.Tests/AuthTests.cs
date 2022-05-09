using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Planscam.MobileApi.Tests;

public class AuthTests : TestBase
{
    public AuthTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Login()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/Auth/Login");
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"grant_type", "password"},
            {"username", "qwe"},
            {"password", "qweQWE123!"}
        });
        var response = await Client.SendAsync(request);
        await WriteResponseToOutput(response);
        response.StatusCodeIsOk();
    }
}
