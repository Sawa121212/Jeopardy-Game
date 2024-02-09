using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Common.Core.Prism;
using Common.Core.Views;
using DataDomain.Rooms.Rounds;
using Game.Domain.Data;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;

namespace Game.Ui.Views.GameControls.GamePages.Topics
{
    public class TopicsNameCarouselControlViewModel : NavigationViewModelBase
    {
        /// <inheritdoc />
        public TopicsNameCarouselControlViewModel(IRegionManager regionManager) : base(regionManager)
        {
            ShowNextTopicCommand = new DelegateCommand(OnShowNextTopic);
            ContinueGameCommand = new DelegateCommand(OnContinueGame);
        }

        /// <summary>
        /// Отображаемая тема
        /// </summary>
        public TopicModel ShowedTopic
        {
            get => _showedTopic;
            set => this.RaiseAndSetIfChanged(ref _showedTopic, value);
        }

        /// <summary>
        /// Все темы показаны
        /// </summary>
        public bool IsShowedAllTopic
        {
            get => _isShowedAllTopic;
            set => this.RaiseAndSetIfChanged(ref _isShowedAllTopic, value);
        }

        public ICommand ShowNextTopicCommand { get; }
        public ICommand ContinueGameCommand { get; }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            _topics = default;
            ShowedTopic = default;

            object parameter = navigationContext.Parameters[NavigationParameterService.InitializeParameter];
            if (parameter is IList<TopicModel> topics)
            {
                _topics = new List<TopicModel>(topics);
                OnShowNextTopic();
            }

            IsShowedAllTopic = false;
        }

        /// <summary>
        /// Отобразить следующую тему
        /// </summary>
        private void OnShowNextTopic()
        {
            if (!_topics.Any())
            {
                return;
            }

            if (ShowedTopic is null)
            {
                ShowedTopic = _topics.First();
                return;
            }

            int nextIndex = _topics.IndexOf(_showedTopic) + 1;
            if (nextIndex >= _topics.Count)
            {
                return;
            }

            ShowedTopic = _topics[nextIndex];

            if (nextIndex + 1 == _topics.Count)
            {
                IsShowedAllTopic = true;
            }
        }

        /// <summary>
        /// Продолжить игру
        /// </summary>
        private void OnContinueGame()
        {
            MoveBackCommand.Execute(default);
        }

        /// <inheritdoc />
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add(NavigationParameterService.ResultParameter, GameStatusEnum.ShowCurrentRound);
        }

        private TopicModel _showedTopic;
        private List<TopicModel> _topics;
        private bool _isShowedAllTopic;
    }
}