using Infrastructure.Interfaces.Managers;
using Infrastructure.Interfaces.Services;
using Infrastructure.Interfaces.Services.Settings;
using Infrastructure.Module.Managers;
using Infrastructure.Module.Services;
using Infrastructure.Module.Services.Settings;
using Infrastructure.Module.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Infrastructure.Module
{
    public class InfrastructureModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .RegisterSingleton<ISerializableSettingsManager, SerializableSettingsManager>()
                ;

            containerRegistry
                .RegisterSingleton<ISettingsViewManager, SettingsViewManager>()
                .RegisterSingleton<IApplicationSettingsRepositoryService, ApplicationSettingsRepositoryService>()
                .RegisterSingleton<IPathService, PathService>()
                .RegisterSingleton<IProtobufSerializeService, ProtobufSerializeService>()
                ;


            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}