using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Common.Core.Prism;
using Common.Core.Prism.Regions;
using Common.Core.Views;
using Common.Core.Views.Interfaces;
using DataDomain.Rooms;
using Game.Events;
using Game.Mangers;
using Infrastructure.Domain.Helpers;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using ReactiveUI;

namespace Game.Views
{
    public class RoomViewModel : NavigationViewModelBase
    {
        /// <inheritdoc />
        public RoomViewModel(
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IGameManager gameManager)
            : base(regionManager)
        {
            _eventAggregator = eventAggregator;
            _gameManager = gameManager;
            MoveGoBackCommand = new DelegateCommand(OnMoveGoBack);
            CreateRoomCommand = new DelegateCommand(OnCreateRoom);
            AddPlayerCommand = new DelegateCommand(OnAddPlayer);
            KickOutPlayerCommand = new DelegateCommand<Player>(OnKickOutPlayer);
            SetPlayerToHostCommand = new DelegateCommand<Player>(OnSetPlayerToHost);
            StartGameCommand = new DelegateCommand(OnStartGame, () => Host != null && Players.Any())
                .ObservesProperty(() => Host)
                .ObservesProperty(() => Players);

            Players = new ObservableCollection<Player>();
            _eventAggregator.GetEvent<NumberOfPlayersInRoomIsUpdatedEvent>().Subscribe(e => OnUpdatePlayerList(e.RoomKey));
            _eventAggregator.GetEvent<HostPlayerUpdatedEvent>().Subscribe(e => OnUpdateHostPlayer(e.RoomKey));
            _eventAggregator.GetEvent<PlayerKickedOutEvent>().Subscribe(e => OnUpdateAllPlayers(e.RoomKey));
            _eventAggregator.GetEvent<GameIsStartedEvent>().Subscribe(e => OnUpdateGameStarting(e.RoomKey));
        }

        public string RoomKey
        {
            get => _roomKey;
            set => this.RaiseAndSetIfChanged(ref _roomKey, value);
        }

        public ObservableCollection<Player> Players
        {
            get => _players;
            set => this.RaiseAndSetIfChanged(ref _players, value);
        }

        public Player Host
        {
            get => _host;
            set => this.RaiseAndSetIfChanged(ref _host, value);
        }

        public ICommand AddPlayerCommand { get; }
        public ICommand KickOutPlayerCommand { get; }
        public ICommand SetPlayerToHostCommand { get; }
        public ICommand CreateRoomCommand { get; }
        public ICommand StartGameCommand { get; }
        public ICommand MoveGoBackCommand { get; }

        private void OnCreateRoom()
        {
            RoomKey = _gameManager.CreateRoom();
        }

        // Test
        private void OnAddPlayer()
        {
            Player player = new()
            {
                Id = RandomGenerator.GenerateSixDigitRandomNumber()
            };

            _eventAggregator.GetEvent<PlayerIsTryingToConnectToRoomEvent>()
                .Publish(new PlayerIsTryingToConnectToRoomEvent(_roomKey, player.Id));
        }

        /// <summary>
        /// Выгнать игрока
        /// </summary>
        /// <param name="player"></param>
        private void OnKickOutPlayer(Player player)
        {
            _eventAggregator.GetEvent<KickOutPlayerEvent>().Publish(new KickOutPlayerEvent(_roomKey, player.Id));
        }

        private void OnStartGame()
        {
            _eventAggregator.GetEvent<GameIsReadyToStartEvent>().Publish(new GameIsReadyToStartEvent(_roomKey));
        }

        private void OnSetPlayerToHost(Player player)
        {
            if (Players.Contains(player))
            {
                _eventAggregator.GetEvent<SetPlayerToHostEvent>().Publish(new SetPlayerToHostEvent(_roomKey, player.Id));
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

        private void OnUpdateGameStarting(string roomKey)
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


        private void OnMoveGoBack()
        {
            // ToDo: Close room event
            //_eventAggregator.GetEvent<Event>().Publish(new SetPlayerToHostEvent(_roomKey, player.Id));
            RegionManager.RequestNavigate(RegionNameService.ShellRegionName, "MainView");
        }

        private readonly IEventAggregator _eventAggregator;
        private readonly IGameManager _gameManager;
        private string _roomKey;
        private ObservableCollection<Player> _players;
        private Player _host;
    }
}