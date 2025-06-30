using System.Threading.Tasks;
using Common.Extensions;
using DataDomain.Rooms;
using GameSender.Infrastructure.Interfaces;
using Telegram.Bot.Types;
using TelegramAPI.Domain.Models;
using TopicDb.Domain.Models;
using Users.Domain.Models;

namespace Game.Ui.Views.GameControls
{
    public partial class GameViewModel
    {
        /// <summary>
        /// Сообщить имя активного игрока
        /// </summary>
        /// <returns></returns>
        private async Task SendActivePlayerNameMove()
        {
            if (ActivePlayer is not null)
            {
                Message = $"Выбор темы и стоимости вопроса осуществляет игрок {ActivePlayer?.Name}";
                await SendEmptyButtonsForPlayers(Message);
            }
        }

        private async Task SendEmptyButtonsForPlayers(string text, bool sendForHost = false)
        {
            foreach (PlayerModel playerModel in Players)
            {
                _userService.SetStatus(playerModel.Id, StateUserEnum.InRoom);

                await _gameSenderService.SendEmptyGameButton(playerModel.Id, text);
            }

            if (sendForHost)
            {
                await _gameSenderService.SendEmptyGameButton(Host.Id, text);
            }
        }

        /// <summary>
        /// Отправить сообщение через <see cref="IGameSenderService"/>
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        private async Task<MessageModel?> OnSendQuestionMessage(Question question)
        {
            // Picture
            if (question?.Picture is {ChatId: > 0})
            {
                foreach (PlayerModel playerModel in Players)
                {
                    await _gameSenderService.ForwardMessageAsync(playerModel.Id, question.Picture.ChatId, question.Picture.MessageId);
                }

                // send to host
                Message? message = await _gameSenderService.ForwardMessageAsync(
                    Host.Id,
                    question.Picture.ChatId,
                    question.Picture.MessageId);

                return await _gameSenderService.ParseMessageAsync(message) as MessageModel;
            }
            else
            {
                // base message
                foreach (PlayerModel? playerModel in Players)
                {
                    await _gameSenderService.SendMessageAsync(playerModel.Id, question.Text);
                }

                // send to host
                Message sentMessage = await _gameSenderService.SendMessageAsync(_host.Id, question.Text);
                
                await _gameSenderService.SendMessageAsync(_host.Id, $"Правильный ответ: {question.CorrectAnswer}");

                return sentMessage == null ? null : new MessageModel(sentMessage.Text);
            }

            return null;
        }

        private async Task<MessageModel?> SendMessage(string text)
        {
            Message sentMessage = null;

            if (!text.IsNullOrEmpty())
            {
                // base message
                foreach (PlayerModel? playerModel in Players)
                {
                    await _gameSenderService.SendMessageAsync(playerModel.Id, text);
                }

                // send to host
                sentMessage = await _gameSenderService.SendMessageAsync(_host.Id, text);
            }

            return sentMessage == null ? null : new MessageModel(sentMessage.Text);

            ;
        }
    }
}