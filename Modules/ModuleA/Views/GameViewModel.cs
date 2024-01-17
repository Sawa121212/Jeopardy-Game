using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Common.Core.Prism;
using Common.Core.Views;
using DataDomain.Rooms;
using DataDomain.Rooms.Rounds;
using Game.Mangers;
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
            //_gameManager.
            Rounds = new List<RoundModel?>();
            Topics = new ObservableCollection<TopicModel>();
        }

        public GameModel? Game
        {
            get => _game;
            set => this.RaiseAndSetIfChanged(ref _game, value);
        }

        public List<RoundModel?> Rounds
        {
            get => _rounds;
            set => this.RaiseAndSetIfChanged(ref _rounds, value);
        }

        public ObservableCollection<TopicModel> Topics
        {
            get => _topics;
            set => this.RaiseAndSetIfChanged(ref _topics, value);
        }

        public ObservableCollection<PlayerModel> Players
        {
            get => _players;
            set => this.RaiseAndSetIfChanged(ref _players, value);
        }

        /// <inheritdoc />
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            object parameter = navigationContext.Parameters[NavigationParameterService.InitializeParameter];
            string? value = parameter?.ToString();
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            _roomKey = value;
            Game = _gameManager.GetGame(_roomKey);

            if (Game?.Rounds is null || Game.Rounds.Count == 0)
            {
                return;
            }

            Rounds.AddRange(Game.Rounds);

            List<TopicModel>? topicModels = Rounds.FirstOrDefault(r => r.Level == Game.CurrentRound)?.Topics;
            if (topicModels != null)
            {
                Topics = new ObservableCollection<TopicModel>(topicModels);
            }

            Players = new ObservableCollection<PlayerModel>(_gameManager.GetPlayersFromRoom(_roomKey));
        }

        private readonly IGameManager _gameManager;
        private string? _roomKey;
        private List<RoundModel?> _rounds;
        private ObservableCollection<PlayerModel> _players;
        private ObservableCollection<TopicModel> _topics;
        private GameModel? _game;
    }
}