using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Extensions.Collections;
using DataDomain.Rooms;
using DataDomain.Rooms.Rounds;
using Game.Ui.Models;
using Infrastructure.Domain.Helpers;
using ReactiveUI;

namespace Game.Ui.Views.GameControls
{
    public partial class GameViewModel
    {
        public ICommand RemoveTopicFromFinalRoundCommand { get; }
        public ICommand EndPlaceBetsCommand { get; }

        /// <summary>
        /// Темы вопросов
        /// </summary>
        public ObservableCollection<TopicModel>? TopicsFromFinalRound
        {
            get => _topicsFromFinalRound;
            private set => this.RaiseAndSetIfChanged(ref _topicsFromFinalRound, value);
        }

        public ObservableCollection<PlayerBetModel>? PlayerBetModels
        {
            get => _playerBetModels;
            private set => this.RaiseAndSetIfChanged(ref _playerBetModels, value);
        }

        public bool IsRemoveMode
        {
            get => _isRemoveMode;
            private set => this.RaiseAndSetIfChanged(ref _isRemoveMode, value);
        }

        private List<PlayerModel?>? SortedPlayers { get; set; }
        private QuestionModel? FinalQuestion { get; set; }

        /// <summary>
        /// Подготовь финальный раунд
        /// </summary>
        private async Task PrepareFinalRound()
        {
            IsRemoveMode = true;

            // отображаемые темы
            TopicsFromFinalRound = new ObservableCollection<TopicModel>(_currentRound?.Topics);
            if (_players == null)
            {
                return;
            }

            // Если у кого-либо из игроков перед финальным раундом сумма на игровом счёте отрицательная или равна нулю,
            // он покидает игру и терпит досрочное поражение
            List<PlayerModel?> getOutPlayers = _players.OrderByDescending(o => o.Points)
                .Where(p => p.Points <= 0).ToList();

            if (getOutPlayers.Any())
            {
                foreach (PlayerModel playerModel in getOutPlayers)
                {
                    await _telegramBotService.SendMessageAsync(playerModel.Id,
                        $"Финальный раунд: вы ПОКИДАТЕ игру и терпит досрочное поражение.");
                }
            }

            // ToDo: покидать игру 
            SortedPlayers = _players.OrderByDescending(o => o.Points).ToList();

            SetPlayerFirstChoosingTopic(SortedPlayers);
        }

        /// <summary>
        /// Убрать тему из финального раунда.
        /// </summary>
        /// <param name="topicModel">Тема.</param>
        /// <returns></returns>
        private async Task OnRemoveTopicFromFinalRound(TopicModel? topicModel)
        {
            if (topicModel == null || TopicsFromFinalRound == null || SortedPlayers == null)
            {
                return;
            }

            if (TopicsFromFinalRound.Contains(topicModel))
            {
                // удалить тему из финального раунда
                TopicsFromFinalRound.Remove(topicModel);
            }

            // если в списке осталась только 1 тема, то переходим к выставлению ставок
            if (TopicsFromFinalRound.Count == 1)
            {
                TopicModel topic = TopicsFromFinalRound.First();
                int questionIndex = RandomGenerator.GetRandom().Next(0, topic.Questions.Count);

                FinalQuestion = topic.Questions[questionIndex];
                ActivePlayer = null;
                Message = null;

                // Начать выставление ставок
                OnPlayersPlaceBets();
                return;
            }

            if (_activePlayer == null)
            {
                Message = $"Error. Метод {nameof(OnRemoveTopicFromFinalRound)}: не найден {ActivePlayer}";
                return;
            }

            // даем право убрать тему следующему игроку
            int? nextActivePlayerIndex = SortedPlayers?.GetIndex(_activePlayer);
            nextActivePlayerIndex++;
            if (nextActivePlayerIndex is null || nextActivePlayerIndex == SortedPlayers.Count)
            {
                nextActivePlayerIndex = 0;
            }

            ActivePlayer = SortedPlayers?[(int) nextActivePlayerIndex];

            if (ActivePlayer != null)
            {
                Message = $"Убирает вопрос игрок {ActivePlayer.Name}";
            }
        }

        /// <summary>
        /// Начать выставление ставок
        /// </summary>
        private void OnPlayersPlaceBets()
        {
            IsRemoveMode = false;
            PlayerBetModels = new ObservableCollection<PlayerBetModel>();
            if (_players == null)
            {
                return;
            }

            foreach (PlayerModel? playerModel in _players)
            {
                if (playerModel != null)
                {
                    PlayerBetModels.Add(
                        new PlayerBetModel(playerModel)
                        {
                            Bet = RandomGenerator.GetRandom().Next(50, 1000), // Test. Remove
                            IsMadeBet = true // Test. Remove
                        }
                    );
                }
            }
        }

        private void PlayerPlaceBet(int playerId)
        {
            if (SortedPlayers == null)
            {
                return;
            }

            PlayerModel? firstOrDefault = SortedPlayers.FirstOrDefault(p => p.Id == playerId);

            if (firstOrDefault == null)
            {
                return;
            }

            // OnEndPlaceBets()
        }

        /// <summary>
        /// Завершить прием ставок
        /// </summary>
        private async Task OnEndPlaceBets()
        {
            await OnSelectAndShowQuestionAnswer(FinalQuestion);
        }

        private ObservableCollection<TopicModel>? _topicsFromFinalRound;
        private ObservableCollection<PlayerBetModel>? _playerBetModels;
        private bool _isRemoveMode;
    }
}