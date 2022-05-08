using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Planscam.MobileApi.Tests;

public abstract class TestBase
{
    protected readonly HttpClient Client;
    protected readonly ITestOutputHelper Output;

    protected TestBase(ITestOutputHelper output)
    {
        Output = output;
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json").AddEnvironmentVariables().Build();
        Client = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
                builder.ConfigureAppConfiguration((_, b) =>
                    b.AddJsonFile("appsettings.Test.json")))
            .CreateClient();
    }

    protected async Task WriteResponseToOutput(HttpResponseMessage response) => 
        Output.WriteLine(await response.Content.ReadAsStringAsync());
}
