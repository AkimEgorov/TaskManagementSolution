using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Dto;
using TaskManagement.Contracts.Events;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;
using TaskStatus = TaskManagement.Domain.Entities.TaskStatus;

namespace TaskManagement.Application.Services
{
    /// <summary>
    /// Сервис управления задачами. Содержит бизнес-логику CRUD-операций
    /// и публикацию событий при изменении состояния задач.
    /// </summary>
    public sealed class TaskService : ITaskService
    {
        private readonly AppDbContext _dbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IValidator<CreateTaskRequest> _createValidator;
        private readonly IValidator<UpdateTaskRequest> _updateValidator;

        public TaskService(
            AppDbContext dbContext,
            IEventPublisher eventPublisher,
            IValidator<CreateTaskRequest> createValidator,
            IValidator<UpdateTaskRequest> updateValidator)
        {
            _dbContext = dbContext;
            _eventPublisher = eventPublisher;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<TaskResponse> CreateAsync(CreateTaskRequest request, CancellationToken cancellationToken = default)
        {
            await _createValidator.ValidateAndThrowAsync(request, cancellationToken);

            var task = UserTask.Create(request.Title, request.Description);

            await _dbContext.Tasks.AddAsync(task, cancellationToken);

            var correlationId = CorrelationContext.CorrelationId;
            await _eventPublisher.PublishAsync(new TaskCreatedEvent(
                task.Id, task.Title, task.Description, task.Status.ToString(), task.CreatedAt, correlationId), cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return MapToResponse(task);
        }

        public async Task<TaskResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var task = await _dbContext.Tasks.FindAsync([id], cancellationToken);
            return task is null ? null : MapToResponse(task);
        }

        public async Task<IReadOnlyList<TaskResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var tasks = await _dbContext.Tasks.OrderByDescending(t => t.CreatedAt).ToListAsync(cancellationToken);
            return tasks.Select(MapToResponse).ToList();
        }

        public async Task<TaskResponse?> UpdateAsync(Guid id, UpdateTaskRequest request, CancellationToken cancellationToken = default)
        {
            await _updateValidator.ValidateAndThrowAsync(request, cancellationToken);

            var task = await _dbContext.Tasks.FindAsync([id], cancellationToken);
            if (task is null) 
                return null;

            task.Update(request.Title, request.Description);
            if (Enum.TryParse<TaskStatus>(request.Status, ignoreCase: true, out var newStatus))
            {
                task.ChangeStatus(newStatus);
            }

            var correlationId = CorrelationContext.CorrelationId;
            await _eventPublisher.PublishAsync(new TaskUpdatedEvent(
                task.Id, task.Title, task.Description, task.Status.ToString(), task.UpdatedAt!.Value, correlationId), cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return MapToResponse(task);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var task = await _dbContext.Tasks.FindAsync([id], cancellationToken);
            if (task is null)
                return false;

            _dbContext.Tasks.Remove(task);

            var correlationId = CorrelationContext.CorrelationId;
            await _eventPublisher.PublishAsync(new TaskDeletedEvent(id, DateTime.UtcNow, correlationId), cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        private static TaskResponse MapToResponse(UserTask task) => new()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
