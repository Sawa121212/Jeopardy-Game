using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Common.Core.Prism;
using Common.Core.Prism.Regions;
using Common.Core.Views;
using DataDomain.Rooms.Rounds;
using Game.Domain.Data;
using Game.Ui.Views.GameControls.Pages.GamePages.Rounds;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;

namespace Game.Ui.Views.GameControls.Pages.GamePages.Topics
{
    public class AllTopicsNameViewModel : NavigationViewModelBase
    {
        /// <inheritdoc />
        public AllTopicsNameViewModel(IRegionManager regionManager)
            : base(regionManager)
        {
            ContinueGameCommand = new DelegateCommand(OnContinueGame);
        }

        /// <summary>
        /// Отображаемая тема
        /// </summary>
        public ObservableCollection<TopicModel> Topics
        {
            get => _topics;
            set => this.RaiseAndSetIfChanged(ref _topics, value);
        }

        public ICommand ContinueGameCommand { get; }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            Topics = new ObservableCollection<TopicModel>();

            object parameter = navigationContext.Parameters[NavigationParameterService.InitializeParameter];

            if (parameter is not IList<RoundModel> roundModels || !roundModels.Any())
            {
                return;
            }

            for (int i = 0; i < 3; i++)
            {
                Topics.AddRange(roundModels[i].Topics);
            }

            _firstRoundModel = roundModels.First();
        }

        /// <summary>
        /// Продолжить игру
        /// </summary>
        private void OnContinueGame()
        {
            // 1. Change ContentRegion
            MoveBackCommand.Execute(default);

            // 2. Change ShellRegion
            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.InitializeParameter, _firstRoundModel
                }
            };

            RegionManager.RequestNavigate(RegionNameService.ShellRegionName, nameof(RoundLevelView), parameter);
        }

        /// <inheritdoc />
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add(NavigationParameterService.ResultParameter, GameStatusEnum.ShowRoundLevel);
        }

        private ObservableCollection<TopicModel> _topics;
        private RoundModel _firstRoundModel;
    }
}