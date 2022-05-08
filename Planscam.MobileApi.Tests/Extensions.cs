using System.Net;
using System.Net.Http;
using Xunit;

namespace Planscam.MobileApi.Tests;

internal static class Extensions
{
    public static HttpResponseMessage StatusCodeIsOk(this HttpResponseMessage response)
    {
        Assert.True(response.StatusCode == HttpStatusCode.OK);
        return response;
    }
}
