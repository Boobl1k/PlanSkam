using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
        var request = new HttpRequestMessage(HttpMethod.Get, "Playlists/FavoriteTracks")
            .AddTokenToHeaders(Client, Output);
        Output.WriteLine(request.Headers.First().Key + " " + request.Headers.First().Value.First());
        await SimpleTest(request);
    }

    [Fact]
    public async Task AddTrackToPlaylist()
    {
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/Playlists/AddTrackToPlaylist")
                .AddTokenToHeaders(Client);
            request.Headers.Add("playlistId", "1");
            request.Headers.Add("trackId", "7");
            await SimpleTest(request);
        }
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/Playlists/RemoveTrackFromPlaylist")
                .AddTokenToHeaders(Client);
            request.Headers.Add("playlistId", "1");
            request.Headers.Add("trackId", "7");
            await SimpleTest(request);
        }
    }
}
