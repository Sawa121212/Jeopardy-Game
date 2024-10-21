using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Extensions;
using DataDomain.Rooms;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Enums;
using DataDomain.Rooms.Rounds.Helpers;
using Game.Domain.Events.Questions;
using Users.Domain.Models;

namespace Game.Ui.Views.GameControls
{
    public partial class GameViewModel
    {
        public ICommand GongButtonClickedCommand { get; }

        private async Task GongButtonClickedAsync()
        {
            await GameIsReadyToReceiveAnswersAsync(true);

            foreach (PlayerModel playerModel in Players)
            {
                _userService.SetStatus(playerModel.Id, StateUserEnum.IsReadyReceiveAnswer);
            }

            IsVisibleGongButton = false;
        }

        /// <summary>
        /// Игра готова принимать ответы.
        /// </summary>
        /// <param name="isReady"></param>
        private async Task GameIsReadyToReceiveAnswersAsync(bool isReady)
        {
            IsReadyGameToReceiveAnswers = isReady;

            if (!isReady)
            {
                return;
            }

            Message = $"\ud83d Ответы принимаются !!!";

            foreach (PlayerModel player in Players)
            {
                await _gameSenderService.SendRedButton(player.Id);
            }
        }

        /// <inheritdoc cref="PlayerIsReadyAnswerQuestionEvent"/>
        private void OnPlayerIsReadyAnswerQuestion(long playerId)
        {
            if (!IsReadyGameToReceiveAnswers)
            {
                return;
            }

            PlayerModel? player = Players.FirstOrDefault(p => p.Id == playerId);

            if (player == null)
            {
                Message = EmojiExtension.EmojiMessageFormat(MessageEmoji.Error, $"Ошибка. Не найден игрок с ИД: {playerId}");
                return;
            }

            ActivePlayer = player;
            Message = $"Отвечает на вопрос игрок {ActivePlayer.Name}";
        }

        /// <summary>
        /// Выбрать игрока, который будет выбирать вопрос первым
        /// </summary>
        /// <param name="playerList"></param>
        /// <returns></returns>
        private void SetPlayerFirstChoosingTopic(IEnumerable<PlayerModel> playerList)
        {
            if (!playerList.Any())
            {
                return;
            }

            List<PlayerModel> players = new(playerList);

            if (_currentRound == null)
            {
                return;
            }

            switch (_currentRound.Level)
            {
                case RoundsLevelEnum.Round1:
                    // выбор темы и стоимости вопроса первым осуществляет игрок за центральным столом
                    if (players.Count is 1 or 2)
                    {
                        ActivePlayer = players[0];
                        ActivePlayerBackup = ActivePlayer;

                        return;
                    }

                    int ceiling = (int) Math.Ceiling((double) players.Count / 2) - 1;

                    PlayerModel? playerModel = players[ceiling];
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
                        ActivePlayer = GetPlayerWithMinPoint(Players);
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
        /// Получить игрока с минимальным количеством очков
        /// </summary>
        /// <param name="playerList"></param>
        /// <returns></returns>
        private PlayerModel GetPlayerWithMinPoint(IEnumerable<PlayerModel> playerList)
        {
            IEnumerable<PlayerModel> playerModels = playerList.ToList();
            int minPoint = playerModels.Min(p => p.Points);

            return playerModels.First(p => p.Points == minPoint);
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