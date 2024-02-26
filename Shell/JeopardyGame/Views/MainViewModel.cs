using Common.Core.Localization;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace JeopardyGame.Views
{
    public partial class MainViewModel : BindableBase
    {
        public MainViewModel(IRegionManager regionManager, ILocalizer localizer)
        {
            _localizer = localizer;
            _regionManager = regionManager;

            ShowTopicsCommand = new DelegateCommand(OnShowTopics);
            ShowPlayInformationCommand = new DelegateCommand(OnShowPlayInformation);
            OnShowTelegramTestCommand = new DelegateCommand(OnShowTelegramTest);
            ShowSettingsCommand = new DelegateCommand(OnShowSettings);
            PlayCommand = new DelegateCommand(OnPlay);


        }

        private readonly ILocalizer _localizer;
    }
}