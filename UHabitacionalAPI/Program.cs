using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using UHabitacionalAPI.Application.Interfaces;
using UHabitacionalAPI.Application.Services;
using UHabitacionalAPI.Infrastructure.Contexts;
using UHabitacionalAPI.Infrastructure.Repositories;
using UHabitacionalAPI.Presentation.Dtos;
using UHabitacionalAPI.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IEdificiosRepository, EdificiosRepository>();
builder.Services.AddScoped<IEdificiosService, EdificiosService>();
builder.Services.AddScoped<IIdentificacionesRepository, IdentificacionesRepository>();
builder.Services.AddScoped<IIdentificacionesService, IdentificacionesService>();

builder.Services.AddControllers();

string connectionString = builder.Configuration.GetConnectionString("UnidadHabitacionalDB");
builder.Services.AddDbContext<UHabitacionalContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var response = ApiResponse<string>.Fail(StatusCodes.Status400BadRequest, "Petición malformada, verífique la información y vuelva a intentar.", errors);

        return new BadRequestObjectResult(response);
    };
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "UHabitacional API",
        Version = "v1",
        Description = "API para gestion de unidades habitacionales"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "UHabitacional API v1");
    c.RoutePrefix = string.Empty;
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
