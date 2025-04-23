using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Core.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Adapters;
using Infrastructure.Services;
using Application.Handlers;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ReservaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 1. Agregar controladores
builder.Services.AddControllers();

// 2. Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Registrar dependencias (Inyección de dependencias)
builder.Services.AddSingleton<IReservaRepository, ReservaRepository>();
builder.Services.AddSingleton<IProveedorAdapter, ProveedorHotelAdapter>();
// Si tienes más adaptadores, agrégalos aquí:
builder.Services.AddSingleton<IProveedorAdapter, ProveedorVueloAdapter>();
builder.Services.AddSingleton<IProveedorAdapter, ProveedorTourAdapter>();

builder.Services.AddSingleton<IPagoService, PagoService>();

// Handler: puedes registrarlo como Transient o Scoped
builder.Services.AddTransient<CrearReservaHandler>();

var app = builder.Build();

// 4. Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();