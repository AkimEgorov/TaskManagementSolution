using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Models
{
    /// <summary>
    /// Сущность для хранения истории событий задачи.
    /// </summary>
    public class TaskEventLog
    {
        public long Id { get; set; }
        public Guid TaskId { get; set; }
        public string EventType { get; set; } = string.Empty; // "Created", "Updated", "Deleted"
        public string EventData { get; set; } = string.Empty; // JSON-представление
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
