using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс для публикации доменных событий.
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Публикует событие в шину сообщений.
        /// </summary>
        /// <typeparam name="TEvent">Тип события.</typeparam>
        /// <param name="event">Экземпляр события.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class;
    }
}
