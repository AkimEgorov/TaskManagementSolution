using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Contracts.Events
{
    /// <summary>
    /// Событие, публикуемое при обновлении задачи.
    /// </summary>
    /// <param name="TaskId">Уникальный идентификатор задачи.</param>
    /// <param name="Title">Новое название задачи.</param>
    /// <param name="Description">Новое описание задачи.</param>
    /// <param name="Status">Новый статус задачи (строковое представление).</param>
    /// <param name="UpdatedAt">Дата и время обновления задачи (UTC).</param>
    public record TaskUpdatedEvent(
        Guid TaskId,
        string Title,
        string? Description,
        string Status,
        DateTime UpdatedAt,
        string? CorrelationId = null);
}
