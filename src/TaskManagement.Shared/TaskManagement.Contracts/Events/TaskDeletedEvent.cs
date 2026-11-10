using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Contracts.Events
{
    /// <summary>
    /// Событие, публикуемое при удалении задачи.
    /// </summary>
    /// <param name="TaskId">Уникальный идентификатор удаленной задачи.</param>
    /// <param name="DeletedAt">Дата и время удаления задачи (UTC).</param>
    public record TaskDeletedEvent(
        Guid TaskId,
        DateTime DeletedAt,
        string? CorrelationId = null);
}
