using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Common.Core.Components;
using Common.Core.Views;
using Common.Extensions;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;
using TelegramAPI.Infrastructure.Interfaces.Managers;
using TelegramAPI.Infrastructure.Interfaces.Services.Settings;
using Users.Domain.Models;
using Users.Infrastructure.Interfaces;
using Timer = System.Timers.Timer;

namespace TelegramAPI.Ui.Views.Settings
{
    /// <summary>
    /// Основные настройки приложения
    /// </summary>
    public class TelegramSettingsViewModel : NavigationViewModelBase
    {
        public TelegramSettingsViewModel(
            IRegionManager regionManager,
            ITelegramSettingsService telegramSettingsService,
            ITelegramBotManager telegramBotManager,
            ITelegramBotService telegramBotService,
            IUserService userService)
            : base(regionManager)
        {
            _userService = userService;
            _telegramSettingsService = telegramSettingsService;
            _telegramBotManager = telegramBotManager;
            _telegramBotService = telegramBotService;
            CheckTokenCommand = new DelegateCommand(async () => await OnCheckToken());

            ApplySettingsCommand = new DelegateCommand(OnSaveSettings);
        }

        public string Token
        {
            get => _token;
            set => this.RaiseAndSetIfChanged(ref _token, value);
        }

        public string TokenStatus
        {
            get => _tokenStatus;
            set => this.RaiseAndSetIfChanged(ref _tokenStatus, value);
        }

        public ICommand CheckTokenCommand { get; }
        public ICommand ApplySettingsCommand { get; }

        private async Task Initialization()
        {
            TokenStatus = null;
            Token = _telegramSettingsService.GetGameBotToken();
            await OnCheckToken();
        }

        private async Task OnCheckToken()
        {
            if (!_token.IsNullOrEmpty())
            {
                /*if (_telegramBotManager.Is)
                {

                }*/
                Result<bool> result = await _telegramBotManager.StartTelegramBot(_token);

                if (!result)
                {
                    TokenStatus = result.ErrorMessage;
                    return;
                }

                await _telegramSettingsService.SetGameBotToken(_token);
                TokenStatus = "Telegram бот запущен";
            }
        }

        private long GetAdminUserIdKey() => _telegramSettingsService.GetAdminUserId();

        private void OnSaveSettings()
        {
            _telegramSettingsService.SaveSettings();
        }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            Task.Run(async () => await Initialization());
        }

        private readonly ITelegramBotManager _telegramBotManager;
        private readonly ITelegramBotService _telegramBotService;
        private readonly IUserService _userService;
        private readonly ITelegramSettingsService _telegramSettingsService;
        private Timer _timer;
        private string _token;
        private long _adminId;
        private string _tokenStatus;
        private string _adminIdStatus;
        private string _adminKey;
        private bool _isAddAdminMode;
        private int _timerSecond;
    }
}