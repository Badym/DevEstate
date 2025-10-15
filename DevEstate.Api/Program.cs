using DevEstate.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DevEstate.Api.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ------------------ MongoDB ------------------

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// ------------------ JWT ------------------
// (Dodamy później po konfiguracji JwtSettings i JwtService)

// ------------------ Services ------------------
// (Dodamy później np. UserService, PropertyService, DatabaseSeeder itp.)
builder.Services.AddSingleton<TestService>();

// ------------------ Controllers ------------------

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// ------------------ Swagger ------------------

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "DevEstate API", Version = "v1" });
});

// ------------------ Authorization ------------------
// (Na razie puste, dodamy później role/polityki JWT)

// ------------------ Build ------------------

var app = builder.Build();

// ------------------ Seed danych ------------------
// (Dodamy po stworzeniu DatabaseSeeder)

// ------------------ Middleware ------------------

if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevEstate API v1");
        c.RoutePrefix = "swagger"; // /swagger zamiast root
    });
}

app.UseHttpsRedirection();

// app.UseAuthentication(); // (na razie wyłączone)
// app.UseAuthorization();

app.MapControllers();

app.Run();