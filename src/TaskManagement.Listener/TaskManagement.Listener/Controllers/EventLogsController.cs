using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Models;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Listener.Controllers
{
    /// <summary>
    /// Контроллер для получения истории событий задач.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public sealed class EventLogsController : ControllerBase
    {
        private readonly IEventLogService _eventLogService;

        public EventLogsController(IEventLogService eventLogService)
        {
            _eventLogService = eventLogService;
        }

        /// <summary>
        /// Получает историю событий для конкретной задачи по её Id.
        /// </summary>
        /// <param name="taskId">Идентификатор задачи.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список событий, отсортированных по времени (от новых к старым).</returns>
        [HttpGet("task/{taskId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventsByTaskId(Guid taskId, CancellationToken cancellationToken)
        {
            var events = await _eventLogService.GetEventsByTaskIdAsync(taskId, cancellationToken);
            return Ok(events);
        }

        /// <summary>
        /// Получает общую ленту всех событий.
        /// </summary>
        /// <param name="limit">Максимальное количество записей (по умолчанию 100).</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список последних событий.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEvents([FromQuery] int limit = 100, CancellationToken cancellationToken = default)
        {
            var events = await _eventLogService.GetAllEventsAsync(limit, cancellationToken);
            return Ok(events);
        }
    }
}
