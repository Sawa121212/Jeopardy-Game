using System.Windows.Input;
using Common.Core.Prism.Regions;
using Prism.Regions;
using TelegramAPI.Test.Views;
using TopicsDB.Infrastructure.Views;
using TopicsDB.Infrastructure.Views.Topics;

namespace JeopardyGame.Views
{
    public partial class ShellViewModel
    {
        public ICommand ShowTopicsCommand { get; }
        public ICommand ShowPlayInformationCommand { get; }
        public ICommand OnShowTelegramTestCommand { get; }

        private void OnShowTopics()
        {
            _regionManager.RequestNavigate(RegionNameService.ContentRegionName, nameof(TopicListView));
        }

        private void OnShowPlayInformation()
        {
            _regionManager.RequestNavigate(RegionNameService.ContentRegionName, nameof(PlayInfoView));
        }

        private void OnShowTelegramTest()
        {
            _regionManager.RequestNavigate(RegionNameService.ContentRegionName, nameof(TelegramTestView));
        }

        private readonly IRegionManager _regionManager;
    }
}