using AutoMapper;
using Events_WEB_APP.Persistence.Contracts.Role;
using Events_WEB_APP.Application.Services.RoleService;
using Events_WEB_APP.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events_WEB_APP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RoleController"/>.
        /// </summary>
        /// <param name="roleService">Сервис для управления ролями.</param>
        /// <param name="mapper">Mapper для преобразования между сущностями и DTO.</param>
        public RoleController(
            IRoleService roleService,
            IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        /// <summary>
        /// Создает новую роль.
        /// </summary>
        /// <param name="request">Запрос для создания роли.</param>
        /// <returns>Ответ с созданной ролью.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var role = await _roleService.CreateRoleAsync(_mapper.Map<Role>(request));
                return CreatedAtAction(
                    nameof(GetRoleById),
                    new { roleId = role.Id },
                    _mapper.Map<RoleResponse>(role));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Получает все роли.
        /// </summary>
        /// <returns>Список ролей.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleResponse>>> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(_mapper.Map<IEnumerable<RoleResponse>>(roles));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Получает роль по ее ID.
        /// </summary>
        /// <param name="roleId">ID роли для получения.</param>
        /// <returns>Ответ с ролью.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("{roleId}")]
        public async Task<ActionResult<RoleResponse>> GetRoleById(Guid roleId)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(roleId);
                return Ok(_mapper.Map<RoleResponse>(role));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Обновляет существующую роль.
        /// </summary>
        /// <param name="roleId">ID роли для обновления.</param>
        /// <param name="request">Запрос с обновленными данными роли.</param>
        /// <returns>Ответ с обновленной ролью.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole(
            Guid roleId,
            [FromBody] UpdateRoleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedRole = await _roleService.UpdateRoleAsync(
                    roleId,
                    request.Name);

                return Ok(_mapper.Map<RoleResponse>(updatedRole));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Удаляет роль по ее ID.
        /// </summary>
        /// <param name="roleId">ID роли для удаления.</param>
        /// <returns>Результат операции удаления.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
            try
            {
                await _roleService.DeleteRoleAsync(roleId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
