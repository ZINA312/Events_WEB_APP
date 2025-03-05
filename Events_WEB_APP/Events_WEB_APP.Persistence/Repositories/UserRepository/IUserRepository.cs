using Events_WEB_APP.Core.Entities;

namespace Events_WEB_APP.Persistence.Repositories.UserRepository
{
    /// <summary>
    /// Интерфейс репозитория для управления пользователями.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Асинхронно получает пользователя по адресу электронной почты.
        /// </summary>
        /// <param name="email">Адрес электронной почты пользователя.</param>
        /// <returns>Пользователь, соответствующий указанному адресу электронной почты.</returns>
        Task<User> GetByEmailAsync(string email);
    }
}
