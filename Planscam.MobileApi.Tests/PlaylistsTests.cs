using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Planscam.MobileApi.Tests;

public class PlaylistsTests : TestBase
{
    public PlaylistsTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task All() => await SimpleTest("/Playlists/All");

    [Fact]
    public async Task Index() => await SimpleTest("Playlists/Index?id=1");

    [Fact]
    public async Task GetData() => await SimpleTest("Playlists/GetData?id=1");

    [Fact]
    public async Task FavouriteTracks()
    {
        var token = (string)((dynamic)JsonConvert.DeserializeObject(await GetToken())).access_token;
        var request = new HttpRequestMessage(HttpMethod.Get, "Playlists/FavoriteTracks");
        request.Headers.Add("Authorization", $"Bearer {token}");
        var response = await Client.SendAsync(request);
        await WriteResponseToOutput(response);
        response.StatusCodeIsOk();
    }

    private async Task<string> GetToken()
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
        return await response.Content.ReadAsStringAsync();
    }
}
