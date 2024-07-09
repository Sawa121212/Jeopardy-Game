using Common.Core.Views;
using TelegramAPI.Infrastructure.Interfaces.Managers;
using ReactiveUI;

namespace JeopardyGame.Views.Shell
{
    public class ShellViewModel : ViewModelBase
    {
        public ShellViewModel(ITelegramBotManager telegramBotManager)
        {
            _telegramBotManager = telegramBotManager;
        }

        public string Title => "Своя игра";

        public ITelegramBotManager TelegramBotManager
        {
            get => _telegramBotManager;
            set => this.RaiseAndSetIfChanged(ref _telegramBotManager, value);
        }

        private ITelegramBotManager _telegramBotManager;
    }
}