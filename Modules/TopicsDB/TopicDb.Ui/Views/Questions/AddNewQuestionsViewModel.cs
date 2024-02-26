using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Platform.Storage;
using Common.Core.Prism;
using Common.Core.Views;
using Common.Extensions;
using Notification.Module.Services;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;
using Telegram.Bot.Types;
using TelegramAPI.Test.Managers;
using TelegramAPI.Test.Services.Settings;
using TopicDb.Domain.Models;
using TopicDb.Domain.Models.QuestionAttachments;
using TopicsDB.Infrastructure.Interfaces.Services;
using File = System.IO.File;

namespace TopicDb.Ui.Views.Questions
{
    public class AddNewQuestionViewModel : NavigationViewModelBase
    {
        /// <inheritdoc />
        public AddNewQuestionViewModel(
            IRegionManager regionManager,
            INotificationService notificationService,
            ITelegramSettingsService telegramSettingsService,
            IQuestionService questionService,
            ITelegramBotService telegramBotService)
            : base(regionManager)
        {
            _questionService = questionService;
            _telegramBotService = telegramBotService;
            _telegramSettingsService = telegramSettingsService;
            _notificationService = notificationService;
            PriceIsChangedCommand = new DelegateCommand<string>(OnPriceIsChanged);
            CreateCommand = new DelegateCommand(OnCreate, CanExecuteMethod)
                .ObservesProperty(() => Text)
                .ObservesProperty(() => CorrectAnswer);
            ImportPictureCommand = new DelegateCommand(async () => await OnImportPicture(), () => ChatId > 0)
                .ObservesProperty(() => ChatId);
            ImportMusicCommand = new DelegateCommand(async () => await OnImportMusic(), () => ChatId > 0)
                .ObservesProperty(() => ChatId);
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

        /// <summary>
        /// Текст
        /// </summary>
        public string Text
        {
            get => _text;
            set => this.RaiseAndSetIfChanged(ref _text, value);
        }

        public string CorrectAnswer
        {
            get => _correctAnswer;
            set => this.RaiseAndSetIfChanged(ref _correctAnswer, value);
        }

        /// <summary>
        /// Рисунок
        /// </summary>
        public string? PictureUrl
        {
            get => _pictureUrl;
            set => this.RaiseAndSetIfChanged(ref _pictureUrl, value);
        }

        public string? MusicUrl
        {
            get => _musicUrl;
            set => this.RaiseAndSetIfChanged(ref _musicUrl, value);
        }

        public long PictureMessageId
        {
            get => _pictureMessageId;
            private set => this.RaiseAndSetIfChanged(ref _pictureMessageId, value);
        }

        public long MusicMessageId
        {
            get => _musicMessageId;
            private set => this.RaiseAndSetIfChanged(ref _musicMessageId, value);
        }

        public int Price
        {
            get => _price;
            set => this.RaiseAndSetIfChanged(ref _price, value);
        }

        public bool IsCreateMode { get; set; } = true;

        public long ChatId
        {
            get => _chatId;
            set => this.RaiseAndSetIfChanged(ref _chatId, value);
        }

        public bool IsSelected100Points
        {
            get => _isSelected100Points;
            set => this.RaiseAndSetIfChanged(ref _isSelected100Points, value);
        }

        public ICommand PriceIsChangedCommand { get; }
        public ICommand ImportPictureCommand { get; }
        public ICommand ImportMusicCommand { get; }
        public ICommand CreateCommand { get; }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            object parameter = navigationContext.Parameters[NavigationParameterService.InitializeParameter];
            switch (parameter)
            {
                case Topic topic:
                    IsCreateMode = true;
                    Topic = topic;
                    OnPriceIsChanged("100");
                    IsSelected100Points = true;
                    Text = default;
                    CorrectAnswer = default;

                    //attachments
                    PictureMessageId = default;
                    PictureUrl = default;
                    MusicMessageId = default;
                    MusicUrl = default;
                    break;
                case Question question:
                    Question = question;
                    IsCreateMode = false;
                    if (question.Picture is not null)
                    {
                        PictureMessageId = question.Picture.MessageId;
                    }

                    if (question.Music is not null)
                    {
                        MusicMessageId = question.Music.MessageId;
                    }

                    break;
                default:
                    throw new Exception("Неправильный входной параметр");
                    break;
            }

            long chatId = _telegramSettingsService.GetAdminUserId();
            if (chatId != -1)
            {
                ChatId = chatId;
            }
            else
            {
                _notificationService.Show("Error", "Не указан аккаунт администратора", NotificationType.Warning);
            }
        }

        private void OnPriceIsChanged(string price)
        {
            if (int.TryParse(price, out int priceValue))
            {
                Price = priceValue;
            }
        }

        private void OnCreate()
        {
            if (IsCreateMode)
            {
                _question = new Question()
                {
                    TopicId = _topic.Id,
                    Text = _text,
                    CorrectAnswer = _correctAnswer,
                    Price = _price
                };

                if (PictureMessageId > 0)
                {
                    _question.Picture = new Picture()
                    {
                        ChatId = _chatId,
                        MessageId = _pictureMessageId
                    };
                }

                if (MusicMessageId > 0)
                {
                    _question.Music = new Music()
                    {
                        ChatId = _chatId,
                        MessageId = _pictureMessageId
                    };
                }

                _questionService.CreateQuestion(_question);
            }
            else
            {
                _questionService.UpdateQuestion(_question.Id, _question);
            }

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

                // ToDo: Filter
            }).ConfigureAwait(true);

            if (files.Count >= 1)
            {
                string? filePath = files[0].TryGetLocalPath();
                if (File.Exists(filePath))
                {
                    PictureUrl = filePath;
                    if (PictureUrl != null)
                    {
                        Message? message = await _telegramBotService.SendPhotoAsync(_chatId, PictureUrl, _text);
                        if (message is not null)
                        {
                            PictureMessageId = message.MessageId;
                        }
                    }
                }
            }
        }

        private async Task OnImportMusic()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
            {
                throw new NullReferenceException("Missing StorageProvider instance.");
            }

            // Start async operation to open the dialog.
            IReadOnlyList<IStorageFile> files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Выберите музыку",
                AllowMultiple = false

                // ToDo: Filter
            }).ConfigureAwait(true);

            if (files.Count >= 1)
            {
                string? filePath = files[0].TryGetLocalPath();
                if (File.Exists(filePath))
                {
                    MusicUrl = filePath;
                    if (MusicUrl != null)
                    {
                        Message? message = await _telegramBotService.SendPhotoAsync(_chatId, MusicUrl, _text);
                        if (message is not null)
                        {
                            MusicMessageId = message.MessageId;
                        }
                    }
                }
            }
        }

        private bool CanExecuteMethod()
        {
            return !_text.IsNullOrEmpty() && !_correctAnswer.IsNullOrEmpty() && _price != 0;
        }

        /// <inheritdoc />
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add(NavigationParameterService.ResultParameter, true);
        }

        private readonly INotificationService _notificationService;
        private readonly ITelegramSettingsService _telegramSettingsService;
        private readonly ITelegramBotService _telegramBotService;
        private readonly IQuestionService _questionService;
        private long _chatId;
        private Topic _topic;
        private Question _question;
        private string? _pictureUrl;
        private string? _musicUrl;
        private string _text;
        private string _correctAnswer;
        private int _price;
        private long _pictureMessageId;
        private long _musicMessageId;
        private bool _isSelected100Points;
    }
}