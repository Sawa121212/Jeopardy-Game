using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using Notification.Module.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace Notification.Module.Views
{
    public class NotificationTestViewModel : BindableBase
    {
        private readonly INotificationService _notificationService;

        public NotificationTestViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
            ShowInfoMessageCommand = new DelegateCommand(async () => await OnShowInfoMessage());
            ShowWarningMessageCommand = new DelegateCommand(async () => await OnShowWarningMessage());
            ShowErrorMessageCommand = new DelegateCommand(async () => await OnShowErrorMessage());
        }

        public ICommand ShowInfoMessageCommand { get; }
        public ICommand ShowWarningMessageCommand { get; }
        public ICommand ShowErrorMessageCommand { get; }

        private async Task OnShowInfoMessage()
        {
            Dispatcher.UIThread.Post(() =>
            {
                _notificationService.Show("Information!", "Nice!", NotificationType.Information);
            });
        }

        private async Task OnShowWarningMessage()
        {
            _notificationService.Show("Warning!", "Nice!", NotificationType.Warning);
        }

        private async Task OnShowErrorMessage()
        {
            _notificationService.Show("Error!", "Nice!", NotificationType.Error);
        }
    }
}