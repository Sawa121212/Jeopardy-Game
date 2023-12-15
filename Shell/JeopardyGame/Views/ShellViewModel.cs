using Common.Core.Localization;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace JeopardyGame.Views
{
    public partial class ShellViewModel : BindableBase
    {
        private readonly ILocalizer _localizer;

        public ShellViewModel(IRegionManager regionManager, ILocalizer localizer)
        {
            _localizer = localizer;
            _regionManager = regionManager;

            ShowTopicsCommand = new DelegateCommand(OnShowTopics);
            ShowPlayInformationCommand = new DelegateCommand(OnShowPlayInformation);
            OnShowTelegramTestCommand = new DelegateCommand(OnShowTelegramTest);
        }
    }
}