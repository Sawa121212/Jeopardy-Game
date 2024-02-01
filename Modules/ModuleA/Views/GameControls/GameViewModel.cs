using System;
using System.Collections.ObjectModel;
using System.Linq;
using Common.Core.Prism;
using Common.Core.Views;
using DataDomain.Rooms;
using DataDomain.Rooms.Rounds;
using Game.Data;
using Game.Mangers;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;
using TelegramAPI.Test.Managers;
using TopicsDB.Infrastructure.Interfaces.Services;

namespace Game.Views.GameControls
{
    public partial class GameViewModel : NavigationViewModelBase
    {
        //ToDo: 1. Перед началом первого раунда игрокам в течение 5 секунд на главном мониторе демонстрируются все темы предстоящей игры (кроме тем финального раунда)
        //ToDo: 2. В подавляющем большинстве случаев выбор темы и стоимости вопроса первым осуществляет игрок за центральным столом, однако в случае,
        //если в игре участвует женщина, не занимающая центральный стол, право выбора первого вопроса отдают ей.
        //ToDo: 3. В случае если никто из игроков не дал правильного ответа, звучит специальный трёхтоновый звуковой сигнал. Ведущий озвучивает ответ
        /// <inheritdoc />
        public GameViewModel(
            IRegionManager regionManager,
            IGameManager gameManager,
            IQuestionService questionService,
            ITelegramBotService telegramBotService)
            : base(regionManager)
        {
            _gameManager = gameManager;
            _telegramBotService = telegramBotService;
            _questionService = questionService;

            ShowGameTopicsCommand = new DelegateCommand(OnShowAllTopics);
            ShowTopicsCarouselCommand = new DelegateCommand(OnShowTopicsCarousel);
            SelectQuestionAnswerCommand = new DelegateCommand<QuestionModel>(async (q) => await OnSelectQuestionAnswer(q));
            AnsweredQuestionCommand = new DelegateCommand<bool?>(async (b) => await OnAnsweredQuestion(b));
            NoAnsweredQuestionCommand = new DelegateCommand(OnNoAnsweredQuestionCommand);
            CloseQuestionCommand = new DelegateCommand(async () => await OnCloseQuestion());
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
        public ObservableCollection<RoundModel?> Rounds
        {
            get => _rounds;
            set => this.RaiseAndSetIfChanged(ref _rounds, value);
        }

        /// <summary>
        /// Ведущий
        /// </summary>
        public PlayerModel Host
        {
            get => _host;
            private set => this.RaiseAndSetIfChanged(ref _host, value);
        }

        public ObservableCollection<PlayerModel> Players
        {
            get => _players;
            private set => this.RaiseAndSetIfChanged(ref _players, value);
        }

        /// <summary>
        /// Игрок, выбирающий вопрос
        /// </summary>
        public PlayerModel ActivePlayer
        {
            get => _activePlayer;
            set => this.RaiseAndSetIfChanged(ref _activePlayer, value);
        }

        /// <summary>
        /// Отвечающий игрок
        /// </summary>
        public PlayerModel RespondingPlayer
        {
            get => _respondingPlayer;
            set => this.RaiseAndSetIfChanged(ref _respondingPlayer, value);
        }

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
        public ObservableCollection<TopicModel> Topics
        {
            get => _topics;
            private set => this.RaiseAndSetIfChanged(ref _topics, value);
        }

        /// <summary>
        /// Текущий раунд
        /// </summary>
        public QuestionModel DisplayedQuestion
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
                        OnShowRoundLevel();
                        return;
                    case GameStatusEnum.ShowCurrentRound:
                        IsShowedTopics = true;
                        OnShowCurrentRound();
                        RespondingPlayer = GetPlayerFirstChoosingTopic();
                        ActivePlayer = _respondingPlayer;
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

            IsGameStarted = false;
            _game = null;
            Rounds = new ObservableCollection<RoundModel?>();
            RespondingPlayer = null;

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
            Players = new ObservableCollection<PlayerModel>(_gameManager.GetPlayersFromRoom(_roomKey));
            Host = _gameManager.GetHostPlayerFromRoom(_roomKey);

            OnChangeRound();
        }

        /// <summary>
        /// Получить игрока, который будет выбирать вопрос первым
        /// </summary>
        /// <returns></returns>
        private PlayerModel GetPlayerFirstChoosingTopic()
        {
            if (!_players.Any())
            {
                return null;
            }

            if (_players.Count is 1 or 2)
            {
                return _players[0];
            }

            int ceiling = (int) Math.Ceiling((double) _players.Count / 2) - 1;
            PlayerModel playerModel = _players[ceiling];

            Message = $"Выбор темы и стоимости вопроса первым осуществляет игрок {playerModel.Name}";

            return playerModel;
        }

        /// <summary>
        /// Текущая игра
        /// </summary>
        private GameModel? _game;

        private string? _roomKey;
        private ObservableCollection<RoundModel?> _rounds;
        private ObservableCollection<PlayerModel> _players;
        private ObservableCollection<TopicModel> _topics;
        private bool _isShowingTopics;
        private RoundModel? _currentRound;
        private string _message;
        private PlayerModel _activePlayer;
        private PlayerModel _host;

        private readonly IGameManager _gameManager;
        private QuestionModel _displayedQuestion;
        private PlayerModel _respondingPlayer;
        private bool _isGameStarted;
    }
}