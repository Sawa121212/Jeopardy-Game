using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Common.Core.Prism;
using Common.Core.Views;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Services.Interfaces;

namespace TopicsDB.Infrastructure.Views.Questions
{
    public class AddNewQuestionViewModel : NavigationViewModelBase
    {
        /// <inheritdoc />
        public AddNewQuestionViewModel(IRegionManager regionManager, IQuestionService questionService) : base(regionManager)
        {
            _questionService = questionService;
            PriceIsChangedCommand = new DelegateCommand<string>(OnPriceIsChanged);
            CreateCommand = new DelegateCommand(OnCreate);
            ImportPictureComand = new DelegateCommand(async () => await OnImportPicture());
        }

        public Topic Topic
        {
            get => _topic;
            set => this.RaiseAndSetIfChanged(ref _topic, value);
        }

        public Question Question
        {
            get => _question;
            set => this.RaiseAndSetIfChanged(ref _question, value);
        }

        public string Text
        {
            get => _text;
            set => this.RaiseAndSetIfChanged(ref _text, value);
        }

        public string Picture
        {
            get => _picture;
            set => this.RaiseAndSetIfChanged(ref _picture, value);
        }

        public string Music
        {
            get => _music;
            set => this.RaiseAndSetIfChanged(ref _music, value);
        }

        public bool IsCreateMode { get; set; } = true;

        public ICommand PriceIsChangedCommand { get; }
        public ICommand ImportPictureComand { get; }
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
                    Question = new Question();
                    break;
                case Question question:
                    Question = question;
                    IsCreateMode = false;
                    break;
                default:
                    throw new Exception("Неправильный входной параметр");
                    break;
            }
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


        private async Task OnImportPicture()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
            {
                throw new NullReferenceException("Missing StorageProvider instance.");
            }

            // Start async operation to open the dialog.
            IReadOnlyList<IStorageFile> files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Выберите картинку",
                AllowMultiple = false
            }).ConfigureAwait(true);

            if (files.Count >= 1)
            {
                string filePath = files[0].TryGetLocalPath();
                if (File.Exists(filePath))
                {
                    Picture = filePath;
                }
            }
        }


        private bool CanExecuteMethod()
        {
            return false;
        }

        private readonly IQuestionService _questionService;
        private Topic _topic;
        private Question _question;
        private string _picture;
        private string _music;
        private string _text;
    }
}