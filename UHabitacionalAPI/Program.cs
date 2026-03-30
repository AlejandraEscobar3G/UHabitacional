using Microsoft.EntityFrameworkCore;
using UHabitacionalAPI.Application.Interfaces;
using UHabitacionalAPI.Application.Services;
using UHabitacionalAPI.Infrastructure.Contexts;
using UHabitacionalAPI.Infrastructure.Repositories;
using UHabitacionalAPI.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IEdificiosRepository, EdificiosRepository>();
builder.Services.AddScoped<IEdificiosService, EdificiosService>();

builder.Services.AddControllers();

string connectionString = builder.Configuration.GetConnectionString("UnidadHabitacionalDB");
builder.Services.AddDbContext<UHabitacionalContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1");
    c.RoutePrefix = string.Empty; // Para que se abra en la raíz (http://localhost:5000)
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
