using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Contracts.Events
{
    /// <summary>
    /// Событие, публикуемое при создании новой задачи.
    /// </summary>
    /// <param name="TaskId">Уникальный идентификатор задачи.</param>
    /// <param name="Title">Название задачи.</param>
    /// <param name="Description">Описание задачи.</param>
    /// <param name="Status">Статус задачи (строковое представление).</param>
    /// <param name="CreatedAt">Дата и время создания задачи (UTC).</param>
    public record TaskCreatedEvent(
        Guid TaskId,
        string Title,
        string? Description,
        string Status,
        DateTime CreatedAt,
        string? CorrelationId = null);
}
