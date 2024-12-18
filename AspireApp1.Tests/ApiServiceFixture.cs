using Aspire.Hosting;

namespace AspireApp1.Tests;

public class ApiServiceFixture : IAsyncLifetime
{
    public HttpClient HttpClient { get; private set; } = default!;

    private DistributedApplication app = default!;
    private ResourceNotificationService resourceNotificationService = default!;

    public async Task InitializeAsync()
    {
        // Arrange: Set up the Aspire application
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AspireApp1_AppHost>();

        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        app = await appHost.BuildAsync();
        await app.StartAsync();

        resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();

        // Wait for the API service to be up and running
        await resourceNotificationService.WaitForResourceAsync("apiservice", KnownResourceStates.Running)
            .WaitAsync(TimeSpan.FromSeconds(30));

        HttpClient = app.CreateHttpClient("apiservice");
    }

    public async Task DisposeAsync()
    {
        // Clean up the Aspire app
        await app.DisposeAsync();
    }
}
