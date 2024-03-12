using System;
using System.Threading.Tasks;
using Common.Core.Components;
using Users.Domain.Models;
using Telegram.Bot.Types;

namespace TelegramAPI.Infrastructure.Interfaces.Managers
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
        /// <param name="update"></param>
        /// <returns></returns>
        Result<Tuple<StateUserEnum, string>> CheckAddedAdminMode(Update update);
    }
}