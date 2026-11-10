using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Dto;

namespace TaskManagement.Application.Services
{
    /// <summary>
    /// Интерфейс сервиса управления задачами.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>Создает новую задачу и публикует событие о её создании.</summary>
        Task<TaskResponse> CreateAsync(CreateTaskRequest request, CancellationToken cancellationToken = default);

        /// <summary>Получает задачу по идентификатору.</summary>
        Task<TaskResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>Получает список всех задач.</summary>
        Task<IReadOnlyList<TaskResponse>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>Обновляет существующую задачу и публикует событие об этом.</summary>
        Task<TaskResponse?> UpdateAsync(Guid id, UpdateTaskRequest request, CancellationToken cancellationToken = default);

        /// <summary>Удаляет задачу по идентификатору и публикует событие об этом.</summary>
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
