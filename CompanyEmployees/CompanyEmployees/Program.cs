using CompanyEmployees.Configuration;
using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using NLog;

var builder = WebApplication.CreateBuilder(args);
// Retrieve the connection string
var connectionString = builder.Configuration.GetConnectionString("AppConfig");
// Load configuration from Azure App Configuration
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(connectionString)
        // Load all keys that start with `TestApp:` and have no label
        .Select("TestApp:*", LabelFilter.Null)
        // Configure to reload configuration if the registered sentinel key is modified
        .ConfigureRefresh(refreshOptions =>
            refreshOptions
                .Register("TestApp:Settings:Sentinel", refreshAll: true)
                .SetCacheExpiration(TimeSpan.FromMinutes(2)));
});
// Add Azure App Configuration middleware to the container of services.
builder.Services.AddAzureAppConfiguration();
// Bind configuration "TestApp:Settings" section to the Configuration object
builder.Services.Configure<Configuration>(builder.Configuration.GetSection("TestApp:Settings"));

// Add services to the container.
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

// Use Azure App Configuration middleware for dynamic configuration refresh.
app.UseAzureAppConfiguration();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();