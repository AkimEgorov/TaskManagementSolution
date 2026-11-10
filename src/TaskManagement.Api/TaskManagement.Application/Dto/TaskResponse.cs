using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Dto
{
    /// <summary>
    /// DTO для возврата информации о задаче клиенту.
    /// </summary>
    public sealed class TaskResponse
    {
        /// <summary>Уникальный идентификатор задачи.</summary>
        public Guid Id { get; init; }

        /// <summary>Название задачи.</summary>
        public string Title { get; init; } = string.Empty;

        /// <summary>Описание задачи.</summary>
        public string? Description { get; init; }

        /// <summary>Статус задачи.</summary>
        public string Status { get; init; } = string.Empty;

        /// <summary>Дата и время создания (UTC).</summary>
        public DateTime CreatedAt { get; init; }

        /// <summary>Дата и время последнего изменения (UTC).</summary>
        public DateTime? UpdatedAt { get; init; }
    }
}
