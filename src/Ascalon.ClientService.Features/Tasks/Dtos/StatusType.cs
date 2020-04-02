namespace Ascalon.ClientService.Features.Tasks.Dtos
{
    /// <summary>
    /// Информация о статусах задачи.
    /// </summary>
    public enum StatusType : short
    {
        /// <summary>
        /// Задача создана.
        /// </summary>
        Created = 0,

        /// <summary>
        /// Задача в процесе выполнения.
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// Задача выполнена.
        /// </summary>
        Done = 2
    }
}
