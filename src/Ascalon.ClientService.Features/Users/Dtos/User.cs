namespace Ascalon.ClientService.Features.Users.Dtos
{
    /// <summary>
    /// Информация о пользователе системы.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Возвращает или задаёт идентификатор пользователя.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Возвращает или задаёт наименование роли.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Возвращает или задаёт ФИО пользователя.
        /// </summary>
        public string FullName { get; set; }
    }
}
