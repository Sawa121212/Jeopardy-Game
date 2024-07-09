using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Common.Core.Prism;
using Common.Core.Prism.Regions;
using Common.Core.Views;
using DataDomain.Rooms;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;

namespace Game.Ui.Views.GameControls.Pages.GamePages
{
    public class GameWinnerViewModel : NavigationViewModelBase
    {
        private PlayerModel? _player;
        private ObservableCollection<PlayerModel>? _players;

        /// <inheritdoc />
        public GameWinnerViewModel(IRegionManager regionManager) : base(regionManager)
        {
            EndGameCommand = new DelegateCommand(OnEndGame);
        }

        public ObservableCollection<PlayerModel>? Players
        {
            get => _players;
            private set => this.RaiseAndSetIfChanged(ref _players, value);
        }

        public PlayerModel? PlayerWinner
        {
            get => _player;
            private set => this.RaiseAndSetIfChanged(ref _player, value);
        }

        public ICommand EndGameCommand { get; }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            // result parameter
            object resultParameter = navigationContext.Parameters[NavigationParameterService.InitializeParameter];
            if (resultParameter is not IList<PlayerModel> playerModels)
            {
                return;
            }

            Players = new ObservableCollection<PlayerModel>(playerModels);
            SetWinner();
        }

        /// <inheritdoc />
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Players = null;
            PlayerWinner = null;
        }

        private void SetWinner()
        {
            if (_players == null)
            {
                return;
            }

            int maxPoint = _players.Max(p => p.Points);
            List<PlayerModel> playerModels = _players.Where(p => p.Points == maxPoint).ToList();
            if (playerModels.Count == 1)
            {
                PlayerWinner = playerModels.First();
            }
        }

        private void OnEndGame()
        {
            RegionManager.RequestNavigate(RegionNameService.ShellRegionName, nameof(RoomView));
        }
    }
}