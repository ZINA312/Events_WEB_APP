using AutoMapper;
using Events_WEB_APP.Application.Services.EventService;
using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts.Event;
using Events_WEB_APP.Persistence.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public EventController(
            IEventService eventService,
            IMapper mapper,
            IWebHostEnvironment env)
        {
            _eventService = eventService;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<EventResponse>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _eventService.GetEventsPaginatedAsync(null, null, null, page, pageSize);
            return Ok(MapToPaginatedResponse(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventResponse>> GetById(Guid id)
        {
            var eventEntity = await _eventService.GetEventByIdAsync(id);
            if (eventEntity == null)
                return NotFound();
            return Ok(_mapper.Map<EventResponse>(eventEntity));
        }

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

        private PaginatedResponse<EventResponse> MapToPaginatedResponse(
            PaginatedResponse<Event> source)
        {
            return new PaginatedResponse<EventResponse>(
                _mapper.Map<List<EventResponse>>(source.Items),
                source.PageNumber,
                source.PageSize,
                source.TotalCount);
        }

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
