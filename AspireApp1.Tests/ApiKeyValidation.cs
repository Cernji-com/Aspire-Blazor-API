using System.Net;
using Xunit;

namespace AspireApp1.Tests;

public class ApiKeyValidationTests : IClassFixture<ApiServiceFixture>
{
    private readonly HttpClient _httpClient;

    public ApiKeyValidationTests(ApiServiceFixture fixture)
    {
        _httpClient = fixture.HttpClient;
    }

    [Fact]
    public async Task ApiService_WithValidApiKey_ReturnsOk()
    {
        // Act
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/weatherforecast");
        request.Headers.Add("x-api-key", "my-secure-api-key"); // Valid API Key

        var response = await _httpClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ApiService_WithoutApiKey_ReturnsUnauthorized()
    {
        // Act
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/weatherforecast");
        // No API Key added

        var response = await _httpClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ApiService_WithInvalidApiKey_ReturnsUnauthorized()
    {
        // Act
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/weatherforecast");
        request.Headers.Add("x-api-key", "invalid-api-key"); // Invalid API Key

        var response = await _httpClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
