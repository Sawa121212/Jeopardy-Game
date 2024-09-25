using System.Threading.Tasks;
using DataDomain.Rooms;
using GameSender.Infrastructure.Interfaces;
using Telegram.Bot.Types;
using TelegramAPI.Domain.Models;
using TopicDb.Domain.Models;

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
                await SendEmptyButtonsForPlayers($"Выбор темы и стоимости вопроса осуществляет игрок {ActivePlayer?.Name}");
            }
        }

        private async Task SendEmptyButtonsForPlayers(string text, bool sendForHost = false)
        {
            foreach (PlayerModel playerModel in Players)
            {
                await _gameSenderService.SendBaseGameButton(playerModel.Id, text);
            }

            if (sendForHost)
            {
                await _gameSenderService.SendBaseGameButton(Host.Id, text);
            }
        }

        /// <summary>
        /// Отправить сообщение через <see cref="IGameSenderService"/>
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        private async Task<MessageModel?> OnSendMessage(Question question)
        {
            // Picture
            if (question?.Picture is {ChatId: > 0})
            {
                foreach (PlayerModel? playerModel in Players)
                {
                    await _gameSenderService.ForwardMessageAsync(playerModel.Id, question.Picture.ChatId, question.Picture.MessageId);
                }

                // send to host
                Message? message = await _gameSenderService.ForwardMessageAsync(
                    _host.Id,
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

                return sentMessage == null ? null : new MessageModel(sentMessage.Text);
            }

            return null;
        }
    }
}