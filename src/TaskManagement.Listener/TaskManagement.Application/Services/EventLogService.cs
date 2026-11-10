using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Enums;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Models;

namespace TaskManagement.Application.Services
{
    /// <summary>
    /// Сервис для управления историей событий задач.
    /// </summary>
    public sealed class EventLogService : IEventLogService
    {
        private readonly IEventLogRepository _repository;

        public EventLogService(IEventLogRepository repository)
        {
            _repository = repository;
        }

        /// <summary>Сохраняет событие о задаче.</summary>
        public async Task SaveEventAsync(Guid taskId, TaskEventType eventType, string eventData, CancellationToken cancellationToken = default)
        {
            var log = new TaskEventLog
            {
                TaskId = taskId,
                EventType = eventType.ToString(),
                EventData = eventData,
                Timestamp = DateTime.UtcNow
            };

            await _repository.SaveAsync(log, cancellationToken);
        }

        /// <summary>Получает историю событий для задачи.</summary>
        public async Task<IReadOnlyList<TaskEventLog>> GetEventsByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default)
        {
            return await _repository.GetByTaskIdAsync(taskId, cancellationToken);
        }

        /// <summary>Получает все события.</summary>
        public async Task<IReadOnlyList<TaskEventLog>> GetAllEventsAsync(int limit = 100, CancellationToken cancellationToken = default)
        {
            return await _repository.GetAllAsync(limit, cancellationToken);
        }
    }
}
