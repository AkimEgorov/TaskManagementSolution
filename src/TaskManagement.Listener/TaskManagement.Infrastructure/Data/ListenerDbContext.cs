using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Models;

namespace TaskManagement.Infrastructure.Data
{
    public sealed class ListenerDbContext : DbContext
    {
        public DbSet<TaskEventLog> TaskEventLogs { get; set; } = null!;

        public ListenerDbContext(DbContextOptions<ListenerDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEventLog>(entity =>
            {
                entity.ToTable("task_event_logs");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TaskId).HasColumnName("task_id");
                entity.Property(e => e.EventType).HasColumnName("event_type").HasMaxLength(50);
                entity.Property(e => e.EventData).HasColumnName("event_data").HasColumnType("jsonb");
                entity.Property(e => e.Timestamp).HasColumnName("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasIndex(e => e.TaskId);
            });
        }
    }
}
