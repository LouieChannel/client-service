using Newtonsoft.Json;

namespace Ascalon.ClientService.Features.Tasks.Dtos
{
    /// <summary>
    /// Инфорация о пользователе.
    /// </summary>
    public class User
    {
        /// <summary>
        ///  Возвращает или задаёт идентификатор пользователя.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  Возвращает или задаёт полное имя пользователя.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        ///  Возвращает или задаёт идентификатор самосвала.
        /// </summary>
        [JsonIgnore]
        public int? DumperId { get; set; }
    }
}
