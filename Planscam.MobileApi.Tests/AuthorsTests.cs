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
    public async Task Test1()
    {
        var response =
            await Client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/Authors/Index?id=1"));

        //StatusCodeIsOk(response);
        Output.WriteLine(await response.Content.ReadAsStringAsync());
    }

    
}
