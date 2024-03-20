using Infrastructure.Interfaces.Managers;
using Infrastructure.Interfaces.Services.Settings;
using Infrastructure.Ui.Views;
using Infrastructure.Ui.Views.Settings;
using Prism.Ioc;
using Prism.Modularity;

namespace Infrastructure.Module
{
    public class InfrastructureModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<BaseSettingsView, BaseSettingsViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<TelegramAdminSettingsView, TelegramAdminSettingsViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            //containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));

            // Применить сохраненные настройки
            containerProvider.Resolve<IApplicationSettingsService>()?.ApplySavedSettings();

            ISettingsViewManager settingsViewManager = containerProvider.Resolve<ISettingsViewManager>();

            if (settingsViewManager == null)
            {
                return;
            }

            // settings view
            settingsViewManager.AddView<BaseSettingsView>("Общие", "Общие настройки");
            settingsViewManager.AddView<TelegramAdminSettingsView>("База данных", "Аккаунт администратора");
        }
    }
}