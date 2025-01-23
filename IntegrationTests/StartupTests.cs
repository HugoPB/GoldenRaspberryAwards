using Data.Repositories;
using Data;
using Domain.Interfaces;
using Domain.Validators;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Services;
using System.Reflection;
using Application.Controllers;

namespace IntegrationTests
{
    public class StartupTests
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DataContext>();
            services.AddScoped<IGoldenRaspberryRepository, GoldenRaspberryRepository>();
            services.AddScoped<IGoldenRaspberryService, GoldenRaspberryService>();
            services.AddSingleton<IValidator<GoldenRaspberryCSV>, GoldenRaspberryCSVValidator>();

            services.AddControllers();
            services.AddMvc().AddApplicationPart(typeof(GoldenRaspberryController).GetTypeInfo().Assembly);
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}