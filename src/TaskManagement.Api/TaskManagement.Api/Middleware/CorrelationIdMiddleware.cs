using Serilog;
using Serilog.Context;
using TaskManagement.Application;

namespace TaskManagement.Api.Middleware
{
    /// <summary>
    /// Middleware для генерации и передачи CorrelationId через все сервисы.
    /// Обеспечивает сквозную трассировку запросов.
    /// </summary>
    public sealed class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdHeader = "X-Correlation-ID";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var extractedCorrelationId))
            {
                extractedCorrelationId = Guid.NewGuid().ToString("N");
            }

            var correlationId = extractedCorrelationId.ToString();

            CorrelationContext.CorrelationId = correlationId;

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                context.Response.OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey(CorrelationIdHeader))
                    {
                        context.Response.Headers[CorrelationIdHeader] = correlationId;
                    }
                    return Task.CompletedTask;
                });

                Log.Information("[REQUEST START] {Method} {Path} | CorrelationId: {CorrelationId}",
                    context.Request.Method, context.Request.Path, correlationId);

                await _next(context);

                Log.Information("[REQUEST END] {Method} {Path} | Status: {StatusCode} | CorrelationId: {CorrelationId}",
                    context.Request.Method, context.Request.Path, context.Response.StatusCode, correlationId);
            }
        }
    }
}
