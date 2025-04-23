using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using GestionReserva.Application.Handlers;
using GestionReserva.Core.Interfaces;
using GestionReserva.Infrastructure.Persistence;
using GestionReserva.Infrastructure.Repositories;
using GestionReserva.Infrastructure.Services;
using System.Text.Json.Serialization;  

namespace GestionReserva.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            ConfigureApp(app);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Controladores de API + enum como string
            services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            //  Swagger / OpenAPI
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GestionReserva API", Version = "v1" });
            });

            //  EF Core – InMemory
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("GestionReservaDb"));

            //  MediatR (registra todos los handlers del ensamblado Application)
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(typeof(CrearReservaHandler).Assembly)
            );

            //  Repositorios y Unit of Work
            services.AddScoped<IReservaRepository, ReservaRepository>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

            // Adaptadores Mock de servicios externos
            services.AddScoped<IProveedorService, MockProveedorServiceAdapter>();
            services.AddScoped<IServicioPagos, MockServicioPagosAdapter>();
        }

        private static void ConfigureApp(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                // Muestra la página de diagnóstico de excepciones completas
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestionReserva API V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
