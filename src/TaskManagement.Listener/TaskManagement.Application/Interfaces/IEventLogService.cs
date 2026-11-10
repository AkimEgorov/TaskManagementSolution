using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Enums;
using TaskManagement.Application.Models;

namespace TaskManagement.Application.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для управления историей событий задач.
    /// </summary>
    public interface IEventLogService
    {
        /// <summary>Сохраняет событие о задаче.</summary>
        /// <param name="taskId">Идентификатор задачи.</param>
        /// <param name="eventType">Тип события (Created, Updated, Deleted).</param>
        /// <param name="eventData">JSON-представление данных события.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task SaveEventAsync(Guid taskId, TaskEventType eventType, string eventData, CancellationToken cancellationToken = default);

        /// <summary>Получает историю событий для конкретной задачи.</summary>
        /// <param name="taskId">Идентификатор задачи.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список событий, отсортированных по времени (от новых к старым).</returns>
        Task<IReadOnlyList<TaskEventLog>> GetEventsByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default);

        /// <summary>Получает все события (последние N записей).</summary>
        /// <param name="limit">Максимальное количество записей.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список последних событий.</returns>
        Task<IReadOnlyList<TaskEventLog>> GetAllEventsAsync(int limit = 100, CancellationToken cancellationToken = default);
    }
}
