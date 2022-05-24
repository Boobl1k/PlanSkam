using System;
using System.Linq;
using System.Net.Http;
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
        var request = new HttpRequestMessage(HttpMethod.Get, "Playlists/FavoriteTracks")
            .AddTokenToHeaders(Client, Output);
        Output.WriteLine(request.Headers.First().Key + " " + request.Headers.First().Value.First());
        await SimpleTest(request);
    }

    [Fact]
    public async Task AddTrackToPlaylist()
    {
        Exception? exception = null;
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                    "/Playlists/AddTrackToPlaylist?playlistId=31&trackId=7")
                .AddTokenToHeaders(Client);
            await SimpleTest(request);
        }
        catch (Exception e)
        {
            exception = e;
        }

        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                    "/Playlists/RemoveTrackFromPlaylist?playlistId=31&trackId=7")
                .AddTokenToHeaders(Client);
            await SimpleTest(request);
        }
        if (exception is { })
            throw exception;
    }

    [Fact]
    public async Task LikeAndUnlikePlaylist()
    {
        Exception? exception = null;
        try
        {
            var likeRequest = new HttpRequestMessage(HttpMethod.Post, "/Playlists/LikePlaylist?id=1")
                .AddTokenToHeaders(Client, Output);
            await SimpleTest(likeRequest);
        }
        catch (Exception e)
        {
            exception = e;
        }

        var unlikeRequest = new HttpRequestMessage(HttpMethod.Post, "/Playlists/UnlikePlaylist?id=1")
            .AddTokenToHeaders(Client, Output);
        await SimpleTest(unlikeRequest);
        if (exception is { })
            throw exception;
    }

    [Fact]
    public async Task Liked()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/Playlists/Liked")
            .AddTokenToHeaders(Client, Output);
        await SimpleTest(request);
    }

    private async Task<HttpResponseMessage> Create()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/Playlists/Create")
            .AddTokenToHeaders(Client, Output);
        var content = new MultipartFormDataContent();
        content.Add(new StringContent("fff"), "Name");
        request.Content = content;
        return await SimpleTest(request);
    }

    [Fact]
    public async Task CreateAndDeleteSure()
    {
        var createResponse = await Create();
        int id = (JsonConvert.DeserializeObject(await createResponse.Content.ReadAsStringAsync()) as dynamic).id;
        var request = new HttpRequestMessage(HttpMethod.Post, $"/Playlists/DeleteSure?id={id}")
            .AddTokenToHeaders(Client, Output);
        await SimpleTest(request);
    }

    [Fact]
    public async Task AddAndRemoveTrackFromPlaylist()
    {
        var addRequest =
            new HttpRequestMessage(HttpMethod.Post, "Playlists/AddTrackToPlaylist?playlistId=31&trackId=2")
                .AddTokenToHeaders(Client);
        await SimpleTest(addRequest);
        var removeRequest =
            new HttpRequestMessage(HttpMethod.Post, "/Playlists/RemoveTrackFromPlaylist?playlistId=31&trackId=2")
                .AddTokenToHeaders(Client, Output);
        await SimpleTest(removeRequest);
    }
}
