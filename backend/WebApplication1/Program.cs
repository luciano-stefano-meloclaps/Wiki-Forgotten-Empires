using System.Collections.Generic;
using System.IO;
using System.Text; // Para Encoding
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using ForgottenEmpires.Application.Services;
using ForgottenEmpire.HostedServices;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

LoadLocalEnvironmentOverrides(builder.Configuration, builder.Environment.ContentRootPath);

// 🔒 SEGURIDAD: CORS configurado desde appsettings para mayor flexibilidad
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // Leer orígenes permitidos desde configuración
                          var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:3000" };

                          policy.WithOrigins(allowedOrigins)
                                .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS") // Métodos específicos en lugar de AllowAnyMethod
                                .WithHeaders("Content-Type", "Authorization", "Accept") // Headers específicos en lugar de AllowAnyHeader
                                .AllowCredentials(); // Solo si es necesario para autenticación
                      });
});
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient<INotionSyncService, NotionSyncService>(client =>
{
    client.BaseAddress = new Uri("https://api.notion.com/v1/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

//  Swagger => A�adir un header de nombre Authorize con el siguiente valor Bearer
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("ApiBearerAuth", new OpenApiSecurityScheme //Github ConsultaAlumnos
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Ac� pega el token generado al loguearse."
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiBearerAuth" // AddSecurityDefinition
                }
            },
            new List<string>()
        }
    });
});

//Configuraci�n de JWT
//Linea 52 a 54: Chequeos JWT de la Request
//Linea 55 a 58: Contra que comparamos para validar el token
builder.Services.AddAuthentication("Bearer") //Especifica que el esquema de autenticaci�n es Bearer
    .AddJwtBearer(options =>// Configura de la autenicacion JWT
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(
                    builder.Configuration["Authentication:SecretForKey"] ?? string.Empty
                )
            )
        };
    });

// Register NotionDataStore as singleton for in-memory data storage
builder.Services.AddSingleton<Domain.Interfaces.INotionDataStore, Application.Services.NotionDataStore>();

//Age
builder.Services.AddScoped<IAgeRepository, AgeRepository>();
builder.Services.AddScoped<IAgeService, AgeService>();

//Civilization
builder.Services.AddScoped<ICivilizationRepository, CivilizationRepository>();
builder.Services.AddScoped<ICivilizationService, CivilizationService>();
builder.Services.AddScoped<ITerritoryRepository, TerritoryRepository>();

// Battle
builder.Services.AddScoped<IBattleRepository, BatlleRepository>();
builder.Services.AddScoped<IBattleService, BattleService>();

//Character
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<ICharacterService, CharacterService>();

// Statistics
builder.Services.AddScoped<IStatisticsService, StatisticsService>();

builder.Services.AddHostedService<NotionSyncHostedService>();

var app = builder.Build();

// Validate Notion configuration at startup
using (var scope = app.Services.CreateScope())
{
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    if (!Domain.NotionConfiguration.IsConfigured(key => config[key]))
    {
        throw new InvalidOperationException(
            "STARTUP ERROR: Notion integration is not configured. " +
            "Set Notion:Secret and at least one DatabaseId in appsettings.json or environment variables."
        );
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTP-only in Docker — HTTPS is handled by a reverse proxy upstream
// app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins); // Aplicamos pol�tica de CORS.

//Para que las Request pasen por el middleware de autenticaci�n
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

static void LoadLocalEnvironmentOverrides(ConfigurationManager configuration, string contentRootPath)
{
    var envFilePath = Path.Combine(contentRootPath, ".env.local");
    if (!File.Exists(envFilePath))
    {
        return;
    }

    var overrides = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
    foreach (var line in File.ReadAllLines(envFilePath))
    {
        var trimmed = line.Trim();
        if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#", StringComparison.Ordinal))
        {
            continue;
        }

        var separatorIndex = trimmed.IndexOf('=');
        if (separatorIndex <= 0)
        {
            continue;
        }

        var key = trimmed.Substring(0, separatorIndex).Trim();
        var value = trimmed.Substring(separatorIndex + 1).Trim().Trim('"');
        if (string.IsNullOrEmpty(key) || value is null)
        {
            continue;
        }

        overrides[key] = value;
    }

    if (overrides.Count > 0)
    {
        configuration.AddInMemoryCollection(overrides);
    }
}

