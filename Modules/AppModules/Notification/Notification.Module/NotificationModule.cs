using System.Resources;
using Common.Core.Localization;
using Notification.Module.Properties;
using Notification.Module.Services;
using Notification.Module.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Notification.Module
{
    public class NotificationModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<INotificationService, NotificationService>();
            containerRegistry.Register<NoticeDialogView>();

            containerRegistry.RegisterForNavigation<NotificationTestView>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));
            containerProvider.Resolve<IRegionManager>()
                .RegisterViewWithRegion("NotificationTestControlRegion", typeof(NotificationTestView));
        }
    }
}