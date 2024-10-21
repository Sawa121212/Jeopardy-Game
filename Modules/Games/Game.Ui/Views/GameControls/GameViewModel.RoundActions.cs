using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DataDomain.Rooms;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Enums;
using DataDomain.Rooms.Rounds.Helpers;
using Game.Domain.Events.Questions;

namespace Game.Ui.Views.GameControls
{
    public partial class GameViewModel
    {
        #region [RoundActions]

        /// <summary>
        /// Проверить не закончился ли раунд
        /// </summary>
        private void CheckRoundIsOver()
        {
            // найдем хоть одну тему, где есть не отвеченный вопрос
            if (CurrentRound?.Topics != null &&
                CurrentRound?.Topics.FirstOrDefault(t => t.Questions.Exists(q => q.IsAsked == false)) == null)
            {
                // если все вопросы заданы, переходим в следующий раунд
                OnGoNextRound();
            }
        }

        /// <summary>
        /// Перейти к следующему раунду
        /// </summary>
        private void OnGoNextRound()
        {
            if (_game is null)
            {
                return;
            }

            // если этот раунд был Финальным
            if (_game.CurrentRoundLevel is RoundsLevelEnum.Final)
            {
                int maxPoint = _players.Max(p => p.Points);
                List<PlayerModel> playerModels = _players.Where(p => p.Points == maxPoint).ToList();

                if (playerModels.Count == 1)
                {
                    // Показать победителя игры
                    OnShowGameWinnerView();
                    return;
                }
            }

            // Установить следующий раунд
            _game.CurrentRoundLevel = RoundHelper.GetNextRoundLevel(_game.CurrentRoundLevel);

            SetPlayerFirstChoosingTopic(Players);
            OnChangeRound();
        }

        /// <summary>
        /// Поменять раунд
        /// </summary>
        private void OnChangeRound()
        {
            if (_game is null)
            {
                return;
            }

            // выставим флаг
            IsShowedTopics = false;

            CurrentRound = Rounds.FirstOrDefault(r => r.Level == _game.CurrentRoundLevel);

            IList<TopicModel>? topicModels = CurrentRound?.Topics;

            if (topicModels != null)
            {
                Topics = new ObservableCollection<TopicModel>(topicModels);
            }

            if (IsGameStarted)
            {
                // Отобразить название текущего раунда
                OnShowRoundLevelNameView();
            }
        }

        #endregion
    }
}