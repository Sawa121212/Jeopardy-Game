using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Prism.Commands;
using ReactiveUI;
using Telegram.Bot.Types;
using TelegramAPI.Test.Managers;
using TelegramAPI.Test.Models;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Interfaces.Services;

namespace TelegramAPI.Test.Views
{
    public class TelegramTestViewModel : ReactiveObject
    {
        public TelegramTestViewModel(ITelegramBotService telegramBotService, IQuestionService questionService)
        {
            _telegramBotService = telegramBotService;
            _questionService = questionService;
            SendMessCommand = new DelegateCommand(async () => await OnSendMessage());
        }

        public Bitmap? Bitmap
        {
            get => _bitmap;
            set => this.RaiseAndSetIfChanged(ref _bitmap, value);
        }

        public string? Text
        {
            get => _text;
            set => this.RaiseAndSetIfChanged(ref _text, value);
        }

        public ICommand SendMessCommand { get; }

        private async Task OnSendMessage()
        {
            Question? question = _questionService.GetAllQuestions().FirstOrDefault(q => q.Picture != null);
            if (question?.Picture is {ChatId: > 0})
            {
                // 825819569 - pink
                // 938969260 - sawa
                Message? message
                    = await _telegramBotService.ForwardMessageAsync(938969260, question.Picture.ChatId, question.Picture.MessageId);

                MessageModel? messageModel = await _telegramBotService.ParseMessageAsync(message);
                if (messageModel != null)
                {
                    Text = messageModel.Text;
                    Bitmap = messageModel.Bitmap;
                }
            }
        }


        private readonly ITelegramBotService _telegramBotService;
        private readonly IQuestionService _questionService;
        private Bitmap? _bitmap;
        private string? _text;
    }
}