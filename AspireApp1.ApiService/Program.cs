using AspireApp1.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Custom API Key Middleware
app.Use(async (context, next) =>
{
    var path = context.Request.Path;

    // Skip API Key validation for OpenAPI endpoints and Aspire Dashboard
    if (path.StartsWithSegments("/swagger") || path.StartsWithSegments("/openapi") ||
        path.StartsWithSegments("/_structured"))
    {
        await next();
        return;
    }
    var apiKey = context.Request.Headers["x-api-key"];
    var validApiKey = builder.Configuration["ApiKeys:DefaultKey"];

    if (string.IsNullOrEmpty(apiKey) || apiKey != validApiKey)
    {
        context.Response.StatusCode = 401; // Unauthorized
        await context.Response.WriteAsync("Invalid or missing API Key");
        return;
    }

    await next();
});

app.MapDefaultEndpoints();
app.MapControllers();
app.Run();
