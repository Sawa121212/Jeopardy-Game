using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Common.Core.Prism;
using Common.Core.Views;
using DataDomain.Rooms;
using DataDomain.Rooms.Rounds;
using Game.Mangers;
using Prism.Regions;
using ReactiveUI;
using TopicsDB.Infrastructure.Services;

namespace Game.Views
{
    public class GameViewModel : NavigationViewModelBase
    {
        /// <inheritdoc />
        public GameViewModel(IRegionManager regionManager, IGameManager gameManager) : base(regionManager)
        {
            _gameManager = gameManager;
            //_gameManager.
            Rounds = new List<Round>(3);
            Topics = new ObservableCollection<Topic>();
        }

        public DataDomain.Rooms.Game? Game
        {
            get => _game;
            set => this.RaiseAndSetIfChanged(ref _game, value);
        }

        public List<Round> Rounds
        {
            get => _rounds;
            set => this.RaiseAndSetIfChanged(ref _rounds, value);
        }

        public ObservableCollection<Topic> Topics
        {
            get => _topics;
            set => this.RaiseAndSetIfChanged(ref _topics, value);
        }

        public ObservableCollection<Player> Players
        {
            get => _players;
            set => this.RaiseAndSetIfChanged(ref _players, value);
        }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            object parameter = navigationContext.Parameters[NavigationParameterService.InitializeParameter];
            string? value = parameter?.ToString();
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            _roomKey = value;
            Game = _gameManager.GetGame(_roomKey);

            if (Game == null || Game.Rounds.Count == 0)
            {
                return;
            }

            Rounds.AddRange(Game.Rounds);

            List<Topic>? collection = Rounds.FirstOrDefault(r => r.Level == Game.CurrentRound)?.Topics;
            if (collection != null)
            {
                Topics = new ObservableCollection<Topic>(collection);
            }

            Players = new ObservableCollection<Player>(_gameManager.GetPlayersFromRoom(_roomKey));
        }

        private readonly IGameManager _gameManager;
        private string? _roomKey;
        private List<Round> _rounds;
        private ObservableCollection<Player> _players;
        private ObservableCollection<Topic> _topics;
        private DataDomain.Rooms.Game? _game;
    }
}