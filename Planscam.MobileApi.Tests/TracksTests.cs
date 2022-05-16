using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.FSharp.Core;
using Xunit;
using Xunit.Abstractions;

namespace Planscam.MobileApi.Tests;

public class TracksTests : TestBase
{
    public TracksTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task Index() =>
        await SimpleTest("/Tracks/Index?id=2");

    [Fact]
    public async Task Search_byTracks() =>
        await SimpleTest("/Tracks/Search?Query=t&Page=1&byAuthors=false");

    [Fact]
    public async Task Search_byAuthors() =>
        await SimpleTest("/Tracks/Search?Query=t&Page=1&byAuthors=true");

    [Fact]
    public async Task GetTrackData() =>
        await SimpleTest("/Tracks/GetTrackData?id=2");

    [Fact]
    public async Task AddTrackToFavourite()
    {
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "Tracks/AddTrackToFavourite?id=8")
                .AddTokenToHeaders(Client, Output);
            await SimpleTest(request);
        }
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "Tracks/RemoveTrackFromFavourite?id=8")
                .AddTokenToHeaders(Client, Output);
            await SimpleTest(request);
        }
    }
}
