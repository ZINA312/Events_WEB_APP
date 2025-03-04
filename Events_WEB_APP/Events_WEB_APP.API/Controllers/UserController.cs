using AutoMapper;
using Events_WEB_APP.Persistence.Contracts.User;
using Events_WEB_APP.Application.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.IdentityModel.Tokens;

namespace Events_WEB_APP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(
            IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

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

        //токены достаем из кук, передаем явно для тестирования
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(string accessT, string refreshT)
        {
            try
            {
                //var expiredAccessToken = Request.Cookies["access-token"];
                //var refreshToken = Request.Cookies["refresh-token"];
                var expiredAccessToken = accessT;
                var refreshToken = refreshT;


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

        [HttpGet("{userId}")]
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
    }
}
