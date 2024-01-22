using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.Core.Prism;
using Common.Core.Views;
using DataDomain.Rooms.Rounds;
using Prism.Regions;
using ReactiveUI;

namespace Game.Views.GamePages.Rounds
{
    public class BaseRoundControlViewModel : NavigationViewModelBase
    {
        /// <inheritdoc />
        public BaseRoundControlViewModel(IRegionManager regionManager) : base(regionManager)
        {
        }

        public ObservableCollection<TopicModel> Topics
        {
            get => _topics;
            set => this.RaiseAndSetIfChanged(ref _topics, value);
        }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            object parameter = navigationContext.Parameters[NavigationParameterService.InitializeParameter];

            if (parameter is IList<TopicModel> topicModels)
            {
                Topics = new ObservableCollection<TopicModel>(topicModels);
            }
        }

        private ObservableCollection<TopicModel> _topics;
    }
}