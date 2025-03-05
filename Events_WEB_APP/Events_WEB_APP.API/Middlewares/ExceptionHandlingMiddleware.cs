namespace Events_WEB_APP.API.Middlewares
{
    /// <summary>
    /// Глобальный middleware для обработки исключений.
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GlobalExceptionMiddleware"/>.
        /// </summary>
        /// <param name="next">Делегат для следующего middleware в конвейере.</param>
        /// <param name="logger">Логгер для записи ошибок.</param>
        /// <param name="env">Информация об окружении приложения.</param>
        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Обрабатывает HTTP-запрос и перехватывает исключения.
        /// </summary>
        /// <param name="context">Контекст HTTP-запроса.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Global error: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Обрабатывает исключение и формирует ответ.
        /// </summary>
        /// <param name="context">Контекст HTTP-запроса.</param>
        /// <param name="exception">Исключение, которое произошло.</param>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new
            {
                Status = context.Response.StatusCode,
                Message = "Произошла ошибка при обработке запроса",
                Details = _env.IsDevelopment() ? exception.ToString() : null
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
