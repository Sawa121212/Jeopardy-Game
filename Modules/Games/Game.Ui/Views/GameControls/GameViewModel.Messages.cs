using System.Threading.Tasks;
using DataDomain.Rooms;
using Telegram.Bot.Types;
using TelegramAPI.Domain.Models;
using TelegramAPI.Infrastructure.Interfaces.Managers;
using TopicDb.Domain.Models;

namespace Game.Ui.Views.GameControls
{
    public partial class GameViewModel
    {
        /// <summary>
        /// Отправить сообщение через <see cref="ITelegramBotService"/>
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        private async Task<MessageModel?> OnSendMessage(Question question)
        {
            // Picture
            if (question?.Picture is {ChatId: > 0})
            {
                foreach (PlayerModel? playerModel in _players)
                {
                    await _telegramBotService.ForwardMessageAsync(playerModel.Id, question.Picture.ChatId, question.Picture.MessageId);
                }

                Message? message = await _telegramBotService.ForwardMessageAsync(
                    _host.Id,
                    question.Picture.ChatId,
                    question.Picture.MessageId);

                return await _telegramBotService.ParseMessageAsync(message);
            }
            else
            {
                // base message
                foreach (PlayerModel? playerModel in _players)
                {
                    await _telegramBotService.SendMessageAsync(playerModel.Id, question.Text);
                }

                Message sentMessage = await _telegramBotService.SendMessageAsync(_host.Id, question.Text);
                if (sentMessage == null)
                {
                    return null;
                }

                return new MessageModel(sentMessage.Text);
            }

            return null;
        }

        private readonly ITelegramBotService _telegramBotService;
    }
}