using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Enums
{
    /// <summary>
    /// Типы событий жизненного цикла задачи.
    /// </summary>
    public enum TaskEventType
    {
        Created,
        Updated,
        Deleted
    }
}
