using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
            var request = new HttpRequestMessage(HttpMethod.Post,
                    "/Playlists/AddTrackToPlaylist?playlistId=31&trackId=7")
                .AddTokenToHeaders(Client);
            await SimpleTest(request);
        }
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                    "/Playlists/RemoveTrackFromPlaylist?playlistId=31&trackId=7")
                .AddTokenToHeaders(Client);
            await SimpleTest(request);
        }
    }

    [Fact]
    public async Task LikePlaylist()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/Playlists/LikePlaylist?id=1")
            .AddTokenToHeaders(Client, Output);
        await SimpleTest(request);
    }

    [Fact]
    public async Task UnlikePlaylist()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/Playlists/UnlikePlaylist?id=1")
            .AddTokenToHeaders(Client, Output);
        await SimpleTest(request);
    }

    [Fact]
    public async Task Liked()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/Playlists/Liked?id=1")
            .AddTokenToHeaders(Client, Output);
        await SimpleTest(request);
    }

    [Fact]
    public async Task Create()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/Playlists/Create?id=1")
            .AddTokenToHeaders(Client, Output);
        await SimpleTest(request);
    }

    [Fact]
    public async Task DeleteSure()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/Playlists/DeleteSure?id=1")
            .AddTokenToHeaders(Client, Output);
        await SimpleTest(request);
    }

    [Fact]

    public async Task RemoveTrackFromPlaylist()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/Playlists/RemoveTrackFromPlaylist")
            .AddTokenToHeaders(Client, Output);
        await SimpleTest(request);
    }
}
