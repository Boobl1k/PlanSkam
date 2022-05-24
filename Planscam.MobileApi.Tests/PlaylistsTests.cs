using System;
using System.Linq;
using System.Net.Http;
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
        Exception? exception = null;
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                    "/Playlists/AddTrackToPlaylist?playlistId=31&trackId=7")
                .AddTokenToHeaders(Client);
            await SimpleTest(request);
        }
        catch(Exception e)
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

    [Fact]
    public async Task Create()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/Playlists/Create")
            .AddTokenToHeaders(Client, Output);
        var content = new MultipartFormDataContent();
        content.Add(new StringContent("fff"), "Name");
        request.Content = content;
        await SimpleTest(request);
    }

    [Fact]
    public async Task DeleteSure()
    {
        //todo тут надо с начала создать плейлист запросом /Playlists/Create, получить из ответа айди,
        //например как сделано в AddTokenToHeaders, и в этом запросе указать этот айди
        var request = new HttpRequestMessage(HttpMethod.Post, $"/Playlists/DeleteSure?id=1")
            .AddTokenToHeaders(Client, Output);
        await SimpleTest(request);
    }

    [Fact]
    public async Task RemoveTrackFromPlaylist()
    {
        //todo тут надо указать айдишники и трека, и плейлиста
        var request = new HttpRequestMessage(HttpMethod.Post, "/Playlists/RemoveTrackFromPlaylist")
            .AddTokenToHeaders(Client, Output);
        await SimpleTest(request);
    }
}
