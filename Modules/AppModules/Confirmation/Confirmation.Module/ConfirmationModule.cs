using Confirmation.Module.Services;
using Prism.Ioc;
using Prism.Modularity;

namespace Confirmation.Module
{
    public class ConfirmationModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .RegisterSingleton<IConfirmationService, ConfirmationService>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}