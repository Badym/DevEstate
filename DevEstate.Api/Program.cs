using DevEstate.Api.Models;
using DevEstate.Api.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DevEstate.Api.Services;
using Microsoft.Extensions.FileProviders;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
// ------------------ JWT ------------------

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton<JwtService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

// ------------------ CORS (‼️ DODAJ TO) ------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// ------------------ Services ------------------
// (Dodamy później np. UserService, PropertyService, DatabaseSeeder itp.)
builder.Services.AddSingleton<TestService>();

builder.Services.AddSingleton<CompanyRepository>();
builder.Services.AddSingleton<InvestmentRepository>();
builder.Services.AddSingleton<BuildingRepository>();
builder.Services.AddSingleton<PropertyRepository>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<FeatureRepository>();
builder.Services.AddSingleton<PriceHistoryRepository>();
builder.Services.AddSingleton<DocumentRepository>();

builder.Services.AddSingleton<CompanyService>();
builder.Services.AddSingleton<InvestmentService>();
builder.Services.AddSingleton<BuildingService>();
builder.Services.AddSingleton<PropertyService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<FeatureService>();
builder.Services.AddSingleton<PriceHistoryService>();
builder.Services.AddSingleton<DocumentService>();

// DataSeeder
builder.Services.AddSingleton<DataSeeder>();

// ------------------ Controllers ------------------

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// ------------------ Swagger ------------------

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "DevEstate API", Version = "v1" });

    // 🔐 Dodaj sekcję Bearer Auth do Swaggera
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Podaj token jako: **Bearer {token}**",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ------------------ Uploads (Static Files) ------------------

var documentPath = Path.Combine(builder.Environment.ContentRootPath, "Uploads", "Documents");
Directory.CreateDirectory(documentPath);

var imagePath = Path.Combine(builder.Environment.ContentRootPath, "Uploads", "Images");
Directory.CreateDirectory(imagePath);

// ------------------ Authorization ------------------
// (Na razie puste, dodamy później role/polityki JWT)

// ------------------ Build ------------------

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

// ------------------ Seed danych ------------------
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

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

app.UseCors("AllowFrontend");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(documentPath),
    RequestPath = "/uploads/documents"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagePath),
    RequestPath = "/uploads/images"
});

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();