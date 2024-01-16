using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using TelegramAPI.Test.Managers;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Interfaces.Services;

namespace TelegramAPI.Test.Views
{
    public class TelegramTestViewModel : BindableBase
    {
        public TelegramTestViewModel(ITelegramBotService telegramBotService, IQuestionService questionService)
        {
            _telegramBotService = telegramBotService;
            _questionService = questionService;
            SendMessCommand = new DelegateCommand(async () => await OnSendMessage());
        }

        private async Task OnSendMessage()
        {
            Question? question = _questionService.GetAllQuestions().FirstOrDefault(q => q.Picture != null);
            if (question?.Picture is {ChatId: > 0})
            {
                await _telegramBotService.ForwardMessageAsync(825819569, question.Picture.ChatId, question.Picture.MessageId);
            }
        }

        public ICommand SendMessCommand { get; }


        private readonly ITelegramBotService _telegramBotService;
        private readonly IQuestionService _questionService;
    }
}