using Common.Core.Prism;
using Common.Core.Prism.Regions;
using Confirmation.Module.Enums;
using Prism.Commands;
using Prism.Regions;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Views.Questions;
using TopicsDB.Infrastructure.Views.Topics;

namespace TopicsDB.Infrastructure.Views
{
    public partial class TopicListViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        private void OnAddNewTopic()
        {
            RegionManager.RequestNavigate(RegionNameService.ContentRegionName, nameof(AddNewTopicView));
        }

        private void OnEditTopic(Topic topic)
        {
            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.InitializeParameter, topic
                }
            };

            RegionManager.RequestNavigate(RegionNameService.ContentRegionName, nameof(AddNewTopicView), parameter);
        }


        private async void OnDeleteTopic(Topic topic)
        {
            ConfirmationResultEnum result = await _confirmationService.ShowInfoAsync("Подтверждение",
                $"Вы действительно хотите удалить тему \"{topic.Name}\"?",
                ConfirmationResultEnum.Yes | ConfirmationResultEnum.No);

            if (result == ConfirmationResultEnum.Yes)
            {
                _topicService.DeleteTopic(topic);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        private void OnAddNewQuestion(Topic topic)
        {
            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.InitializeParameter, topic
                }
            };

            RegionManager.RequestNavigate(RegionNameService.ContentRegionName, nameof(AddNewQuestionView), parameter);
        }

        private void OnEditQuestion(Question question)
        {
            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.InitializeParameter, question
                }
            };

            RegionManager.RequestNavigate(RegionNameService.ContentRegionName, nameof(AddNewQuestionView), parameter);
        }


        private async void OnDeleteQuestion(Question question)
        {
            ConfirmationResultEnum result = await _confirmationService.ShowInfoAsync("Подтверждение",
                $"Вы действительно хотите удалить вопрос?",
                ConfirmationResultEnum.Yes | ConfirmationResultEnum.No);

            if (result == ConfirmationResultEnum.Yes)
            {
                _questionService.DeleteQuestion(question);
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            AddNewTopicCommand.RaiseCanExecuteChanged();
            EditTopicCommand.RaiseCanExecuteChanged();
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public DelegateCommand AddNewTopicCommand { get; }
        public DelegateCommand<Topic> EditTopicCommand { get; }
        public DelegateCommand<Topic> DeleteTopicCommand { get; }

        public DelegateCommand<Topic> AddNewQuestionCommand { get; }
        public DelegateCommand<Question> EditQuestionCommand { get; }
        public DelegateCommand<Question> DeleteQuestionCommand { get; }
    }
}