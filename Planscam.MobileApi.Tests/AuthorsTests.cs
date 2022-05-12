using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Planscam.MobileApi.Tests;

public class AuthorsTests : TestBase
{
    public AuthorsTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task Index() => await SimpleTest("/Authors/Index?id=1");

    [Fact]
    public async Task Search() => await SimpleTest("/Authors/Search?Query=t");
}
