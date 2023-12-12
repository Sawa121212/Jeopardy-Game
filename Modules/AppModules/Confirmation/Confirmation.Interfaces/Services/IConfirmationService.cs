using System.Threading.Tasks;
using Confirmation.Domain.Enums;

namespace Confirmation.Interfaces.Services
{
    /// <summary>
    /// Служба для работы с диалогами.
    /// </summary>
    public interface IConfirmationService
    {
        /// <summary>
        /// Показать диалог информационным сообщение. 
        /// </summary>
        /// <param name="title">Заголовок.</param>
        /// <param name="message">Сообщение.</param>
        /// <param name="buttons"></param>
        /// <returns>Результат возвращаемый диалогом сообщений.</returns>
        Task<ConfirmationResultEnum> ShowInfoAsync(
            string title, string message,
            ConfirmationResultEnum buttons = ConfirmationResultEnum.Ok);

        /// <summary>
        /// Показать диалог с сообщением о предупреждении. 
        /// </summary>
        /// <param name="title">Заголовок.</param>
        /// <param name="message">Сообщение.</param>
        /// <param name="buttons"></param>
        /// ///
        /// <returns>Результат возвращаемый диалогом сообщений.</returns>
        Task<ConfirmationResultEnum> ShowWarningAsync(
            string title, string message,
            ConfirmationResultEnum buttons = ConfirmationResultEnum.Ok);

        /// <summary>
        /// Показать диалог с сообщением об ошибке. 
        /// </summary>
        /// <param name="title">Заголовок.</param>
        /// <param name="message">Сообщение.</param>
        /// <param name="buttons"></param>
        /// <returns>Результат возвращаемый диалогом сообщений.</returns>
        Task<ConfirmationResultEnum> ShowErrorAsync(
            string title, string message,
            ConfirmationResultEnum buttons = ConfirmationResultEnum.Ok);
    }
}