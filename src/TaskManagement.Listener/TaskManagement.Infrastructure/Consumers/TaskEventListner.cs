using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagement.Application.Enums;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Models;
using TaskManagement.Contracts.Events;
using TaskManagement.Infrastructure.Data;
using LogContext = Serilog.Context.LogContext;

namespace TaskManagement.Infrastructure.Consumers
{
    /// <summary>
    /// Консьюмер для обработки событий изменения задач.
    /// </summary>
    public sealed class TaskEventListener :
        IConsumer<TaskCreatedEvent>,
        IConsumer<TaskUpdatedEvent>,
        IConsumer<TaskDeletedEvent>
    {
        private readonly ILogger<TaskEventListener> _logger;
        private readonly IEventLogService _eventLogService;

        public TaskEventListener(ILogger<TaskEventListener> logger, IEventLogService eventLogService)
        {
            _logger = logger;
            _eventLogService = eventLogService;
        }

        public async Task Consume(ConsumeContext<TaskCreatedEvent> context)
        {
            var correlationId = context.Message.CorrelationId ?? "N/A";

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _eventLogService.SaveEventAsync(
                    context.Message.TaskId,
                    TaskEventType.Created,
                    JsonSerializer.Serialize(context.Message));

                _logger.LogInformation("[TASK CREATED] TaskId: {TaskId}, Title: {Title}",
                    context.Message.TaskId, context.Message.Title);
            }
        }

        public async Task Consume(ConsumeContext<TaskUpdatedEvent> context)
        {
            var correlationId = context.Message.CorrelationId ?? "N/A";

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _eventLogService.SaveEventAsync(
                    context.Message.TaskId,
                    TaskEventType.Updated,
                    JsonSerializer.Serialize(context.Message));

                _logger.LogInformation("[TASK UPDATED] TaskId: {TaskId}, New Status: {Status}",
                    context.Message.TaskId, context.Message.Status);
            }
        }

        public async Task Consume(ConsumeContext<TaskDeletedEvent> context)
        {
            var correlationId = context.Message.CorrelationId ?? "N/A";

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _eventLogService.SaveEventAsync(
                    context.Message.TaskId,
                    TaskEventType.Deleted,
                    JsonSerializer.Serialize(context.Message));

                _logger.LogInformation("[TASK DELETED] TaskId: {TaskId}", context.Message.TaskId);
            }
        }
    }
}
