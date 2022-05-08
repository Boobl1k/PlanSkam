using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Planscam.MobileApi.Tests;

public class ProfileTests : TestBase
{
    public ProfileTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Index()
    {
        var response = await Client.GetAsync("Profile/Index?id=7f425417-8246-4d8c-b118-10fe2fcf7a9e");
        response.StatusCodeIsOk();
        await WriteResponseToOutput(response);
    }
    
}
