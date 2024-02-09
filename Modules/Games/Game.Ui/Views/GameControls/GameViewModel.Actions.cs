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
using ReactiveUI;
using TelegramAPI.Test.Models;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Interfaces.Services;

namespace Game.Ui.Views.GameControls
{
    public partial class GameViewModel
    {
        /// <summary>
        /// Игра готова принимать ответы
        /// </summary>
        public bool IsReadyGameToReceiveAnswers
        {
            get => _isReadyGameToReceiveAnswers;
            private set => this.RaiseAndSetIfChanged(ref _isReadyGameToReceiveAnswers, value);
        }

        public ICommand SelectQuestionAnswerCommand { get; }
        public ICommand AnsweredQuestionCommand { get; }
        public ICommand NoAnsweredQuestionCommand { get; }
        public ICommand CloseQuestionCommand { get; }

        /// <summary>
        /// Выбрать вопрос для отображения
        /// </summary>
        /// <param name="questionModel"></param>
        /// <returns></returns>
        private async Task OnSelectQuestionAnswer(QuestionModel? questionModel)
        {
            if (questionModel is null)
            {
                Message = "Ошибка. Не удалось получить вопрос";
                return;
            }

            if (questionModel.IsAsked)
            {
                Message = "Ошибка. Вопрос уже был задан";
                return;
            }

            Question? questionById = _questionService.GetQuestionById(questionModel.Id);
            if (questionById is null)
            {
                Message = "Ошибка. Не удалось найти вопрос в БД";
                return;
            }

            DisplayedQuestion = questionModel;

            // backup
            ActivePlayerBackup = _activePlayer;
            ActivePlayer = null;

            await OnSendQuestion(questionById);
        }

        /// <summary>
        /// Отправить сообщением выбранный вопрос игрокам и ведущему
        /// </summary>
        /// <param name="question">Выбранный вопрос</param>
        /// <returns></returns>
        private async Task OnSendQuestion(Question question)
        {
            if (!_players.Any() || _host is null)
            {
                return;
            }

            // ToDo: выполнить проверку на "специальные вопросы"
            MessageModel? sentMessage = await OnSendMessage(question);
            _eventAggregator.GetEvent<QuestionsIsSentEvent>().Publish(new QuestionsIsSentEvent(_roomKey));

            if (sentMessage != null)
            {
                DisplayedQuestion.Picture = sentMessage.Bitmap;
            }

            // Показать вопрос для ответа во вью
            OnShowQuestionForAnswer();
        }

        /// <summary>
        /// На вопрос был дан ответ
        /// </summary>
        /// <param name="isCorrectAnswer">Правильный ли ответ</param>
        /// <returns></returns>
        private async Task OnAnsweredQuestion(bool? isCorrectAnswer)
        {
            if (ActivePlayer == null || _displayedQuestion == null)
            {
                return;
            }

            switch (isCorrectAnswer)
            {
                case true:
                    ActivePlayer.AddPoint(_displayedQuestion.Price);
                    ActivePlayerBackup = _activePlayer;

                    // Показать сразу ответ
                    OnShowCorrectAnswer();
                    break;
                case false:
                    // Неправильный ответ. Ждем еще ответ.
                    ActivePlayer.AddPoint(_displayedQuestion.Price * -1);

                    // очищаем
                    ActivePlayer = null;
                    break;
            }
        }

        /// <summary>
        /// На вопрос не был дан ответ
        /// </summary>
        private void OnNoAnsweredQuestionCommand()
        {
            Message = "На вопрос не был дан ответ";
            OnShowCorrectAnswer();

            // восстановить активного игрока
            ActivePlayer = ActivePlayerBackup;
        }

        /// <summary>
        /// Закрыть вопрос
        /// </summary>
        /// <returns></returns>
        private async Task OnCloseQuestion()
        {
            if (DisplayedQuestion == null)
            {
                return;
            }

            DisplayedQuestion.IsAsked = true;
            DisplayedQuestion = null;

            GameIsReadyToReceiveAnswers(false);
            Message = $"Вопрос выбирает игрок {_activePlayer?.Name}";

            CheckRoundIsOver();
            OnShowCurrentRound();
        }

        /// <inheritdoc cref="GameIsReadyToReceiveAnswersEvent"/>
        private void GameIsReadyToReceiveAnswers(bool isReady)
        {
            IsReadyGameToReceiveAnswers = isReady;
            if (isReady)
            {
                Message = $"Ответы принимаются";
                _eventAggregator.GetEvent<GameIsReadyToReceiveAnswersEvent>().Publish(new GameIsReadyToReceiveAnswersEvent(_roomKey));
            }
        }

        /// <inheritdoc cref="PlayerIsReadyAnswerQuestionEvent"/>
        private void OnPlayerIsReadyAnswerQuestion(PlayerIsReadyAnswerQuestionEvent? playerIsReadyAnswer)
        {
            if (playerIsReadyAnswer == null || !IsReadyGameToReceiveAnswers || ActivePlayer != null)
            {
                return;
            }

            if (playerIsReadyAnswer.RoomKey.IsNullOrEmpty() || playerIsReadyAnswer.RoomKey != _roomKey)
            {
                return;
            }

            PlayerModel? player = Players.FirstOrDefault(p => p.Id == playerIsReadyAnswer.PlayerId);
            if (player == null)
            {
                Message = $"Ошибка. Не найден игрок с ИД: {playerIsReadyAnswer.PlayerId}";
                return;
            }

            Message = $"Отвечает на вопрос игрок {_activePlayer}";
            ActivePlayer = player;
        }

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

            if (_game.CurrentRoundLevel is RoundsLevelEnum.Final)
            {
                int maxPoint = _players.Max(p => p.Points);
                List<PlayerModel?> playerModels = _players.Where(p => p.Points == maxPoint).ToList();
                if (playerModels.Count == 1)
                {
                    OnShowGameWinner();
                }
            }

            _game.CurrentRoundLevel = RoundHelper.GetNextRoundLevel(_game.CurrentRoundLevel);

            ActivePlayer = GetPlayerFirstChoosingTopic();
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

            CurrentRound = Rounds.FirstOrDefault(r => r != null && r.Level == _game.CurrentRoundLevel);

            IList<TopicModel>? topicModels = CurrentRound?.Topics;
            if (topicModels != null)
            {
                Topics = new ObservableCollection<TopicModel>(topicModels);
            }
        }

        #endregion

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
            _roomKey = null;
        }

        private readonly IQuestionService _questionService;
        private bool _isReadyGameToReceiveAnswers;
    }
}