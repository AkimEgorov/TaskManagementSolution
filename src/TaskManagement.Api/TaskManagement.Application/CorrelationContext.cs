using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application
{
    /// <summary>
    /// Контекст для передачи CorrelationId через async-вызовы.
    /// Используется для сквозной трассировки запросов в распределенной системе.
    /// </summary>
    public static class CorrelationContext
    {
        private static readonly AsyncLocal<string?> _correlationId = new();

        /// <summary>
        /// Текущий CorrelationId для данной async-операции.
        /// </summary>
        public static string? CorrelationId
        {
            get => _correlationId.Value;
            set => _correlationId.Value = value;
        }
    }
}
