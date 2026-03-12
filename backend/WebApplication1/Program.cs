using System.Text; // Para Encoding
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using ForgottenEmpires.Application.Services;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// CORS específica para nuestro entorno de desarrollo.
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000") // Solo permitimos este origen
                                .AllowAnyHeader()                     // Permitimos cualquier cabecera (Content-Type, etc.)
                                .AllowAnyMethod();                    // Permitimos cualquier método (GET, POST, etc.)
                      });
});
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

//  Swagger => Añadir un header de nombre Authorize con el siguiente valor Bearer
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("ApiBearerAuth", new OpenApiSecurityScheme //Github ConsultaAlumnos
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Acá pega el token generado al loguearse."
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

//Configuración de JWT
//Linea 52 a 54: Chequeos JWT de la Request
//Linea 55 a 58: Contra que comparamos para validar el token
builder.Services.AddAuthentication("Bearer") //Especifica que el esquema de autenticación es Bearer
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
                    builder.Configuration["Authentication:SecretForKey"]
                )
            )
        };
    });

// Configure DbContext with SQLite
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    // Obtenemos la cadena de conexión correcta del appsettings.json
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    // Configuramos EF Core para que use SQLite con esa cadena de conexión
    options.UseSqlite(connectionString);
});

//Age
builder.Services.AddScoped<IAgeRepository, AgeRepository>();
builder.Services.AddScoped<IAgeService, AgeService>();

//Civilization
builder.Services.AddScoped<ICivilizationRepository, CivilizationRepository>();
builder.Services.AddScoped<ICivilizationService, CivilizationService>();

// Battle
builder.Services.AddScoped<IBattleRepository, BatlleRepository>();
builder.Services.AddScoped<IBattleService, BattleService>();

//Character
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<ICharacterService, CharacterService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins); // Aplicamos política de CORS.

//Para que las Request pasen por el middleware de autenticación
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();