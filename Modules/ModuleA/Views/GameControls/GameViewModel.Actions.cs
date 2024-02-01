using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Helpers;
using TelegramAPI.Test.Models;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Interfaces.Services;

namespace Game.Views.GameControls
{
    public partial class GameViewModel
    {
        public ICommand SelectQuestionAnswerCommand { get; }
        public ICommand AnsweredQuestionCommand { get; }
        public ICommand NoAnsweredQuestionCommand { get; }
        public ICommand CloseQuestionCommand { get; }

        /// <summary>
        /// Выбрать вопрос для отображения
        /// </summary>
        /// <param name="questionModel"></param>
        /// <returns></returns>
        private async Task OnSelectQuestionAnswer(QuestionModel questionModel)
        {
            if (questionModel is null)
            {
                Message = "Ошибка. Не удалось получить вопрос";
                return;
            }

            Question? questionById = _questionService.GetQuestionById(questionModel.Id);
            if (questionById is null)
            {
                Message = "Ошибка. Не удалось найти вопрос в БД";
                return;
            }

            DisplayedQuestion = questionModel;
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
            if (_activePlayer == null)
            {
                return;
            }

            switch (isCorrectAnswer)
            {
                case true:
                    _activePlayer.AddPoint(_displayedQuestion.Price);
                    OnShowCorrectAnswer();
                    break;
                case false:
                    _activePlayer.AddPoint(_displayedQuestion.Price * -1);
                    break;
            }
        }

        /// <summary>
        /// На вопрос не был дан ответ
        /// </summary>
        private void OnNoAnsweredQuestionCommand()
        {
            OnShowCorrectAnswer();
        }

        /// <summary>
        /// Закрыть вопрос
        /// </summary>
        /// <returns></returns>
        private async Task OnCloseQuestion()
        {
            _displayedQuestion.IsAsked = true;
            _displayedQuestion = null;

            OnShowCurrentRound();
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

            _game.CurrentRoundLevel = RoundHelper.GetNextRoundLevel(_game.CurrentRoundLevel);
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

        private readonly IQuestionService _questionService;
    }
}