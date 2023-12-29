using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.Core.Prism;
using Common.Core.Views;
using DataDomain.Rooms;
using DataDomain.Rooms.Rounds;
using Game.Mangers;
using Prism.Regions;
using ReactiveUI;

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

            /*_roomKey = value;
            Game = _gameManager.GetGame(_roomKey);
            if (Game is {Rounds.Count: > 0})
            {
                Rounds.AddRange(_game.Rounds);
            }*/
        }

        private readonly IGameManager _gameManager;
        private string? _roomKey;
        private List<Round> _rounds;
        private List<Player> _players;
        private ObservableCollection<Topic> _topics;
        private DataDomain.Rooms.Game? _game;
    }
}