using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Entities
{
    /// <summary>
    /// Статус задачи в системе управления задачами.
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>Новая задача, еще не взята в работу.</summary>
        New = 0,

        /// <summary>Задача находится в процессе выполнения.</summary>
        InProgress = 1,

        /// <summary>Задача выполнена.</summary>
        Completed = 2
    }
}
