using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Events_WEB_APP.Persistence.Repositories.UserRepository
{
    /// <summary>
    /// Репозиторий для управления пользователями в базе данных.
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserRepository"/>.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных для доступа к данным.</param>
        public UserRepository(EventsAppDbContext dbContext) : base(dbContext) { }


        /// <summary>
        /// Асинхронно получает пользователя по адресу электронной почты.
        /// </summary>
        /// <param name="email">Адрес электронной почты пользователя.</param>
        /// <returns>Пользователь, соответствующий указанному адресу электронной почты, или null, если пользователь не найден.</returns>
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _entities.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
