using System.Collections.ObjectModel;
using System.Windows.Input;
using Common.Core.Views;
using Confirmation.Module.Services;
using DataDomain.Rooms;
using DataDomain.Rooms.Rounds;
using Game.Domain.Events.Questions;
using Game.Infrastructure.Interfaces.Mangers;
using GameSender.Infrastructure.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using ReactiveUI;
using TopicsDB.Infrastructure.Interfaces.Services;
using Users.Infrastructure.Interfaces;

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
            IConfirmationService confirmationService,
            IGameSenderService gameSenderService,
            IUserService userService)
            : base(regionManager)
        {
            _gameManager = gameManager;
            _confirmationService = confirmationService;
            _gameSenderService = gameSenderService;
            _userService = userService;
            _questionService = questionService;

            eventAggregator.GetEvent<PlayerIsReadyAnswerQuestionEvent>().Subscribe(OnPlayerIsReadyAnswerQuestion);

            MoveBackButtonCommand = new DelegateCommand(async () => await GoBackOrderAsync());

            StartGameCommand = new DelegateCommand(OnShowAllTopicsView);
            ShowTopicsCarouselCommand = new DelegateCommand(OnShowTopicsCarouselView);

            // Question
            SelectQuestionAnswerCommand = new DelegateCommand<QuestionModel?>(async (q) => await OnSelectAndShowQuestionAnswer(q));
            GongButtonClickedCommand = new DelegateCommand(async () => await GongButtonClickedAsync());

            // Answered
            AnsweredQuestionCommand = new DelegateCommand<bool?>(async (value) => await OnAnsweredQuestion(value), _ => ActivePlayer != null)
                .ObservesProperty(() => ActivePlayer);
            NoAnsweredQuestionCommand = new DelegateCommand(async () => await OnNoAnsweredQuestion());
            CloseQuestionCommand = new DelegateCommand(async () => await OnCloseQuestion());

            // Final round
            RemoveTopicFromFinalRoundCommand = new DelegateCommand<TopicModel>(async (t) => await OnRemoveTopicFromFinalRound(t));
            SetPlayerBetsCommand = new DelegateCommand(OnSetPlayerBets);
            EndPlaceBetsCommand = new DelegateCommand(async () => await OnEndPlaceBets());
        }

        /// <summary>
        /// Раунды
        /// </summary>
        public ObservableCollection<RoundModel>? Rounds
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
        public ObservableCollection<PlayerModel> Players
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
            private set => this.RaiseAndSetIfChanged(ref _message, value);
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
        /// Игра готова принимать ответы
        /// </summary>
        private bool IsReadyGameToReceiveAnswers
        {
            get => _isReadyGameToReceiveAnswers;
            set => this.RaiseAndSetIfChanged(ref _isReadyGameToReceiveAnswers, value);
        }
        
        /// <summary>
        /// Гонг
        /// </summary>
        private bool IsVisibleGongButton
        {
            get => _isVisibleGongButton;
            set => this.RaiseAndSetIfChanged(ref _isVisibleGongButton, value);
        }

        /// <summary>
        /// Текущая игра
        /// </summary>
        private GameModel? _game;

        private readonly IGameManager _gameManager;
        private readonly IConfirmationService _confirmationService;
        private readonly IGameSenderService _gameSenderService;
        private readonly IUserService _userService;

        private ObservableCollection<RoundModel> _rounds;
        private ObservableCollection<PlayerModel> _players;
        private ObservableCollection<TopicModel> _topics;
        private bool _isShowingTopics;
        private bool _isGameStarted;
        private RoundModel? _currentRound;
        private PlayerModel? _activePlayer;
        private PlayerModel? _host;
        private QuestionModel? _displayedQuestion;
        private string _message;
        private bool _isVisibleGongButton;
    }
}