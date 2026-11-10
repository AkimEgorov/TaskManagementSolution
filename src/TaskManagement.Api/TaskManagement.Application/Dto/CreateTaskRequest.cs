using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Dto
{
    /// <summary>
    /// DTO для создания новой задачи.
    /// </summary>
    public sealed class CreateTaskRequest
    {
        /// <summary>Название задачи.</summary>
        public string Title { get; init; } = string.Empty;

        /// <summary>Описание задачи (опционально).</summary>
        public string? Description { get; init; }
    }
}
