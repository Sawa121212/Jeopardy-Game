using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using Confirmation.Interfaces.Services;
using Prism.Commands;
using ReactiveUI;

namespace Confirmation.Infrastructure.Views
{
    public class ConfirmationTestViewModel : ReactiveObject
    {
        private readonly IConfirmationService _confirmationService;

        public ConfirmationTestViewModel(IConfirmationService notificationService)
        {
            _confirmationService = notificationService;
            ShowInfoMessageCommand = new DelegateCommand(async () => await OnShowInfoMessage());
            ShowWarningMessageCommand = new DelegateCommand(async () => await OnShowWarningMessage());
            ShowErrorMessageCommand = new DelegateCommand(async () => await OnShowErrorMessage());
        }

        public ICommand ShowInfoMessageCommand { get; }
        public ICommand ShowWarningMessageCommand { get; }
        public ICommand ShowErrorMessageCommand { get; }

        private async Task OnShowInfoMessage()
        {
            await Dispatcher.UIThread.InvokeAsync(() => { _confirmationService.ShowInfoAsync("Information!", "Nice message!"); });
        }

        private async Task OnShowWarningMessage()
        {
            await _confirmationService.ShowWarningAsync("Warning!", "Nice message!");
        }

        private async Task OnShowErrorMessage()
        {
            await _confirmationService.ShowErrorAsync("Error!", "Nice! message");
        }
    }
}