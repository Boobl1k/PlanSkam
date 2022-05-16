using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Planscam.MobileApi.Tests;

public class ProfileTests : TestBase
{
    public ProfileTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task Index() =>
        await SimpleTest("Profile/Index?id=7f425417-8246-4d8c-b118-10fe2fcf7a9e");

    [Fact]
    public async Task Edit()
    {
        //todo тут надо залезть в модель, которую принимает метод, понять что там нужно добавить, и поменять чисто почту
        var request = new HttpRequestMessage(HttpMethod.Post, "/Profile/Edit")
            .AddTokenToHeaders(Client, Output);
        await SimpleTest(request);
    }
}
