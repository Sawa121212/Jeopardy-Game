using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Core.Prism;
using Common.Core.Prism.Regions;
using Common.Core.Views;
using Confirmation.Module.Enums;
using Confirmation.Module.Services;
using DataDomain.Rooms;
using Game.Domain.Events.Games;
using Game.Domain.Events.Players;
using Game.Domain.Events.Players.Host;
using Game.Domain.Events.Rooms;
using Game.Infrastructure.Interfaces.Mangers;
using Game.Ui.Views.GameControls;
using Game.Ui.Views.GameControls.Pages;
using Infrastructure.Domain.Helpers;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using ReactiveUI;
using TelegramAPI.Infrastructure.Interfaces.Managers;

namespace Game.Ui.Views
{
    public class RoomViewModel : NavigationViewModelBase
    {
        /// <inheritdoc />
        public RoomViewModel(
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IConfirmationService confirmationService,
            ITelegramBotManager telegramBotManager,
            IGameManager gameManager)
            : base(regionManager)
        {
            _eventAggregator = eventAggregator;
            _confirmationService = confirmationService;
            _telegramBotManager = telegramBotManager;
            _gameManager = gameManager;

            MoveGoBackCommand = new DelegateCommand(OnMoveGoBack);

            CreateRoomCommand = new DelegateCommand(async () => await OnCreateRoom());

            SendAnInvitationCommand = new DelegateCommand(OnSendAnInvitation);
            AddPlayerCommand = new DelegateCommand(OnAddBot);
            KickOutPlayerCommand = new DelegateCommand<PlayerModel>(OnKickOutPlayer);
            SetPlayerToHostCommand = new DelegateCommand<PlayerModel>(OnSetPlayerToHost);
            GetOutHostPlayerCommand = new DelegateCommand(OnGetOutHostPlayer);

            StartGameCommand = new DelegateCommand(OnStartGame, () => Host != null && (Players != null && Players.Any()))
                .ObservesProperty(() => Host)
                .ObservesProperty(() => Players);

            _eventAggregator.GetEvent<NumberOfPlayersInRoomIsUpdatedEvent>().Subscribe(e => OnUpdatePlayerList(e.RoomKey));
            _eventAggregator.GetEvent<HostPlayerUpdatedEvent>().Subscribe(e => OnUpdateHostPlayer(e.RoomKey));
            _eventAggregator.GetEvent<PlayerKickedOutEvent>().Subscribe(e => OnUpdateAllPlayers(e.RoomKey));
            _eventAggregator.GetEvent<GameIsStartedEvent>().Subscribe(e => OnUpdateGameStartingView(e.RoomKey));
        }

        /// <summary>
        /// Ключ комнаты
        /// </summary>
        public string RoomKey
        {
            get => _roomKey;
            set => this.RaiseAndSetIfChanged(ref _roomKey, value);
        }

        /// <summary>
        /// Игроки
        /// </summary>
        public ObservableCollection<PlayerModel?> Players
        {
            get => _players;
            set => this.RaiseAndSetIfChanged(ref _players, value);
        }

        /// <summary>
        /// Ведущий
        /// </summary>
        public PlayerModel? Host
        {
            get => _host;
            set => this.RaiseAndSetIfChanged(ref _host, value);
        }

        public ICommand AddPlayerCommand { get; }
        public ICommand KickOutPlayerCommand { get; }
        public ICommand SetPlayerToHostCommand { get; }
        public ICommand GetOutHostPlayerCommand { get; }
        public ICommand CreateRoomCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand MoveGoBackCommand { get; }
        public ICommand SendAnInvitationCommand { get; }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
        }

        /// <summary>
        /// Создать комнату
        /// </summary>
        private async Task OnCreateRoom()
        {
            if (!_telegramBotManager.IsConnected)
            {
                await _telegramBotManager.StartTelegramBot().ConfigureAwait(true);
            }

            if (!_telegramBotManager.IsConnected)
            {
                await _confirmationService.ShowInfoAsync("Ошибка", $"TelegramBotClient не запущен!");

                return;
            }

            Players = new ObservableCollection<PlayerModel?>();
            RoomKey = _gameManager.CreateRoom();
        }

