using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DataDomain.Rooms;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Enums;
using Game.Domain.Events.Questions;
using TelegramAPI.Domain.Models;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Interfaces.Services;
using Users.Domain.Models;

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
            IsVisibleGongButton = true;

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
            MessageModel? sendMessage = await OnSendQuestionMessage(question);

            if (sendMessage != null)
            {
                DisplayedQuestion.Picture = sendMessage.Bitmap;
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
            if (ActivePlayer == null || DisplayedQuestion == null)
            {
                return;
            }

            switch (isCorrectAnswer)
            {
                case true:
                    // Завершить прием ответов
                    await GameIsReadyToReceiveAnswersAsync(false);
                    await SendEmptyButtonsForPlayers("На вопрос был дан правильный ответ");

                    // Сообщить об активном игроке
                    await SendActivePlayerNameMove();

                    ActivePlayer.AddPoint(DisplayedQuestion.Price);
                    ActivePlayerBackup = ActivePlayer;

                    // Показать сразу ответ
                    OnShowCorrectAnswerView();

                    break;
                case false:
                    await SendMessage("На вопрос был дан неправильный ответ");
                    await GameIsReadyToReceiveAnswersAsync(true);

                    // Неправильный ответ. Ждем еще ответ.
                    ActivePlayer.AddPoint(DisplayedQuestion.Price * -1);

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
            await GameIsReadyToReceiveAnswersAsync(false);
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

            foreach (PlayerModel playerModel in Players)
            {
                _userService.SetStatus(playerModel.Id, StateUserEnum.Playing);
            }

            // Сообщить об активном игроке
            await SendActivePlayerNameMove();

            CheckRoundIsOver();
            OnShowCurrentRoundView();
        }

        private readonly IQuestionService _questionService;
    }
}