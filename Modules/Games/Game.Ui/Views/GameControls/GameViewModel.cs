using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Common.Core.Prism;
using Common.Core.Views;
using DataDomain.Rooms;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Enums;
using Game.Domain.Data;
using Game.Domain.Events.Questions;
using Game.Infrastructure.Interfaces.Mangers;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using ReactiveUI;
using TelegramAPI.Test.Managers;
using TopicsDB.Infrastructure.Interfaces.Services;

namespace Game.Ui.Views.GameControls
{
    public partial class GameViewModel : NavigationViewModelBase
    {
        //ToDo: звуковой сигнал

        /// <inheritdoc />
        public GameViewModel(
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IGameManager gameManager,
            IQuestionService questionService,
            ITelegramBotService telegramBotService)
            : base(regionManager)
        {
            _gameManager = gameManager;
            _eventAggregator = eventAggregator;
            _telegramBotService = telegramBotService;
            _questionService = questionService;

            _eventAggregator.GetEvent<PlayerIsReadyAnswerQuestionEvent>().Subscribe(e => OnPlayerIsReadyAnswerQuestion(e));

            ShowGameTopicsCommand = new DelegateCommand(OnShowAllTopics);
            ShowTopicsCarouselCommand = new DelegateCommand(OnShowTopicsCarousel);
            SelectQuestionAnswerCommand = new DelegateCommand<QuestionModel?>(async (q) => await OnSelectAndShowQuestionAnswer(q));
            AnsweredQuestionCommand = new DelegateCommand<bool?>(async (b) => await OnAnsweredQuestion(b));
            NoAnsweredQuestionCommand = new DelegateCommand(OnNoAnsweredQuestionCommand);
            CloseQuestionCommand = new DelegateCommand(async () => await OnCloseQuestion());

            // Final round
            RemoveTopicFromFinalRoundCommand = new DelegateCommand<TopicModel>(async (t) => await OnRemoveTopicFromFinalRound(t));
            EndPlaceBetsCommand = new DelegateCommand(async() => await OnEndPlaceBets());
        }

        /// <summary>
        /// Начата ли игра
        /// </summary>
        public bool IsGameStarted
        {
            get => _isGameStarted;
            private set => this.RaiseAndSetIfChanged(ref _isGameStarted, value);
        }

        /// <summary>
        /// Раунды
        /// </summary>
        public ObservableCollection<RoundModel?>? Rounds
        {
            get => _rounds;
            set => this.RaiseAndSetIfChanged(ref _rounds, value);
        }

        /// <summary>
        /// Ведущий
        /// </summary>
        public PlayerModel? Host
        {
            get => _host;
            private set => this.RaiseAndSetIfChanged(ref _host, value);
        }

        /// <summary>
        /// Игроки
        /// </summary>
        public ObservableCollection<PlayerModel?>? Players
        {
            get => _players;
            private set => this.RaiseAndSetIfChanged(ref _players, value);
        }

        /// <summary>
        /// Игрок, выбирающий вопрос
        /// </summary>
        public PlayerModel? ActivePlayer
        {
            get => _activePlayer;
            set => this.RaiseAndSetIfChanged(ref _activePlayer, value);
        }

        /// <summary>
        /// Резервная копия игрока, выбирающего вопрос
        /// </summary>
        private PlayerModel? ActivePlayerBackup { get; set; }

        /// <summary>
        /// Текущий раунд
        /// </summary>
        public RoundModel? CurrentRound
        {
            get => _currentRound;
            set => this.RaiseAndSetIfChanged(ref _currentRound, value);
        }

        /// <summary>
        /// Темы вопросов
        /// </summary>
        public ObservableCollection<TopicModel>? Topics
        {
            get => _topics;
            private set => this.RaiseAndSetIfChanged(ref _topics, value);
        }

        /// <summary>
        /// Текущий раунд
        /// </summary>
        public QuestionModel? DisplayedQuestion
        {
            get => _displayedQuestion;
            set => this.RaiseAndSetIfChanged(ref _displayedQuestion, value);
        }

