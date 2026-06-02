using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UHabitacional.API.Application.Interfaces.Repositories;
using UHabitacional.API.Application.Interfaces.Services;
using UHabitacional.API.Application.Services;
using UHabitacional.API.Infrastructure.Data;
using UHabitacional.API.Infrastructure.Repositories;
using UHabitacional.API.Infrastructure.Security;
using UHabitacional.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// =====================================================================
// Configuración general
// =====================================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

// =====================================================================
// Entity Framework / SQL Server
// =====================================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// =====================================================================
// JWT
// =====================================================================
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("Jwt").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Mantener el mapeo de claims clásicos (sub -> NameIdentifier, role -> Role, etc.)
        // para compatibilidad con ClaimTypes en CurrentUserService.
        options.MapInboundClaims = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero,
            // Indicar explícitamente cuáles claims contienen el nombre y el rol
            // para que User.IsInRole(...) funcione correctamente.
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();

// =====================================================================
// Dependency Injection - Repositorios
// =====================================================================
builder.Services.AddScoped<IIdentificacionRepository, IdentificacionRepository>();
builder.Services.AddScoped<ITipoUsuarioRepository, TipoUsuarioRepository>();
builder.Services.AddScoped<IEdificioRepository, EdificioRepository>();
builder.Services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IInquilinoRepository, InquilinoRepository>();
builder.Services.AddScoped<IBitacoraVigilanteRepository, BitacoraVigilanteRepository>();
builder.Services.AddScoped<IBitacoraVisitanteRepository, BitacoraVisitanteRepository>();
builder.Services.AddScoped<ISesionRepository, SesionRepository>();

// =====================================================================
// Dependency Injection - Servicios
// =====================================================================
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IIdentificacionService, IdentificacionService>();
builder.Services.AddScoped<ITipoUsuarioService, TipoUsuarioService>();
builder.Services.AddScoped<IEdificioService, EdificioService>();
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IInquilinoService, InquilinoService>();
builder.Services.AddScoped<IBitacoraVigilanteService, BitacoraVigilanteService>();
builder.Services.AddScoped<IBitacoraVisitanteService, BitacoraVisitanteService>();
builder.Services.AddScoped<ISesionService, SesionService>();

// =====================================================================
// Swagger
// =====================================================================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "UHabitacional API",
        Version = "v1",
        Description = "API para la gestión integral de accesos en una unidad habitacional. " +
                      "Permite el registro de visitantes, consulta de información por parte de inquilinos " +
                      "y verificación rápida por parte de vigilantes.",
        Contact = new OpenApiContact { Name = "UHabitacional", Email = "soporte@uhabitacional.com" }
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Ingrese 'Bearer <token>' (sin las comillas).",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

// =====================================================================
// CORS (permitir conexiones desde el front-end de desarrollo)
// =====================================================================
builder.Services.AddCors(opts =>
{
    opts.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// =====================================================================
// Pipeline HTTP
// =====================================================================
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UHabitacional API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
// Middleware de validación de sesión: corre tras la autenticación JWT,
// verifica que el JTI exista como sesión activa y no expirada en BD,
// y actualiza el campo FechaUltimaActividad.
app.UseMiddleware<SessionValidationMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
