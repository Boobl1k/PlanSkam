using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Planscam.MobileApi.Tests;

public class TracksTests : TestBase
{
    public TracksTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task Index() =>
        await SimpleTest("/Tracks/Index?id=1");

    [Fact]
    public async Task Search_byTracks() =>
        await SimpleTest("/Tracks/Search?Query=t&Page=1&byAuthors=false");

    [Fact]
    public async Task Search_byAuthors() =>
        await SimpleTest("/Tracks/Search?Query=t&Page=1&byAuthors=true");

    [Fact]
    public async Task GetTrackData() =>
        await SimpleTest("/Tracks/GetTrackData?id=1");
}
