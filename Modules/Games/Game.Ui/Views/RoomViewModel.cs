using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Core.Prism;
using Common.Core.Prism.Regions;
using Common.Core.Views;
using Confirmation.Module.Enums;
using Confirmation.Module.Services;
using DataDomain.Rooms;
using Game.Domain.Data;
using Game.Domain.Events.Games;
using Game.Domain.Events.Players;
using Game.Domain.Events.Rooms;
using Game.Infrastructure.Interfaces.Mangers;
using Game.Ui.Views.GameControls;
using Game.Ui.Views.GameControls.Pages;
using GameSender.Infrastructure.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using ReactiveUI;

namespace Game.Ui.Views
{
    public class RoomViewModel : NavigationViewModelBase
    {
        /// <inheritdoc />
        public RoomViewModel(
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IConfirmationService confirmationService,
            IGameManager gameManager,
            IGameSenderService gameSenderService)
            : base(regionManager)
        {
            _eventAggregator = eventAggregator;
            _confirmationService = confirmationService;
            _gameManager = gameManager;
            _gameSenderService = gameSenderService;

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

            _eventAggregator.GetEvent<NumberOfPlayersInRoomIsUpdatedEvent>().Subscribe(OnUpdateAllPlayer);
            _eventAggregator.GetEvent<HostPlayerUpdatedEvent>().Subscribe(OnUpdateAllPlayer);
            _eventAggregator.GetEvent<IsKickedOutPlayerEvent>().Subscribe(OnUpdateAllPlayer);
            _eventAggregator.GetEvent<IsStartedGameEvent>().Subscribe(OnUpdateGameStartingView);
        }

        /// <summary>
        /// Игроки
        /// </summary>
        public ObservableCollection<PlayerModel> Players
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

        /// <summary>
        /// Игра создана
        /// </summary>
        public bool IsCreated
        {
            get => _isCreated;
            set => this.RaiseAndSetIfChanged(ref _isCreated, value);
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
            if (_gameSenderService.IsReady())
            {
                IsCreated = _gameManager.CreateRoom();

                if (IsCreated)
                {
                    Players = new ObservableCollection<PlayerModel>();
                }
                else
                {
                    await _confirmationService.ShowErrorAsync("Ошибка", $"Не удалось создать комнату!");
                }
            }
            else
            {
                await _confirmationService.ShowErrorAsync("Ошибка", $"TelegramBotClient не запущен!");
            }
        }

        // Test method
        private void OnAddBot()
        {
            _eventAggregator.GetEvent<AddBotToRoomEvent>().Publish();
        }

        /// <summary>
        /// Выгнать игрока
        /// </summary>
        /// <param name="player"></param>
        private void OnKickOutPlayer(PlayerModel player)
        {
            _eventAggregator.GetEvent<KickOutPlayerEvent>().Publish(player.Id);
        }

        /// <summary>
        /// Начать игру
        /// </summary>
        private void OnStartGame()
        {
            _eventAggregator.GetEvent<GameIsReadyToStartEvent>().Publish();
        }

        /// <summary>
        /// Поставить игрока на место ведущего
        /// </summary>
        /// <param name="player"></param>
        private void OnSetPlayerToHost(PlayerModel player)
        {
            if (Players.Contains(player))
            {
                _gameManager.SetPlayerToHost(player.Id);
            }
        }

        private void OnGetOutHostPlayer()
        {
            if (Host is not null)
            {
                _gameManager.GetOutHostPlayer();
            }
        }

        private void OnUpdateAllPlayer()
        {
            // если обновилась наша комната`
            Players.Clear();
            Players.AddRange(_gameManager.GetPlayersFromRoom());

            Host = _gameManager.GetHostPlayerFromRoom();
        }

        /// <summary>
        /// Перейти в игру
        /// </summary>
        private void OnUpdateGameStartingView()
        {
            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.InitializeParameter, _
                }
            };

            RegionManager.RequestNavigate(GameRegionNameService.GameMainLayerRegionName, nameof(GameView), parameter);

            // send buttons
            Task.Run(async () => await SendEmptyButtonsForPlayers());
        }

        private async Task SendEmptyButtonsForPlayers()
        {
            string message = "Игра началась!";

            foreach (PlayerModel playerModel in Players)
            {
                await _gameSenderService.SendBaseGameButton(playerModel.Id, message);
            }

            await _gameSenderService.SendBaseGameButton(Host.Id, message);
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

            RegionManager.RequestNavigate(GameRegionNameService.GameMainLayerRegionName, nameof(SendAnInvitationControlView), parameter);
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
                await _gameManager.CloseRoom().ConfigureAwait(true);
                Players = null;
                Host = null;
            }

            RegionManager.RequestNavigate(RegionNameService.ShellRegionName, "MainView");
        }

        private readonly IEventAggregator _eventAggregator;
        private readonly IConfirmationService _confirmationService;
        private readonly IGameManager _gameManager;
        private readonly IGameSenderService _gameSenderService;
        private string _;
        private ObservableCollection<PlayerModel> _players;
        private PlayerModel? _host;
        private bool _isCreated;
    }
}