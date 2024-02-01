using Common.Core.Components;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Users.Domain.Models;

namespace TelegramAPI.Test.Managers
{
    /// <summary>
    /// Специалист по работе с админом.
    /// </summary>
    public interface IAdminManager
    {
        /// <summary>
        /// Запуск ожидания админа
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        Task<string> VerifyAddAdminMode(long chatId);

        /// <summary>
        /// Отмена ожидания админа
        /// </summary>
        void CancelAddAdminMode();

        /// <summary>
        /// Проверка админа.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Result<StateUserEnum> CheckAddedAdminMode(Message message);
    }
}