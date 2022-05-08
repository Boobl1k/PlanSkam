using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Planscam.MobileApi.Tests;

public class PlaylistsTests : TestBase
{
    public PlaylistsTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task All()
    {
        var response = await Client.GetAsync("/Playlists/All");
        response.StatusCodeIsOk();
        await WriteResponseToOutput(response);
    }

    [Fact]
    public async Task Index()
    {
        var response = await Client.GetAsync("Playlists/Index?id=1");
        response.StatusCodeIsOk();
        await WriteResponseToOutput(response);
    }
}
