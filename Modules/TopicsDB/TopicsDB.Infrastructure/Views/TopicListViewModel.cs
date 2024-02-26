using System.Collections.ObjectModel;
using Common.Core.Views;
using Confirmation.Module.Services;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Interfaces.Services;

namespace TopicsDB.Infrastructure.Views
{
    public partial class TopicListViewModel : NavigationViewModelBase
    {
        public TopicListViewModel(
            IRegionManager regionManager,
            IConfirmationService confirmationService,
            ITopicService topicService,
            IQuestionService questionService)
            : base(regionManager)
        {
            _confirmationService = confirmationService;
            _topicService = topicService;
            _questionService = questionService;
            Topics = new ObservableCollection<Topic>();
            Questions = new ObservableCollection<Question>();

            // topic commands
            AddNewTopicCommand = new DelegateCommand(OnAddNewTopic);
            EditTopicCommand = new DelegateCommand<Topic>(OnEditTopic);
            DeleteTopicCommand = new DelegateCommand<Topic>(OnDeleteTopic);

            //question commands
            AddNewQuestionCommand = new DelegateCommand<Topic>(OnAddNewQuestion);
            EditQuestionCommand = new DelegateCommand<Question>(OnEditQuestion);
            DeleteQuestionCommand = new DelegateCommand<Question>(OnDeleteQuestion);

            // search commands
            FindTopicsCommand = new DelegateCommand(OnFindTopics);
            ClearFoundElementsCommand = new DelegateCommand(OnClearFoundElements);
            UpdateTopicsInformation();
        }

        public ObservableCollection<Topic> Topics
        {
            get => _topics;
            set => this.RaiseAndSetIfChanged(ref _topics, value);
        }

        public ObservableCollection<Question> Questions
        {
            get => _questions;
            set => this.RaiseAndSetIfChanged(ref _questions, value);
        }

        private void UpdateTopicsInformation()
        {
            Topics.Clear();
            foreach (Topic customer in _topicService.GetAllTopics())
            {
                Topics.Add(customer);
            }

            Questions.Clear();
            Questions.AddRange(_questionService.GetAllQuestions());
        }

        private ObservableCollection<Topic> _topics;

        private readonly IConfirmationService _confirmationService;
        private readonly ITopicService _topicService;
        private readonly IQuestionService _questionService;
        private ObservableCollection<Question> _questions;
    }
}