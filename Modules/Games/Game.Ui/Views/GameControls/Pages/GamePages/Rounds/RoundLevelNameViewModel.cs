using System.Windows.Input;
using Common.Core.Prism;
using Common.Core.Views;
using DataDomain.Rooms.Rounds;
using Game.Domain.Data;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;

namespace Game.Ui.Views.GameControls.Pages.GamePages.Rounds
{
    public class RoundLevelNameViewModel : NavigationViewModelBase
    {
        /// <inheritdoc />
        public RoundLevelNameViewModel(IRegionManager regionManager)
            : base(regionManager)
        {
            ContinueGameCommand = new DelegateCommand(OnContinueGame);
        }

        /// <summary>
        /// Текущий раунд
        /// </summary>
        public RoundModel Round
        {
            get => _round;
            private set => this.RaiseAndSetIfChanged(ref _round, value);
        }

        public ICommand ContinueGameCommand { get; }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            object parameter = navigationContext.Parameters[NavigationParameterService.InitializeParameter];

            if (parameter is RoundModel roundModel)
            {
                Round = roundModel;
            }
        }

        /// <summary>
        /// Продолжить игру
        /// </summary>
        private void OnContinueGame()
        {
            RegionManager.Regions[GameRegionNameService.GameTopLayerRegionName].RemoveAll();
        }

        private RoundModel _round;
    }
}