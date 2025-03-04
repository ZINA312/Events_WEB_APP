using AutoMapper;
using Events_WEB_APP.Application.Services.ParticipantService;
using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts;
using Events_WEB_APP.Persistence.Contracts.Participant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events_WEB_APP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService _participantService;
        private readonly IMapper _mapper;
        public ParticipantController(IParticipantService participantService, IMapper mapper)
        {
            _participantService = participantService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RegisterParticipant([FromBody] ParticipantCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var participant = _mapper.Map<Participant>(request);
            
            try
            {
                var createdParticipant = await _participantService.CreateParticipantAsync(participant);
                return CreatedAtAction(
                    nameof(GetParticipantById),
                    new { id = createdParticipant.Id },
                    _mapper.Map<ParticipantResponse>(createdParticipant));
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("event/{eventId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetParticipantsByEvent(
            Guid eventId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var participants = await _participantService.GetParticipantsByEventIdPaginatedAsync(
                    eventId, page, pageSize);
                var response = new PaginatedResponse<Participant>(participants, page, pageSize, participants.Count);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetParticipantById(Guid id)
        {
            try
            {
                var participant = await _participantService.GetParticipantByIdAsync(id);
                return Ok(participant);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> CancelParticipation(Guid id)
        {
            try
            {
                var deletedParticipant = await _participantService.DeleteParticipantAsync(id);
                return Ok(deletedParticipant);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetParticipationsByUser(
            Guid userId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var participations = await _participantService.GetParticipantsByUserIdPaginatedAsync(
                    userId, page, pageSize);
                var response = new PaginatedResponse<Participant>(participations, page, pageSize, participations.Count);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
