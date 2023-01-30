using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

var config = new {
    Instance = "https://login.microsoftonline.com/",
    ClientId = "47285f80-f9c8-4f5f-a99c-cc71347bff26",
    TenantId = "24adaeaa-5002-4f6e-aa57-b66c036ba791",
    Scopes = new  string[] { "Global.API" },
};
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(config);
var scopes = config.Scopes;
// Adding authorization policies that enforce authorization using Azure AD roles.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DefaultScope",
        policy =>
            policy
            .RequireAuthenticatedUser()
            .RequireScope(scopes));
});
builder.Services.AddRequiredScopeAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


app.MapGet("/weatherforecast", [Authorize]() =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

