using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Common.Core.Views;
using Infrastructure.Interfaces.Managers;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;
using TelegramAPI.Infrastructure.Interfaces.Services.Settings;
using Users.Domain.Models;
using Users.Infrastructure.Interfaces;
using Timer = System.Timers.Timer;

namespace Infrastructure.Ui.Views.Settings
{
    /// <summary>
    /// Основные настройки приложения
    /// </summary>
    public class TelegramAdminSettingsViewModel : NavigationViewModelBase
    {
        public TelegramAdminSettingsViewModel(
            IRegionManager regionManager,
            ITelegramSettingsService telegramSettingsService,
            IUserService userService,
            IAdminManager adminManager)
            : base(regionManager)
        {
            _adminManager = adminManager;
            _userService = userService;
            _telegramSettingsService = telegramSettingsService;

            CheckAdminUserIdCommand = new DelegateCommand(async () => await OnCheckAdminUserId(), () => !IsAddAdminMode)
                .ObservesProperty(() => IsAddAdminMode);
            CancelAddAdminModeCommand = new DelegateCommand(OnCancelAddAdminMode);
            ApplySettingsCommand = new DelegateCommand(OnSaveSettings);

            // Инициализируем таймер
            _timer = new Timer(2000); // Промежуток времени в миллисекундах - 2000 мс = 2 секунды
        }

        public long AdminId
        {
            get => _adminId;
            set => this.RaiseAndSetIfChanged(ref _adminId, value);
        }

        public string AdminIdStatus
        {
            get => _adminIdStatus;
            set => this.RaiseAndSetIfChanged(ref _adminIdStatus, value);
        }

        public string AdminKey
        {
            get => _adminKey;
            set => this.RaiseAndSetIfChanged(ref _adminKey, value);
        }

        public bool IsAddAdminMode
        {
            get => _isAddAdminMode;
            set => this.RaiseAndSetIfChanged(ref _isAddAdminMode, value);
        }

        public ICommand CheckAdminUserIdCommand { get; }
        public ICommand CancelAddAdminModeCommand { get; }
        public ICommand ApplySettingsCommand { get; }

        private void Initialization()
        {
            AdminIdStatus = null;
            AdminId = GetAdminUserIdKey();
        }

        private async Task OnCheckAdminUserId()
        {
            if (_adminId != -1)
            {
                //if (!long.TryParse(_adminId, out long id))

                /*AdminIdStatus = "Введенные данные не распознаны";
                return;*/

                AdminKey = await _adminManager.VerifyAddAdminMode(_adminId);
                IsAddAdminMode = true;
                CheckAdminUserIdTimerStart();
            }

            AdminIdStatus = "Введите данные";
        }

        private long GetAdminUserIdKey() => _telegramSettingsService.GetAdminUserId();

        private void OnCancelAddAdminMode()
        {
            AdminKey = null;
            _adminManager.CancelAddAdminMode();
            IsAddAdminMode = false;

            AdminIdStatus = "Вы стали администратором";
        }

        private void OnSaveSettings()
        {
            _telegramSettingsService.SaveSettings();
        }

        private void CheckAdminUserIdTimerStart()
        {
            _timerSecond = 0;
            _timer.Elapsed += OnTimedEvent;
            _timer.Enabled = true;

            //настройка состояния юзера
            if (_userService.TryGetUserById(AdminId, out User user))
            {
                user.State = StateUserEnum.CheckAddedAdmin;
                _userService.UpdateUser(user);
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            // Здесь выполняется ваш метод каждые две секунды
            _timerSecond += (int) _timer.Interval / 1000;
            AdminIdStatus = $"Ожидание подтверждения... ({_timerSecond} секунд)";

            long id = GetAdminUserIdKey();

            if (id == -1 && _timerSecond < 120)
            {
                return;
            }

            // Остановка таймера
            StopTimer();
            OnCancelAddAdminMode();
        }

        private void StopTimer()
        {
            _timer.Elapsed -= OnTimedEvent;
            _timer.Stop();
        }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            Initialization();
        }

        /// <inheritdoc />
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            StopTimer();
        }

        private readonly IAdminManager _adminManager;
        private readonly IUserService _userService;
        private readonly ITelegramSettingsService _telegramSettingsService;
        private Timer _timer;
        private long _adminId;
        private string _adminIdStatus;
        private string _adminKey;
        private bool _isAddAdminMode;
        private int _timerSecond;
    }
}