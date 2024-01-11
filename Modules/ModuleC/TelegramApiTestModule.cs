using System.Resources;
using Common.Core.Localization;
using Infrastructure.Interfaces.Managers;
using Prism.Ioc;
using Prism.Modularity;
using TelegramAPI.Test.Managers;
using TelegramAPI.Test.Properties;
using TelegramAPI.Test.Services.Settings;
using TelegramAPI.Test.Views;
using TelegramAPI.Test.Views.Settings;

namespace TelegramAPI.Test
{
    /// <summary>
    /// Модуль C
    /// </summary>
    public class TelegramApiTestModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // регистрируем View для навигации по Регионам
            containerRegistry.RegisterSingleton<ITelegramSettingsRepositoryService, TelegramSettingsRepositoryService>();
            containerRegistry.RegisterSingleton<ITelegramSettingsService, TelegramSettingsService>();
            containerRegistry.RegisterSingleton<ITelegramBotManager, TelegramBotManager>();
            containerRegistry.RegisterForNavigation<TelegramTestView, TelegramTestViewModel>();
            containerRegistry.RegisterForNavigation<TelegramSettingsView, TelegramSettingsViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));

            // Применить загруженные настройки
            containerProvider.Resolve<ITelegramSettingsService>()?.ApplySavedSettings();

            // Зарегистрировать View к региону. Теперь при запуске ПО View будет показано
            //_regionManager.RegisterViewWithRegion("RegionC", typeof(TabCView));
            containerProvider.Resolve<ISettingsViewManager>().AddView<TelegramSettingsView>("Параметры бот", "Telegram бот");
        }
    }
}