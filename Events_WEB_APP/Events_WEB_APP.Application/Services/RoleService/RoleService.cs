using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.UnitsOfWork;
using Microsoft.AspNetCore.Identity;

namespace Events_WEB_APP.Application.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Role> CreateRoleAsync(Role role)
        {
            if (string.IsNullOrWhiteSpace(role.Name)){
                throw new ArgumentException("Role name cannot be empty");
            }

            if (role.Name.Length > 50)
            {
                throw new ArgumentException("Role name cannot exceed 50 characters");
            }

            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.CommitAsync();
            return role;
        }

        public async Task DeleteRoleAsync(Guid roleId)
        {
            var role = await ValidateRoleExists(roleId);
            await _unitOfWork.Roles.DeleteAsync(role);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _unitOfWork.Roles.GetAllAsync();
        }

        public async Task<Role> GetRoleByIdAsync(Guid roleId)
        {
            return await ValidateRoleExists(roleId);
        }

        public async Task<Role> UpdateRoleAsync(Guid roleId, string newRoleName)
        {
            var role = await ValidateRoleExists(roleId);

            if (string.IsNullOrWhiteSpace(newRoleName))
                throw new ArgumentException("Role name cannot be empty");

            if (newRoleName.Length > 50)
                throw new ArgumentException("Role name cannot exceed 50 characters");

            role.Name = newRoleName;
            await _unitOfWork.Roles.UpdateAsync(role);
            await _unitOfWork.CommitAsync();
            return role;
        }

        private async Task<Role> ValidateRoleExists(Guid roleId)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(roleId);
            return role ?? throw new KeyNotFoundException($"Role with ID {roleId} not found");
        }
    }
}