using System.Threading.Tasks;
using System.Windows.Input;
using Common.Core.Views;
using Confirmation.Module.Services;
using Prism.Commands;
using TelegramAPI.Infrastructure.Interfaces.Managers;
using ReactiveUI;
using Users.Infrastructure.Interfaces;

namespace JeopardyGame.Views.Shell
{
    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel(
            IConfirmationService confirmationService,
            ITelegramBotManager telegramBotManager,
            IUserService userService)
        {
            _confirmationService = confirmationService;
            _telegramBotManager = telegramBotManager;
            _userService = userService;
            StartTelegramBotCommand = new DelegateCommand(async () => await OnStartBot());
        }

        private async Task OnStartBot()
        {
            if (!_telegramBotManager.IsConnected)
            {
                await _telegramBotManager.StartTelegramBot().ConfigureAwait(true);
                _userService.ResetUsersStatus();
            }
            else
            {
                return;
            }

            if (!_telegramBotManager.IsConnected)
            {
                await _confirmationService.ShowInfoAsync("Ошибка", $"TelegramBotClient не запущен!");
                return;
            }
        }

        public string Title => "Своя игра";

        public ITelegramBotManager TelegramBotManager
        {
            get => _telegramBotManager;
            set => this.RaiseAndSetIfChanged(ref _telegramBotManager, value);
        }

        public ICommand StartTelegramBotCommand { get; }
        private readonly IConfirmationService _confirmationService;
        private ITelegramBotManager _telegramBotManager;
        private readonly IUserService _userService;
    }
}