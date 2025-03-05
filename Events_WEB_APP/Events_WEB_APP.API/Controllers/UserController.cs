using AutoMapper;
using Events_WEB_APP.Persistence.Contracts.User;
using Events_WEB_APP.Application.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace Events_WEB_APP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserController"/>.
        /// </summary>
        /// <param name="userService">Сервис для управления пользователями.</param>
        /// <param name="mapper">Mapper для преобразования между сущностями и DTO.</param>
        public UserController(
            IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Регистрирует нового пользователя.
        /// </summary>
        /// <param name="request">Запрос для регистрации пользователя.</param>
        /// <returns>Результат операции регистрации.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _userService.RegisterAsync(request.UserName, request.Email, request.Password);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Выполняет вход пользователя.
        /// </summary>
        /// <param name="request">Запрос для входа пользователя.</param>
        /// <returns>Результат операции входа.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var (accessToken, refreshToken) = await _userService.LoginAsync(request.Email, request.Password);

                HttpContext.Response.Cookies.Append("definitely-not-jwt-token", accessToken);
                HttpContext.Response.Cookies.Append("definitely-not-refresh-token", refreshToken);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        /// <summary>
        /// Обновляет токены доступа и обновления.
        /// </summary>
        /// <param name="accessT">Истекший токен доступа.</param>
        /// <param name="refreshT">Токен обновления.</param>
        /// <returns>Результат операции обновления токенов.</returns>
        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var expiredAccessToken = Request.Cookies["definitely-not-jwt-token"];
                var refreshToken = Request.Cookies["definitely-not-refresh-token"];

                if (string.IsNullOrEmpty(expiredAccessToken) || string.IsNullOrEmpty(refreshToken))
                    return Unauthorized("Tokens required");

                var (newAccessToken, newRefreshToken) =
                    await _userService.RefreshTokenAsync(expiredAccessToken, refreshToken);

                HttpContext.Response.Cookies.Append("definitely-not-jwt-token", newAccessToken);
                HttpContext.Response.Cookies.Append("definitely-not-refresh-token", newRefreshToken);

                return Ok(new { Message = "Tokens refreshed" });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        /// <summary>
        /// Получает пользователя по его ID.
        /// </summary>
        /// <param name="userId">ID пользователя для получения.</param>
        /// <returns>Ответ с пользователем.</returns>
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                return Ok(_mapper.Map<UserResponse>(user));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Получает всех пользователей.
        /// </summary>
        /// <returns>Список пользователей.</returns>
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(_mapper.Map<List<UserResponse>>(users));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Удаляет пользователя по его ID.
        /// </summary>
        /// <param name="userId">ID пользователя для удаления.</param>
        /// <returns>Результат операции удаления.</returns>
        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            try
            {
                await _userService.DeleteUserAsync(userId);
                return Ok(new { Message = "User deleted successfully" });
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
    }
}