        // Test method
        private void OnAddBot()
        {
            _eventAggregator.GetEvent<AddBotToRoomEvent>()
                .Publish(new AddBotToRoomEvent(_roomKey));
        }

        /// <summary>
        /// Выгнать игрока
        /// </summary>
        /// <param name="player"></param>
        private void OnKickOutPlayer(PlayerModel player)
        {
            _eventAggregator.GetEvent<KickOutPlayerEvent>().Publish(new KickOutPlayerEvent(_roomKey, player.Id));
        }

        /// <summary>
        /// Начать игру
        /// </summary>
        private void OnStartGame()
        {
            _eventAggregator.GetEvent<GameIsReadyToStartEvent>().Publish(new GameIsReadyToStartEvent(_roomKey));
        }

        /// <summary>
        /// Поставить игрока на место ведущего
        /// </summary>
        /// <param name="player"></param>
        private void OnSetPlayerToHost(PlayerModel player)
        {
            if (Players.Contains(player))
            {
                _eventAggregator.GetEvent<SetPlayerToHostEvent>().Publish(new SetPlayerToHostEvent(_roomKey, player.Id));
            }
        }


        private void OnGetOutHostPlayer()
        {
            if (Host is not null)
            {
                _eventAggregator.GetEvent<GetOutHostPlayerEvent>().Publish(new GetOutHostPlayerEvent(_roomKey));
            }
        }
        private void OnUpdateAllPlayers(string roomKey)
        {
            OnUpdatePlayerList(roomKey);
            OnUpdateHostPlayer(roomKey);
        }

        private void OnUpdatePlayerList(string roomKey)
        {
            if (roomKey != _roomKey)
            {
                return;
            }

            // если обновилась наша комната
            Players.Clear();
            Players.AddRange(_gameManager.GetPlayersFromRoom(roomKey));
            this.RaisePropertyChanged(nameof(Players));
        }

        private void OnUpdateHostPlayer(string roomKey)
        {
            if (roomKey != _roomKey)
            {
                return;
            }

            Host = _gameManager.GetHostPlayerFromRoom(roomKey);
            OnUpdatePlayerList(roomKey);
        }

        /// <summary>
        /// Перейти в игру
        /// </summary>
        /// <param name="roomKey"></param>
        private void OnUpdateGameStartingView(string roomKey)
        {
            if (roomKey != _roomKey)
            {
                return;
            }

            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.InitializeParameter, _roomKey
                }
            };

            RegionManager.RequestNavigate(RegionNameService.ShellRegionName, nameof(GameView), parameter);
        }

        /// <summary>
        /// Отправить приглашения
        /// </summary>
        private void OnSendAnInvitation()
        {
            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.InitializeParameter, _players
                }
            };

            RegionManager.RequestNavigate(RegionNameService.ShellRegionName, nameof(SendAnInvitationControlView), parameter);
        }

        private async void OnMoveGoBack()
        {
            // ToDo: Close game event
            ConfirmationResultEnum result = await _confirmationService.ShowInfoAsync(
                "Подтверждение",
                "Закрыть комнату?",
                ConfirmationResultEnum.Yes | ConfirmationResultEnum.No);

            if (result == ConfirmationResultEnum.Yes)
            {
                await _gameManager.CloseRoom(_roomKey).ConfigureAwait(true);

                RoomKey = null;
                Players = null;
                Host = null;
            }

            RegionManager.RequestNavigate(RegionNameService.ShellRegionName, "MainView");
        }

        private readonly IEventAggregator _eventAggregator;
        private readonly IConfirmationService _confirmationService;
        private readonly ITelegramBotManager _telegramBotManager;
        private readonly IGameManager _gameManager;
        private string _roomKey;
        private ObservableCollection<PlayerModel?> _players;
        private PlayerModel? _host;
    }
}