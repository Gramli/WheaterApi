using Microsoft.FeatureManagement;
using SmallApiToolkit.Extensions;
using SmallApiToolkit.Middleware;
using Weather.API.EndpointBuilders;
using Weather.Core.Configuration;
using Weather.Domain.FeatureFlags;
using Weather.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCore();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var corsPolicyName = builder.Services.AddCorsByConfiguration(builder.Configuration);

builder.Services.AddFeatureManagement(builder.Configuration.GetSection(FeatureFlagKeys.FeatureFlagsKey));

/*builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect("")
    .UseFeatureFlags(featureFlagOptions =>
    {
        featureFlagOptions.SetRefreshInterval(TimeSpan.FromMinutes(1));
    });
});*/

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicyName);

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app.BuildWeatherEndpoints();

app.Run();