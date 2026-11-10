using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Dto
{
    /// <summary>
    /// DTO для обновления существующей задачи.
    /// </summary>
    public sealed class UpdateTaskRequest
    {
        /// <summary>Новое название задачи.</summary>
        public string Title { get; init; } = string.Empty;

        /// <summary>Новое описание задачи.</summary>
        public string? Description { get; init; }

        /// <summary>Новый статус задачи.</summary>
        public string Status { get; init; } = string.Empty;
    }
}
