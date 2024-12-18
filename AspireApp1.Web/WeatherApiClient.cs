namespace AspireApp1.Web;

public class WeatherApiClient(HttpClient httpClient)
{
    private const string ApiKey = "my-secure-api-key"; // Store securely in configuration

    public async Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/weatherforecast");
        request.Headers.Add("x-api-key", ApiKey); // Add the API key header

        var forecasts = new List<WeatherForecast>();

        using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            await foreach (var forecast in response.Content.ReadFromJsonAsAsyncEnumerable<WeatherForecast>(cancellationToken: cancellationToken))
            {
                if (forecasts.Count >= maxItems)
                {
                    break;
                }

                if (forecast is not null)
                {
                    forecasts.Add(forecast);
                }
            }
        }
        else
        {
            throw new HttpRequestException($"Error: {response.StatusCode}");
        }

        return forecasts.ToArray();
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
