using System.Windows.Input;
using Common.Core.Interfaces;
using Common.Core.Prism;
using Common.Core.Views;
using Common.Extensions;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Interfaces.Services;

namespace TopicDb.Ui.Views.Topics
{
    public class AddNewTopicViewModel : NavigationViewModelBase, IInitializable
    {
        /// <inheritdoc />
        public AddNewTopicViewModel(IRegionManager regionManager, ITopicService topicService) : base(regionManager)
        {
            _topicService = topicService;
            CreateCommand = new DelegateCommand(OnCreate, () => !Name.IsNullOrEmpty() && !Name.IsWhiteSpace())
                .ObservesProperty(() => Name);
        }

        public Topic Topic
        {
            get => _topic;
            set => this.RaiseAndSetIfChanged(ref _topic, value);
        }

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public bool IsCreateMode { get; set; } = true;

        public ICommand CreateCommand { get; }

        /// <inheritdoc />
        public void Initialize()
        {
            Topic = new Topic();
        }

        private void OnCreate()
        {
            Topic.Name = _name.Trim();

            if (IsCreateMode)
            {
                _topicService.CreateTopic(_topic);
            }
            else
            {
                _topicService.UpdateTopic(_topic);
            }

            MoveBackCommand.Execute(null);
        }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            object id = navigationContext.Parameters[NavigationParameterService.InitializeParameter];
            if (id is Topic topic)
            {
                Topic = topic;
                Name = topic.Name;
                IsCreateMode = false;
            }
            else
            {
                Initialize();
            }
        }

        /// <inheritdoc />
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add(NavigationParameterService.ResultParameter, true);

            Name = default;
        }

        private Topic _topic;
        private string _name;
        private readonly ITopicService _topicService;
    }
}