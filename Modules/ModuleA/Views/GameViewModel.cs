using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Common.Core.Prism;
using Common.Core.Prism.Regions;
using Common.Core.Views;
using DataDomain.Rooms;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Helpers;
using Game.Data;
using Game.Mangers;
using Game.Views.GamePages;
using Prism.Commands;
using Prism.Regions;
using ReactiveUI;

namespace Game.Views
{
    public class GameViewModel : NavigationViewModelBase
    {
        //ToDo: 1. Перед началом первого раунда игрокам в течение 5 секунд на главном мониторе демонстрируются все темы предстоящей игры (кроме тем финального раунда)
        //ToDo: 2. В подавляющем большинстве случаев выбор темы и стоимости вопроса первым осуществляет игрок за центральным столом, однако в случае,
        //если в игре участвует женщина, не занимающая центральный стол, право выбора первого вопроса отдают ей.
        //ToDo: 3. В случае если никто из игроков не дал правильного ответа, звучит специальный трёхтоновый звуковой сигнал. Ведущий озвучивает ответ
        /// <inheritdoc />
        public GameViewModel(IRegionManager regionManager, IGameManager gameManager) : base(regionManager)
        {
            _gameManager = gameManager;
            ShowTopicsCarouselCommand = new DelegateCommand(OnShowTopicsCarousel);
        }

        public GameModel? Game
        {
            get => _game;
            set => this.RaiseAndSetIfChanged(ref _game, value);
        }

        public ObservableCollection<RoundModel?> Rounds
        {
            get => _rounds;
            set => this.RaiseAndSetIfChanged(ref _rounds, value);
        }

        public ObservableCollection<PlayerModel> Players
        {
            get => _players;
            set => this.RaiseAndSetIfChanged(ref _players, value);
        }

        public RoundModel? CurrentRound
        {
            get => _currentRound;
            set => this.RaiseAndSetIfChanged(ref _currentRound, value);
        }

        public ObservableCollection<TopicModel> Topics
        {
            get => _topics;
            set => this.RaiseAndSetIfChanged(ref _topics, value);
        }

        public bool IsShowedTopics
        {
            get => _isShowingTopics;
            set => this.RaiseAndSetIfChanged(ref _isShowingTopics, value);
        }

        public ICommand ShowTopicsCarouselCommand { get; }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            object resultParameter = navigationContext.Parameters[NavigationParameterService.ResultParameter];
            if (resultParameter is bool IsContinue && IsContinue)
            {
                return;
            }

            object parameter = navigationContext.Parameters[NavigationParameterService.InitializeParameter];
            string? value = parameter?.ToString();
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            Game = null;
            Rounds = new ObservableCollection<RoundModel?>();

            _roomKey = value;
            Game = _gameManager.GetGame(_roomKey);

            if (Game?.Rounds is null || Game.Rounds.Count == 0)
            {
                return;
            }

            Rounds = new ObservableCollection<RoundModel?>(Game.Rounds);
            Players = new ObservableCollection<PlayerModel>(_gameManager.GetPlayersFromRoom(_roomKey));

            ChangeRound();

            IsShowedTopics = true;
        }

        private void GoNextRound()
        {
            Game.CurrentRound = RoundHelper.GetNextRoundLevel(Game.CurrentRound);
            ChangeRound();
        }

        private void ChangeRound()
        {
            IsShowedTopics = false;
            IList<TopicModel>? topicModels = Rounds.FirstOrDefault(r => r.Level == Game.CurrentRound)?.Topics;
            if (topicModels != null)
            {
                Topics = new ObservableCollection<TopicModel>(topicModels);
            }
        }

        private void OnShowTopicsCarousel()
        {
            NavigationParameters parameter = new()
            {
                {
                    NavigationParameterService.InitializeParameter, _topics
                }
            };

            RegionManager.RequestNavigate(RegionNameService.ShellRegionName, nameof(TopicsNameCarouselView), parameter);
            IsShowedTopics = true;
        }

        private readonly IGameManager _gameManager;
        private string? _roomKey;
        private ObservableCollection<RoundModel?> _rounds;
        private ObservableCollection<PlayerModel> _players;
        private ObservableCollection<TopicModel> _topics;
        private GameModel? _game;
        private bool _isShowingTopics;
        private RoundModel? _currentRound;
    }
}