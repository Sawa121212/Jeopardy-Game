namespace Infrastructure.Interfaces.Services
{
    /// <summary>
    /// Служба статуса.
    /// </summary>
    public interface IStatusService
    {
        /// <summary>
        /// Установить статус.
        /// </summary>
        /// <param name="statusText">Описание статуса.</param>
        /// <param name="sucscess">Положительный статус.</param>
        void Status(string statusText, bool sucscess = true);
    }
}