using Ascalon.ClientService.Features.Users.Dtos;
using MediatR;

namespace Ascalon.ClientService.Features.Users.CreateUser
{
    /// <summary>
    /// Комманда создания пользователя.
    /// </summary>
    public class CreateUserCommand : IRequest<User>
    {
        /// <summary>
        /// Возвращает или задаёт логин пользователя.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Возвращает или задаёт пароль пользователя.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Возвращает или задаёт фио пользователя.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Возвращает или задаёт роль пользователя.
        /// </summary>
        public RoleType Role { get; set; }
    }
}
