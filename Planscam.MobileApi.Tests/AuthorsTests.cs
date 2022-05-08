using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Planscam.MobileApi.Tests;

public class AuthorsTests : TestBase
{
    public AuthorsTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Index()
    {
        var response =
            await Client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/Authors/Index?id=1"));

        response.StatusCodeIsOk();
        Output.WriteLine(await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Search()
    {
        var response = await Client.GetAsync("/Authors/Search?Query=t");
        response.StatusCodeIsOk();
        await WriteResponseToOutput(response);
    }
}
