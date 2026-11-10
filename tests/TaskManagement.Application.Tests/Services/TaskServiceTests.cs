using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Dto;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Application.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly AppDbContext _dbContext;
        private readonly Mock<IEventPublisher> _eventPublisherMock;
        private readonly Mock<IValidator<CreateTaskRequest>> _createValidatorMock;
        private readonly Mock<IValidator<UpdateTaskRequest>> _updateValidatorMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(options);

            _eventPublisherMock = new Mock<IEventPublisher>();
            _createValidatorMock = new Mock<IValidator<CreateTaskRequest>>();
            _updateValidatorMock = new Mock<IValidator<UpdateTaskRequest>>();

            _createValidatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<CreateTaskRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _updateValidatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<UpdateTaskRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _taskService = new TaskService(
                _dbContext,
                _eventPublisherMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ShouldCreateTaskAndPublishEventWithCorrelationId()
        {
            // Arrange
            var request = new CreateTaskRequest { Title = "Test Task", Description = "Test Desc" };
            var expectedCorrelationId = "test-correlation-123";
            CorrelationContext.CorrelationId = expectedCorrelationId;

            // Act
            var result = await _taskService.CreateAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(request.Title);

            _eventPublisherMock.Verify(
                p => p.PublishAsync(
                    It.Is<Contracts.Events.TaskCreatedEvent>(e =>
                        e.Title == request.Title &&
                        e.CorrelationId == expectedCorrelationId),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            var taskInDb = await _dbContext.Tasks.FindAsync(result.Id);
            taskInDb.Should().NotBeNull();
            taskInDb!.Title.Should().Be(request.Title);

            CorrelationContext.CorrelationId = null;
        }

        [Fact]
        public async Task UpdateAsync_ExistingTask_ShouldUpdateAndPublishEvent()
        {
            // Arrange
            var existingTask = UserTask.Create("Old Title", "Old Desc");
            await _dbContext.Tasks.AddAsync(existingTask);
            await _dbContext.SaveChangesAsync();

            var updateRequest = new UpdateTaskRequest
            {
                Title = "New Title",
                Description = "New Desc",
                Status = "InProgress"
            };

            // Act
            var result = await _taskService.UpdateAsync(existingTask.Id, updateRequest);

            // Assert
            result.Should().NotBeNull();
            result!.Title.Should().Be("New Title");
            result.Status.Should().Be("InProgress");

            _eventPublisherMock.Verify(
                p => p.PublishAsync(
                    It.Is<Contracts.Events.TaskUpdatedEvent>(e => e.Status == "InProgress"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_TaskNotFound_ShouldReturnNullAndNotPublish()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var updateRequest = new UpdateTaskRequest { Title = "New Title", Status = "New" };

            // Act
            var result = await _taskService.UpdateAsync(taskId, updateRequest);

            // Assert
            result.Should().BeNull();

            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ExistingTask_ShouldDeleteAndPublishEvent()
        {
            // Arrange
            var existingTask = UserTask.Create("Task to Delete", "Desc");
            await _dbContext.Tasks.AddAsync(existingTask);
            await _dbContext.SaveChangesAsync();

            CorrelationContext.CorrelationId = "delete-corr-id";

            // Act
            var result = await _taskService.DeleteAsync(existingTask.Id);

            // Assert
            result.Should().BeTrue();

            _eventPublisherMock.Verify(
                p => p.PublishAsync(
                    It.Is<Contracts.Events.TaskDeletedEvent>(e =>
                        e.TaskId == existingTask.Id &&
                        e.CorrelationId == "delete-corr-id"),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            var taskInDb = await _dbContext.Tasks.FindAsync(existingTask.Id);
            taskInDb.Should().BeNull();

            CorrelationContext.CorrelationId = null;
        }

        [Fact]
        public async Task DeleteAsync_TaskNotFound_ShouldReturnFalseAndNotPublish()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            // Act
            var result = await _taskService.DeleteAsync(taskId);

            // Assert
            result.Should().BeFalse();

            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
