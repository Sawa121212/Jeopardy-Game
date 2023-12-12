using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using Confirmation.Module.Services;
using Prism.Commands;
using Prism.Mvvm;
using ReactiveUI;

namespace Confirmation.Module.Views
{
    public class ConfirmationTestViewModel : BindableBase
    {
        private readonly IConfirmationService _confirmationService;

        public ConfirmationTestViewModel(IConfirmationService confirmationService)
        {
            _confirmationService = confirmationService;
            ShowInfoMessageCommand = new DelegateCommand(OnShowInfoMessage);
            ShowWarningMessageCommand = new DelegateCommand(OnShowWarningMessage);
            ShowErrorMessageCommand = new DelegateCommand(OnShowErrorMessage);
        }

        public ICommand ShowInfoMessageCommand { get; }
        public ICommand ShowWarningMessageCommand { get; }
        public ICommand ShowErrorMessageCommand { get; }

        private async void OnShowInfoMessage()
        {
            await _confirmationService.ShowInfoAsync("Information!", "Nice message!");
        }

        private async void OnShowWarningMessage()
        {
            await _confirmationService.ShowWarningAsync("Warning!", "Nice message!");
        }

        private async void OnShowErrorMessage()
        {
            await _confirmationService.ShowErrorAsync("Error!", "Nice! message");
        }
    }
}