using Common.Core.Prism;
using Common.Core.Prism.Regions;
using Common.Core.Views;
using DataDomain.Rooms.Rounds;
using Game.Domain.Data;
using Prism.Regions;
using ReactiveUI;

namespace Game.Ui.Views.GameControls.Pages.GamePages.Rounds
{
    public class RoundLevelViewModel : NavigationViewModelBase
    {
        /// <inheritdoc />
        public RoundLevelViewModel(IRegionManager regionManager) : base(regionManager)
        {
        }

        public RoundModel Round
        {
            get => _round;
            private set => this.RaiseAndSetIfChanged(ref _round, value);
        }

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

        /// <inheritdoc />
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add(NavigationParameterService.ResultParameter, GameStatusEnum.ShowCurrentRound);
        }

        public override void GoBackOrder()
        {
            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.ResultParameter, GameStatusEnum.ShowCurrentRound
                }
            };

            RegionManager.RequestNavigate(RegionNameService.ShellRegionName, nameof(GameView), parameter);
        }

        private RoundModel _round;
    }
}