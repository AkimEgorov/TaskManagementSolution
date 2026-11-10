using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Models;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Реализация репозитория истории событий.
    /// </summary>
    public sealed class EventLogRepository : IEventLogRepository
    {
        private readonly ListenerDbContext _dbContext;

        public EventLogRepository(ListenerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync(TaskEventLog eventLog, CancellationToken cancellationToken = default)
        {
            await _dbContext.TaskEventLogs.AddAsync(eventLog, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<TaskEventLog>> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.TaskEventLogs
                .Where(e => e.TaskId == taskId)
                .OrderByDescending(e => e.Timestamp)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<TaskEventLog>> GetAllAsync(int limit = 100, CancellationToken cancellationToken = default)
        {
            return await _dbContext.TaskEventLogs
                .OrderByDescending(e => e.Timestamp)
                .Take(limit)
                .ToListAsync(cancellationToken);
        }
    }
}
