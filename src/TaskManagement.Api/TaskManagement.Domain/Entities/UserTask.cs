using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Entities
{
    /// <summary>
    /// Доменная сущность "Задача пользователя".
    /// </summary>
    public class UserTask
    {
        /// <summary>Уникальный идентификатор задачи.</summary>
        public Guid Id { get; private set; }

        /// <summary>Название задачи.</summary>
        public string Title { get; private set; } = string.Empty;

        /// <summary>Описание задачи (может быть пустым).</summary>
        public string? Description { get; private set; }

        /// <summary>Текущий статус задачи.</summary>
        public TaskStatus Status { get; private set; }

        /// <summary>Дата и время создания задачи (UTC).</summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>Дата и время последнего изменения задачи (UTC).</summary>
        public DateTime? UpdatedAt { get; private set; }

        /// <summary>
        /// Конструктор для EF Core.
        /// </summary>
        private UserTask() { }

        /// <summary>
        /// Фабричный метод создания новой задачи.
        /// </summary>
        /// <param name="title">Название задачи.</param>
        /// <param name="description">Описание задачи (опционально).</param>
        /// <returns>Новый экземпляр задачи со статусом "Новая".</returns>
        public static UserTask Create(string title, string? description)
        {
            var now = DateTime.UtcNow;
            return new UserTask
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                Status = TaskStatus.New,
                CreatedAt = now,
                UpdatedAt = now
            };
        }

        /// <summary>
        /// Обновляет название и описание задачи.
        /// </summary>
        /// <param name="title">Новое название задачи.</param>
        /// <param name="description">Новое описание задачи.</param>
        public void Update(string title, string? description)
        {
            Title = title;
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Изменяет статус задачи.
        /// </summary>
        /// <param name="newStatus">Новый статус задачи.</param>
        public void ChangeStatus(TaskStatus newStatus)
        {
            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
