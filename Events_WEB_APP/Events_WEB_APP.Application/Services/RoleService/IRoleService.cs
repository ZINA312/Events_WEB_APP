using Events_WEB_APP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events_WEB_APP.Application.Services.RoleService
{
    public interface IRoleService
    {
        Task<Role> CreateRoleAsync(Role role);
        Task DeleteRoleAsync(Guid roleId);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(Guid roleId);
        Task<Role> UpdateRoleAsync(Guid roleId, string newRoleName);
    }
}
