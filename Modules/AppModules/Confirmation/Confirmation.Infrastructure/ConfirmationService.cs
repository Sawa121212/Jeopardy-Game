using System.Threading.Tasks;
using Common.Ui.Managers;
using Confirmation.Domain.Enums;
using Confirmation.Domain.Models;
using Confirmation.Interfaces.Services;
using Confirmation.Ui.Views;

namespace Confirmation.Infrastructure
{
    public class ConfirmationService : IConfirmationService
    {
        private readonly IDialogManager _dialogManager;

        public ConfirmationService(IDialogManager dialogManager)
        {
            _dialogManager = dialogManager;
        }

        /// <inheritdoc />
        public async Task<ConfirmationResultEnum> ShowInfoAsync(string title, string message,
            ConfirmationResultEnum buttons = ConfirmationResultEnum.Ok)
        {
            return await _dialogManager.ShowDialogAsync<ConfirmationView, ConfirmationResultEnum, DialogInfo>(
                new Information(title, message, buttons), true);
        }

        /// <inheritdoc />
        public async Task<ConfirmationResultEnum> ShowWarningAsync(string title, string message,
            ConfirmationResultEnum buttons = ConfirmationResultEnum.Ok)
        {
            return await _dialogManager.ShowDialogAsync<ConfirmationView, ConfirmationResultEnum, DialogInfo>(
                new Warning(title, message, buttons), true);
        }

        /// <inheritdoc />
        public async Task<ConfirmationResultEnum> ShowErrorAsync(string title, string message,
            ConfirmationResultEnum buttons = ConfirmationResultEnum.Ok)
        {
            return await _dialogManager.ShowDialogAsync<ConfirmationView, ConfirmationResultEnum, DialogInfo>(
                new Error(title, message, buttons), true);
        }
    }
}