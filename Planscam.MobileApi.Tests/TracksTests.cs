using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Planscam.MobileApi.Tests;

public class TracksTests : TestBase
{
    public TracksTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Index()
    {
        var response = await Client.GetAsync("/Tracks/Index?id=1");
        await WriteResponseToOutput(response);
        response.StatusCodeIsOk();
    }

    [Fact]
    public async Task Search_byTracks()
    {
        var response = await Client.GetAsync("/Tracks/Search?Query=t&Page=1&byAuthors=false");
        await WriteResponseToOutput(response);
        response.StatusCodeIsOk();
    }
    
    [Fact]
    public async Task Search_byAuthors()
    {
        var response = await Client.GetAsync("/Tracks/Search?Query=t&Page=1&byAuthors=true");
        await WriteResponseToOutput(response);
        response.StatusCodeIsOk();
    }
    
    [Fact]
    public async Task GetTrackData()
    {
        var response = await Client.GetAsync("/Tracks/GetTrackData?id=1");
        await WriteResponseToOutput(response);
        response.StatusCodeIsOk();
    }
}
