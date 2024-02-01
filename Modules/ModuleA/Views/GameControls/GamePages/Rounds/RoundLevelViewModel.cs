using Common.Core.Prism;
using Common.Core.Views;
using DataDomain.Rooms.Rounds;
using Game.Data;
using Prism.Regions;
using ReactiveUI;

namespace Game.Views.GameControls.GamePages.Rounds
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

        private RoundModel _round;
    }
}