using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Enums;
using Game.Domain.Events.Questions;
using TelegramAPI.Domain.Models;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Interfaces.Services;

namespace Game.Ui.Views.GameControls
{
    public partial class GameViewModel
    {
        public ICommand SelectQuestionAnswerCommand { get; }
        public ICommand AnsweredQuestionCommand { get; }
        public ICommand NoAnsweredQuestionCommand { get; }
        public ICommand CloseQuestionCommand { get; }

        /// <summary>
        /// Выбрать вопрос и отобразить на экране
        /// </summary>
        /// <param name="questionModel"></param>
        /// <returns></returns>
        private async Task OnSelectAndShowQuestionAnswer(QuestionModel? questionModel)
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
            if (_players != null && (!_players.Any() || _host is null))
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
            OnShowQuestionForAnswerView();
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
                    // Завершить прием ответов
                    await GameIsReadyToReceiveAnswers(false);
                    await SendEmptyButtonsForPlayers("Жаль... На вопрос не был дан ответ");

                    // Сообщить об активном игроке
                    await SendActivePlayerNameMove();

                    ActivePlayer.AddPoint(_displayedQuestion.Price);
                    ActivePlayerBackup = ActivePlayer;

                    // Показать сразу ответ
                    OnShowCorrectAnswerView();

                    break;
                case false:
                    await GameIsReadyToReceiveAnswers(true);

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
        private async Task OnNoAnsweredQuestion()
        {
            await GameIsReadyToReceiveAnswers(false);
            await SendEmptyButtonsForPlayers("Жаль... На вопрос не был дан ответ");

            Message = "На вопрос не был дан ответ";
            OnShowCorrectAnswerView();

            // восстановить активного игрока
            ActivePlayer = ActivePlayerBackup;
        }

        /// <summary>
        /// Закрыть вопрос
        /// </summary>
        /// <returns></returns>
        private async Task OnCloseQuestion()
        {
            if (_currentRound?.Level == RoundsLevelEnum.Final)
            {
                // если Финальный раунд, покажем ставки и ответы
                OnShowPlayersBetView();

                return;
            }

            if (DisplayedQuestion == null)
            {
                return;
            }

            DisplayedQuestion.IsAsked = true;
            DisplayedQuestion = null;

            GameIsReadyToReceiveAnswers(false);
            Message = $"Вопрос выбирает игрок {_activePlayer?.Name}";

            CheckRoundIsOver();
            OnShowCurrentRoundView();
        }

        private readonly IQuestionService _questionService;
    }
}