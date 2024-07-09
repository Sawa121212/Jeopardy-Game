using System.Windows.Input;
using Common.Core.Prism.Regions;
using Game.Ui.Views;
using Infrastructure.Ui.Views;
using Prism.Regions;
using TelegramAPI.Ui.Views;
using TopicDb.Ui.Views;

namespace JeopardyGame.Views
{
    public partial class MainViewModel
    {
        public ICommand ShowTopicsCommand { get; }
        public ICommand ShowPlayInformationCommand { get; }
        public ICommand OnShowTelegramTestCommand { get; }
        public ICommand ShowSettingsCommand { get; }
        public ICommand PlayCommand { get; }

        private void OnShowTopics()
        {
            _regionManager.RequestNavigate(RegionNameService.ContentRegionName, nameof(MainTopicDbView));
        }

        private void OnShowPlayInformation()
        {
            _regionManager.RequestNavigate(RegionNameService.ContentRegionName, nameof(PlayInfoPages.PlayInfoView));
        }

        private void OnShowTelegramTest()
        {
            _regionManager.RequestNavigate(RegionNameService.ContentRegionName, nameof(TelegramTestView));
        }

        private void OnPlay()
        {
            _regionManager.RequestNavigate(RegionNameService.ShellRegionName, nameof(RoomView));
        }

        private void OnShowSettings()
        {
            _regionManager.RequestNavigate(RegionNameService.ShellRegionName, nameof(SettingsView));
        }

        private readonly IRegionManager _regionManager;
    }
}