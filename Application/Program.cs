using Data;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Validators;
using Services;
using FluentValidation;
using Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DataContext>();
builder.Services.AddScoped<IGoldenRaspberryRepository, GoldenRaspberryRepository>();
builder.Services.AddScoped<IGoldenRaspberryService, GoldenRaspberryService>();
builder.Services.AddSingleton<IValidator<GoldenRaspberryCSV>, GoldenRaspberryCSVValidator>();

builder.Services.AddCors();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("../swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();