using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DataDomain.Rooms;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Enums;
using DataDomain.Rooms.Rounds.Helpers;
using Game.Domain.Events.Questions;

namespace Game.Ui.Views.GameControls
{
    public partial class GameViewModel
    {
        /// <summary>
        /// Выбрать игрока, который будет выбирать вопрос первым
        /// </summary>
        /// <param name="players"></param>
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
                        ActivePlayerBackup = ActivePlayer;

                        return;
                    }

                    int ceiling = (int) Math.Ceiling((double) _players.Count / 2) - 1;
                    PlayerModel? playerModel = _players[ceiling];

                    Message = $"Выбор темы и стоимости вопроса первым осуществляет игрок {playerModel?.Name}";
                    ActivePlayer = playerModel;
                    ActivePlayerBackup = ActivePlayer;

                    break;
                case RoundsLevelEnum.Round2:
                case RoundsLevelEnum.Round3:
                case RoundsLevelEnum.Final:
                    // раунд начинает игрок с наименьшим количеством очков
                    if (_currentRound is {Level: not RoundsLevelEnum.Round1})
                    {
                        ActivePlayer = GetPlayerWithMinPoint(players);
                        ActivePlayerBackup = ActivePlayer;
                    }

                    break;
                case RoundsLevelEnum.Shootout:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Task.Run(async () => await SendActivePlayerNameMove());
        }

        /// <summary>
        /// Игра готова принимать ответы.
        /// </summary>
        /// <param name="isReady"></param>
        private async Task GameIsReadyToReceiveAnswers(bool isReady)
        {
            IsReadyGameToReceiveAnswers = isReady;

            if (!isReady)
            {
                return;
            }

            Message = $"Ответы принимаются";

            foreach (PlayerModel player in Players)
            {
                await _gameSenderService.SendRedButton(player.Id);
            }
        }

        /// <inheritdoc cref="PlayerIsReadyAnswerQuestionEvent"/>
        private void OnPlayerIsReadyAnswerQuestion(long playerId)
        {
            if (!IsReadyGameToReceiveAnswers || ActivePlayer != null)
            {
                return;
            }

            PlayerModel? player = Players.FirstOrDefault(p => p.Id == playerId);

            if (player == null)
            {
                Message = $"Ошибка. Не найден игрок с ИД: {playerId}";

                return;
            }

            Message = $"Отвечает на вопрос игрок {_activePlayer}";
            ActivePlayer = player;
        }

        /// <summary>
        /// Получить игрока с минимальным количеством очков
        /// </summary>
        /// <param name="playerList"></param>
        /// <returns></returns>
        private PlayerModel GetPlayerWithMinPoint(List<PlayerModel?> playerList = null)
        {
            List<PlayerModel?> players = new();

            if (playerList != null && !playerList.Any())
            {
                if (_players == null || !playerList.Any())
                {
                    return null;
                }

                players = new List<PlayerModel?>(_players);
            }
            else
            {
                players = playerList;
            }

            int? minPoint = players?.Min(p => p.Points);

            return players?.FirstOrDefault(p => p.Points == minPoint);
        }

        /// <summary>
        /// Очистить все данные
        /// </summary>
        private void ClearAllParameters()
        {
            IsGameStarted = false;
            IsShowedTopics = false;
            Host = null;
            ActivePlayer = null;
            ActivePlayerBackup = null;
            CurrentRound = null;
            DisplayedQuestion = null;
            Message = null;

            Rounds = null;
            Players = null;
            Topics = null;

            _game = null;
        }

        private bool _isReadyGameToReceiveAnswers;
    }
}