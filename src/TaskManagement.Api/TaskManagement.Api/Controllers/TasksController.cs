using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Dto;
using TaskManagement.Application.Services;

namespace TaskManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        /// <summary>
        /// Создает новую задачу.
        /// </summary>
        /// <param name="request">Данные для создания задачи.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Созданная задача.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
        {
            var result = await _taskService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Получает задачу по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор задачи.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Данные задачи или 404, если не найдена.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _taskService.GetByIdAsync(id, cancellationToken);
            return result is null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Получает список всех задач.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список задач.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<TaskResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _taskService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Обновляет существующую задачу.
        /// </summary>
        /// <param name="id">Идентификатор задачи.</param>
        /// <param name="request">Новые данные задачи.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Обновленная задача или 404.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequest request, CancellationToken cancellationToken)
        {
            var result = await _taskService.UpdateAsync(id, request, cancellationToken);
            return result is null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Удаляет задачу по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор задачи.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>204 No Content при успехе или 404.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _taskService.DeleteAsync(id, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
    }
}
