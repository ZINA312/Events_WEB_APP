using AutoMapper;
using Events_WEB_APP.Persistence.Contracts.Role;
using Events_WEB_APP.Core.Entities;

namespace Events_WEB_APP.API.Mapping
{
    /// <summary>
    /// Профиль для маппинга ролей.
    /// </summary>
    public class RoleProfile : Profile
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RoleProfile"/>.
        /// </summary>
        public RoleProfile()
        {
            CreateMap<CreateRoleRequest, Role>();

            CreateMap<Role, RoleResponse>();
        }
    }
}
