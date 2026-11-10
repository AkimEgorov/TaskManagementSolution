using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Models;

namespace TaskManagement.Application.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с историей событий задач.
    /// </summary>
    public interface IEventLogRepository
    {
        /// <summary>Сохраняет событие в хранилище.</summary>
        Task SaveAsync(TaskEventLog eventLog, CancellationToken cancellationToken = default);

        /// <summary>Получает историю событий для конкретной задачи.</summary>
        Task<IReadOnlyList<TaskEventLog>> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default);

        /// <summary>Получает все события (последние N записей).</summary>
        Task<IReadOnlyList<TaskEventLog>> GetAllAsync(int limit = 100, CancellationToken cancellationToken = default);
    }
}
