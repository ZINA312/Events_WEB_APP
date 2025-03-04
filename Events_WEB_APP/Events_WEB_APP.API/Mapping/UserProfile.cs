using AutoMapper;
using Events_WEB_APP.Persistence.Contracts.User;
using Events_WEB_APP.Core.Entities;

namespace Events_WEB_APP.API.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserRequest, User>();

            CreateMap<User, UserResponse>();
        }
    }
}
