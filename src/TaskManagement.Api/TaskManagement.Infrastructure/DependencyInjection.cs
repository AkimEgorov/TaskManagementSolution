using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Messaging;

namespace TaskManagement.Infrastructure
{
    /// <summary>
    /// Методы расширения для регистрации зависимостей Infrastructure-слоя.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Host=localhost;Port=5432;Database=taskmanagement;Username=app;Password=secret";

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IEventPublisher, MassTransitEventPublisher>();

            services.AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<AppDbContext>(o =>
                {
                    o.UsePostgres();
                    o.UseBusOutbox();
                });

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

                    cfg.UseMessageRetry(r => r.Intervals(100, 500, 1000));
                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