        /// <summary>
        /// Показаны ли темы вопросов текущего раунда
        /// </summary>
        public bool IsShowedTopics
        {
            get => _isShowingTopics;
            private set => this.RaiseAndSetIfChanged(ref _isShowingTopics, value);
        }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            // result parameter
            object resultParameter = navigationContext.Parameters[NavigationParameterService.ResultParameter];
            if (resultParameter is GameStatusEnum gameStatus)
            {
                switch (gameStatus)
                {
                    case GameStatusEnum.Continue:
                        return;
                    case GameStatusEnum.ShowRoundLevel:
                        OnShowRoundLevelInformation();
                        return;
                    case GameStatusEnum.ShowCurrentRound:
                        IsShowedTopics = true;
                        OnShowCurrentRound();
                        SetPlayerFirstChoosingTopic();
                        return;
                    case GameStatusEnum.GoNextRound:
                        OnGoNextRound();
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Initialize parameter
            object parameter = navigationContext.Parameters[NavigationParameterService.InitializeParameter];
            string? value = parameter?.ToString();
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            ClearAllParameters();

            Rounds = new ObservableCollection<RoundModel?>();

            _roomKey = value;
            _game = _gameManager.GetGame(_roomKey);

            if (_game is null)
            {
                Message = "Ошибка. Игра не найдена";
                return;
            }


            if (_game?.Rounds is null || _game.Rounds.Count == 0)
            {
                Message = "Ошибка. Не удалось собрать раунд";
                return;
            }

            Rounds = new ObservableCollection<RoundModel?>(_game.Rounds);
            Players = new ObservableCollection<PlayerModel?>(_gameManager.GetPlayersFromRoom(_roomKey));
            Host = _gameManager.GetHostPlayerFromRoom(_roomKey);

            // Test.Remove
            _game.CurrentRoundLevel = RoundsLevelEnum.Final;

            OnChangeRound();
        }

        /// <summary>
        /// Получить игрока, который будет выбирать вопрос первым
        /// </summary>
        /// <param name="sortedPlayers"></param>
        /// <returns></returns>
        private void SetPlayerFirstChoosingTopic(List<PlayerModel?> players = null)
        {
            if (_players == null || !_players.Any())
            {
                return;
            }

            if (_currentRound == null)
            {
                return;
            }

            switch (_currentRound.Level)
            {
                case RoundsLevelEnum.Round1:
                    // выбор темы и стоимости вопроса первым осуществляет игрок за центральным столом
                    if (_players.Count is 1 or 2)
                    {
                        ActivePlayer = _players[0];
                        return;
                    }

                    int ceiling = (int) Math.Ceiling((double) _players.Count / 2) - 1;
                    PlayerModel? playerModel = _players[ceiling];

                    Message = $"Выбор темы и стоимости вопроса первым осуществляет игрок {playerModel?.Name}";
                    ActivePlayer = playerModel;
                    break;
                case RoundsLevelEnum.Round2:
                case RoundsLevelEnum.Round3:
                case RoundsLevelEnum.Final:
                    // раунд начинает игрок с наименьшим на начало раунда количеством очков
                    if (_currentRound is {Level: not RoundsLevelEnum.Round1})
                    {
                        ActivePlayer = GetPlayerWithMinPoint(players);
                        return;
                    }

                    break;
                case RoundsLevelEnum.Shootout:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Текущая игра
        /// </summary>
        private GameModel? _game;

        /// <summary>
        /// Ключ комнаты
        /// </summary>
        private string? _roomKey;

        private readonly IGameManager _gameManager;
        private readonly IEventAggregator _eventAggregator;

        private ObservableCollection<RoundModel?>? _rounds;
        private ObservableCollection<PlayerModel?>? _players;
        private ObservableCollection<TopicModel>? _topics;
        private bool _isShowingTopics;
        private bool _isGameStarted;
        private RoundModel? _currentRound;
        private PlayerModel? _activePlayer;
        private PlayerModel? _host;
        private QuestionModel? _displayedQuestion;
        private string _message;
    }
}