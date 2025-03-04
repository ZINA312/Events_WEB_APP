using AutoMapper;
using Events_WEB_APP.Persistence.Contracts.Role;
using Events_WEB_APP.Core.Entities;

namespace Events_WEB_APP.API.Mapping
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<CreateRoleRequest, Role>();

            CreateMap<Role, RoleResponse>();
        }
    }
}
