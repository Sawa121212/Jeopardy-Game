using GameSender.Infrastructure;
using GameSender.Infrastructure.Interfaces;
using Prism.Ioc;
using Prism.Modularity;

namespace GameSender.Module
{
    /// <summary>
    /// Модуль Game
    /// </summary>
    public class GameSenderModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IGameSenderService, GameSenderService>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            //containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));
        }
    }
}