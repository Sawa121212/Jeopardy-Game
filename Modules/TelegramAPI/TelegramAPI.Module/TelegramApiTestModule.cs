using System.Resources;
using Common.Core.Localization;
using Infrastructure.Interfaces.Managers;
using Prism.Ioc;
using Prism.Modularity;
using TelegramAPI.Infrastructure.Interfaces.Managers;
using TelegramAPI.Infrastructure.Managers;
using TelegramAPI.Module.Properties;
using TelegramAPI.Ui.Views;
using TelegramAPI.Ui.Views.Settings;

namespace TelegramAPI.Module
{
    /// <summary>
    /// Модуль Telegram API
    /// </summary>
    public class TelegramApiModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .RegisterSingleton<ITelegramBotService, TelegramBotService>();

            // views
            containerRegistry.RegisterForNavigation<TelegramTestView, TelegramTestViewModel>();
            containerRegistry.RegisterForNavigation<TelegramSettingsView, TelegramSettingsViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));

            // Применить загруженные настройки
            //containerProvider.Resolve<ITelegramSettingsService>()?.ApplySavedSettings();

            containerProvider.Resolve<ISettingsViewManager>().AddView<TelegramSettingsView>("Параметры бота", "Telegram бот");
        }
    }
}