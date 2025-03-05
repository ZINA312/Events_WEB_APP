using AutoMapper;
using Events_WEB_APP.Persistence.Contracts.User;
using Events_WEB_APP.Core.Entities;

namespace Events_WEB_APP.API.Mapping
{
    /// <summary>
    /// Профиль для маппинга пользователей.
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserProfile"/>.
        /// </summary>
        public UserProfile()
        {
            CreateMap<RegisterUserRequest, User>();

            CreateMap<User, UserResponse>();
        }
    }
}
