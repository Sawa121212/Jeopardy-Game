﻿using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Common.Core.Components;
using Common.Core.Views;
using Common.Extensions;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;
using TelegramAPI.Test.Managers;
using TelegramAPI.Test.Services.Settings;
using Users.Domain.Models;
using Users.Infrastructure.Interfaces;
using Timer = System.Timers.Timer;

namespace TelegramAPI.Test.Views.Settings
{
    /// <summary>
    /// Основные настройки приложения
    /// </summary>
    public class TelegramSettingsViewModel : NavigationViewModelBase
    {
        public TelegramSettingsViewModel(
            IRegionManager regionManager,
            ITelegramSettingsService telegramSettingsService,
            ITelegramBotManager telegramBotManager, ITelegramBotService telegramBotService,
            IUserService userService,
            IAdminManager adminManager)
            : base(regionManager)
        {
            _adminManager = adminManager;
            _userService = userService;
            _telegramSettingsService = telegramSettingsService;
            _telegramBotManager = telegramBotManager;
            _telegramBotService = telegramBotService;
            CheckTokenCommand = new DelegateCommand(async () => await OnCheckToken());
            CheckAdminUserIdCommand = new DelegateCommand(async () => await OnCheckAdminUserId(), () => !IsAddAdminMode)
                .ObservesProperty(() => IsAddAdminMode);
            CancelAddAdminModeCommand = new DelegateCommand(OnCancelAddAdminMode);
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

        public ICommand CheckTokenCommand { get; }
        public ICommand CheckAdminUserIdCommand { get; }
        public ICommand CancelAddAdminModeCommand { get; }
        public ICommand ApplySettingsCommand { get; }

        private async Task Initialization()
        {
            AdminIdStatus = null;
            TokenStatus = null;
            Token = _telegramSettingsService.GetGameBotToken();
            AdminId = GetAdminUserIdKey();
            await OnCheckToken();
        }

        private async Task OnCheckToken()
        {
            if (!_token.IsNullOrEmpty())
            {
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
            // Инициализируем таймер
            _timer = new Timer(2000); // Промежуток времени в миллисекундах - 2000 мс = 2 секунды
            _timer.Elapsed += OnTimedEvent;
            _timer.Enabled = true;
            _timerSecond = 0;

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
            if (_timer is null)
            {
                return;
            }
            _timer.Elapsed -= OnTimedEvent;

            _timer.Stop();
            _timer.Dispose();
        }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            Task.Run(async () => await Initialization());
        }

        /// <inheritdoc />
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            StopTimer();
        }

        private readonly ITelegramBotManager _telegramBotManager;
        private readonly ITelegramBotService _telegramBotService;
        private readonly IAdminManager _adminManager;
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