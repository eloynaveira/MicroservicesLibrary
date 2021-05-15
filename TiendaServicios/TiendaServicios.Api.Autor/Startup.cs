using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.ManejadorRabbit;
using TiendaServicios.Api.Autor.Persistencia;
using TiendaServicios.Api.Autor.Servicios;
using TiendaServicios.Mensajeria.Lib.SendGridLib.Impl;
using TiendaServicios.Mensajeria.Lib.SendGridLib.Interface;
using TiendaServicios.RabbitMQ.Lib.BusRabbit;
using TiendaServicios.RabbitMQ.Lib.EventQueue;
using TiendaServicios.RabbitMQ.Lib.Impl;

namespace TiendaServicios.Api.Autor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEventHandler<EmailEventQueue>, EmailEventHandler>();

            services.AddSingleton<IRabbitEventBus, RabbitEventBus>( sp => {
                var scopefactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitEventBus(sp.GetService<IMediator>(), scopefactory);
            });

            services.AddSingleton<ISendGridEmail, SendGridEmail>();

            services.AddTransient<EmailEventHandler>();
            
            services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            services.AddDbContext<AutorContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("ConexionDatabaseDocker"));
            });

            services.AddMediatR(typeof(Nuevo.Manejador).Assembly);
            services.AddAutoMapper(typeof(Consulta.Manejador));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var eventBus = app.ApplicationServices.GetRequiredService<IRabbitEventBus>();
            eventBus.Subscribe<EmailEventQueue, EmailEventHandler>();
        }
    }
}
