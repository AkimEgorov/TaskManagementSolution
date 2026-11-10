using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Services;
using TaskManagement.Infrastructure.Consumers;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Infrastructure
{
    /// <summary>
    /// Регистрация зависимостей для Listener-сервиса.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddListenerInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Host=localhost;Port=5432;Database=taskmanagement;Username=app;Password=secret";

            services.AddDbContext<ListenerDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IEventLogRepository, EventLogRepository>();

            services.AddScoped<IEventLogService, EventLogService>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<TaskEventListener>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitHost = configuration["RabbitMq:Host"] ?? "localhost";
                    var rabbitUser = configuration["RabbitMq:Username"] ?? "guest";
                    var rabbitPass = configuration["RabbitMq:Password"] ?? "guest";

                    cfg.Host(rabbitHost, "/", h =>
                    {
                        h.Username(rabbitUser);
                        h.Password(rabbitPass);
                    });

                    cfg.ConfigureEndpoints(context);
                    cfg.UseMessageRetry(r => r.Interval(3, 1000));
                });
            });

            return services;
        }
    }
}
