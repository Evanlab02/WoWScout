using Backend.Clients;
using Backend.Config;
using Backend.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Create configs
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
    IConfigurationSection blizzardConfig = builder.Configuration.GetSection("BlizzardAuth");
    var config = blizzardConfig.Get<BlizzardAuthConfig>() ?? throw new SystemException("Could not configure blizzard auth config.");
    builder.Services.AddSingleton(config);
}
else
{
    var blizzardConfig = new BlizzardAuthConfig
    {
        ClientId = Environment.GetEnvironmentVariable("BLIZZARD_CLIENT_ID") ?? "",
        ClientSecret = Environment.GetEnvironmentVariable("BLIZZARD_CLIENT_SECRET") ?? ""
    };
    if (string.IsNullOrEmpty(blizzardConfig.ClientId) || string.IsNullOrEmpty(blizzardConfig.ClientSecret))
    {
        throw new InvalidOperationException("ClientId or ClientSecret is not configured in production environment.");
    }
    builder.Services.AddSingleton(blizzardConfig);
}

// Register Clients
builder.Services.AddHttpClient<AuthClient>((serviceProvider, client) =>
{
    var blizzardConfig = serviceProvider.GetRequiredService<IOptions<BlizzardAuthConfig>>().Value;
    var logger = serviceProvider.GetRequiredService<ILogger<AuthClient>>();
    var authClient = new AuthClient(logger, client, blizzardConfig);
});

// Register services
builder.Services.AddScoped<AuthTokenService>();

// Run
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
