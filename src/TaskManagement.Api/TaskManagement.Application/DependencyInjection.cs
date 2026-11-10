using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Services;
using TaskManagement.Application.Validators;

namespace TaskManagement.Application
{
    /// <summary>
    /// Методы расширения для регистрации сервисов Application-слоя в DI-контейнере.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Регистрирует сервисы, валидаторы и другие зависимости Application-слоя.
        /// </summary>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ITaskService, TaskService>();

            // Регистрация всех валидаторов из сборки, где находится CreateTaskValidator
            services.AddValidatorsFromAssemblyContaining<CreateTaskValidator>();

            return services;
        }
    }
}
