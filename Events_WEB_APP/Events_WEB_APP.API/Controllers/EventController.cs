using AutoMapper;
using Events_WEB_APP.Application.Services.EventService;
using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts.Event;
using Events_WEB_APP.Persistence.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events_WEB_APP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EventController"/>.
        /// </summary>
        /// <param name="eventService">Сервис для управления событиями.</param>
        /// <param name="mapper">Mapper для преобразования между сущностями и DTO.</param>
        /// <param name="env">Среда веб-хостинга.</param>
        public EventController(
            IEventService eventService,
            IMapper mapper,
            IWebHostEnvironment env)
        {
            _eventService = eventService;
            _mapper = mapper;
            _env = env;
        }

        /// <summary>
        /// Получает все события с пагинацией.
        /// </summary>
        /// <param name="page">Номер страницы для получения (по умолчанию 1).</param>
        /// <param name="pageSize">Размер страницы (по умолчанию 10).</param>
        /// <returns>Пагинированный ответ с событиями.</returns>
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<EventResponse>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _eventService.GetEventsPaginatedAsync(null, null, null, page, pageSize);
            return Ok(MapToPaginatedResponse(result));
        }

        /// <summary>
        /// Получает событие по его ID.
        /// </summary>
        /// <param name="id">ID события для получения.</param>
        /// <returns>Ответ с событием.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EventResponse>> GetById(Guid id)
        {
            var eventEntity = await _eventService.GetEventByIdAsync(id);
            if (eventEntity == null)
                return NotFound();
            return Ok(_mapper.Map<EventResponse>(eventEntity));
        }

        /// <summary>
        /// Ищет события по названию с пагинацией
        /// </summary>
        /// <param name="name">Фрагмент названия для поиска</param>
        /// <param name="page">Номер страницы (по умолчанию 1)</param>
        /// <param name="pageSize">Размер страницы (по умолчанию 10)</param>
        [HttpGet("search")]
        public async Task<ActionResult<PaginatedResponse<EventResponse>>> GetByName(
            [FromQuery] string name,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Search query cannot be empty");
            }

            var result = await _eventService.SearchEventsByNameAsync(name, page, pageSize);
            return Ok(MapToPaginatedResponse(result));
        }

        /// <summary>
        /// Создает новое событие.
        /// </summary>
        /// <param name="request">Запрос для создания события.</param>
        /// <returns>Ответ с созданным событием.</returns>
        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<EventResponse>> Create(
            [FromBody] EventCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var eventEntity = _mapper.Map<Event>(request);
            await _eventService.CreateEventAsync(eventEntity);
            return CreatedAtAction(nameof(GetById), new { Id = eventEntity.Id },
                _mapper.Map<EventResponse>(eventEntity));
        }

        /// <summary>
        /// Обновляет существующее событие.
        /// </summary>
        /// <param name="id">ID события для обновления.</param>
        /// <param name="request">Запрос с обновленными данными события.</param>
        /// <returns>Результат операции обновления.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] EventUpdateRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            var existingEvent = await _eventService.GetEventByIdAsync(id);
            if (existingEvent == null)
                return NotFound();

            var eventEntity = _mapper.Map<Event>(request);

            await _eventService.UpdateEventAsync(eventEntity);
            return NoContent();
        }

        /// <summary>
        /// Загружает изображение для события.
        /// </summary>
        /// <param name="id">ID события для загрузки изображения.</param>
        /// <param name="request">Запрос с изображением.</param>
        /// <returns>URL загруженного изображения.</returns>
        [HttpPost("{id}/image")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> UploadImage(
            Guid id,
            [FromForm] ImageUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest("File is required");
            }
            var allowedExtensions = new[] { ".jpg", ".png" };
            var fileExtension = Path.GetExtension(request.File.FileName);
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                return BadRequest("Invalid file type");
            }
            var eventEntity = await _eventService.GetEventByIdAsync(id);
            if (eventEntity == null)
            {
                return NotFound(); 
            }
            var fileName = await SaveFile(request.File);
            eventEntity.ImagePath = fileName;

            await _eventService.UpdateEventAsync(eventEntity);
            return Ok(_mapper.Map<EventResponse>(eventEntity).ImageUrl);
        }

        /// <summary>
        /// Фильтрует события по категориям, дате и местоположению с пагинацией.
        /// </summary>
        /// <param name="category">Категория для фильтрации.</param>
        /// <param name="date">Дата для фильтрации.</param>
        /// <param name="location">Местоположение для фильтрации.</param>
        /// <param name="page">Номер страницы для получения (по умолчанию 1).</param>
        /// <param name="pageSize">Размер страницы (по умолчанию 10).</param>
        /// <returns>Пагинированный ответ с отфильтрованными событиями.</returns>
        [HttpGet("filter")]
        public async Task<ActionResult<PaginatedResponse<EventResponse>>> Filter(
            [FromQuery] string? category,
            [FromQuery] DateTime? date,
            [FromQuery] string? location,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _eventService.GetEventsPaginatedAsync(
                category, date, location, page, pageSize);

            return Ok(MapToPaginatedResponse(result));
        }

        /// <summary>
        /// Удаляет событие по его ID.
        /// </summary>
        /// <param name="id">ID события для удаления.</param>
        /// <returns>Результат операции удаления.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _eventService.DeleteEventAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Преобразует пагинированный ответ событий в ответ с пагинацией событий.
        /// </summary>
        /// <param name="source">Исходный пагинированный ответ событий.</param>
        /// <returns>Пагинированный ответ с событиями.</returns>
        private PaginatedResponse<EventResponse> MapToPaginatedResponse(
            PaginatedResponse<Event> source)
        {
            return new PaginatedResponse<EventResponse>(
                _mapper.Map<List<EventResponse>>(source.Items),
                source.PageNumber,
                source.PageSize,
                source.TotalCount);
        }

        /// <summary>
        /// Сохраняет файл на сервере.
        /// </summary>
        /// <param name="file">Файл для сохранения.</param>
        /// <returns>Имя сохраненного файла.</returns>
        private async Task<string> SaveFile(IFormFile file)
        {
            var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsDir, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }
    }
}
