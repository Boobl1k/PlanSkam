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
        Output.WriteLine(await response.Content.ReadAsStringAsync());
        response.StatusCodeIsOk();
    }

    [Fact]
    public async Task Search()
    {
        var response = await Client.GetAsync("/Authors/Search?Query=t");
        await WriteResponseToOutput(response);
        response.StatusCodeIsOk();
    }
}
