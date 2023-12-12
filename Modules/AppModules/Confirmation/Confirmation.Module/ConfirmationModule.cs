using Confirmation.Module.Services;
using Confirmation.Module.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Confirmation.Module
{
    public class ConfirmationModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .RegisterSingleton<IConfirmationService, ConfirmationService>()
                .Register<ConfirmationView>()
                .RegisterForNavigation<ConfirmationTestView, ConfirmationTestViewModel>()
                ;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.Resolve<IRegionManager>()
                .RegisterViewWithRegion("ConfirmationTestControlRegion", typeof(ConfirmationTestView));
        }
    }
}