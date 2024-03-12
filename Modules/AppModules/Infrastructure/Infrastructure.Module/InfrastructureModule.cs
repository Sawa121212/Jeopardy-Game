using Infrastructure.Interfaces.Managers;
using Infrastructure.Interfaces.Services;
using Infrastructure.Interfaces.Services.Settings;
using Infrastructure.Module.Managers;
using Infrastructure.Module.Services;
using Infrastructure.Module.Services.ApplicationInfo;
using Infrastructure.Module.Services.Settings;
using Infrastructure.Module.Views;
using Infrastructure.Module.Views.Settings;
using Prism.Ioc;
using Prism.Modularity;

namespace Infrastructure.Module
{
    public class InfrastructureModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .RegisterSingleton<ISerializableSettingsManager, SerializableSettingsManager>();

            containerRegistry
                .RegisterSingleton<IApplicationInfoService, ApplicationInfoService>()
                .RegisterSingleton<ISettingsViewManager, SettingsViewManager>()
                .RegisterSingleton<IPathService, PathService>()
                .RegisterSingleton<IProtobufSerializeService, ProtobufSerializeService>()
                .RegisterSingleton<IApplicationSettingsService, ApplicationSettingsService>()
                .RegisterSingleton<IApplicationSettingsRepositoryService, ApplicationSettingsRepositoryService>()
                ;

            containerRegistry.RegisterForNavigation<BaseSettingsView, BaseSettingsViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            //containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));

            // Применить загруженные настройки
            containerProvider.Resolve<IApplicationSettingsService>()?.ApplySavedSettings();

            containerProvider.Resolve<ISettingsViewManager>()
                .AddView<BaseSettingsView>("Общие", "Общие настройки");
        }
    }
}