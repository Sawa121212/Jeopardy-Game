using System.Windows.Input;
using Common.Core.Interfaces;
using Common.Core.Prism;
using Common.Core.Views;
using Common.Extensions;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Services.Interfaces;

namespace TopicsDB.Infrastructure.Views.Questions
{
    public class AddNewQuestionViewModel : NavigationViewModelBase, IInitializable
    {
        /// <inheritdoc />
        public AddNewQuestionViewModel(IRegionManager regionManager, IQuestionService questionService) : base(regionManager)
        {
            _questionService = questionService;
            PriceIsChangedCommand = new DelegateCommand<string>(OnPriceIsChanged);
            CreateCommand = new DelegateCommand(OnCreate);
        }

        public Topic Topic
        {
            get => _topic;
            private set => this.RaiseAndSetIfChanged(ref _topic, value);
        }

        public Question Question
        {
            get => _question;
            private set => this.RaiseAndSetIfChanged(ref _question, value);
        }
        
        public string Picture
        {
            get => _picture;
            set => this.RaiseAndSetIfChanged(ref _picture, value);
        }

        public string Music
        {
            get => _music;
            private set => this.RaiseAndSetIfChanged(ref _music, value);
        }

        public bool IsCreateMode { get; set; } = true;

        public ICommand PriceIsChangedCommand { get; }
        public ICommand CreateCommand { get; }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            object parameter = navigationContext.Parameters[NavigationParameterService.InitializeParameter];
            switch (parameter)
            {
                case Topic topic:
                    Topic = topic;
                    Initialize();
                    break;

                case Question question:
                    Question = question;
                    IsCreateMode = false;
                    break;
            }
        }

        /// <inheritdoc />
        public void Initialize()
        {
            Question = new Question();
        }

        private void OnPriceIsChanged(string price)
        {
            if (int.TryParse(price, out int priceValue))
            {
                Question.Price = priceValue;
            }
        }

        private void OnCreate()
        {
            /*Topic.Name = _name;

            if (IsCreateMode)
            {
                _questionService.CreateTopic(_topic);
            }
            else
            {
                _questionService.UpdateTopic(_topic);
            }
            */

            MoveBackCommand.Execute(null);
        }

        private bool CanExecuteMethod()
        {
            return false;
        }


        private Topic _topic;
        private Question _question;
        private readonly IQuestionService _questionService;
        private string _picture;
        private string _music;
    }
}