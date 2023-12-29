using System.Resources;
using Common.Core.Localization;
using Game.Mangers;
using Game.Views;
using Game.Properties;
using Game.Services;
using Prism.Ioc;
using Prism.Modularity;

namespace Game
{
    /// <summary>
    /// Модуль Game
    /// </summary>
    public class GameModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IRoundService, RoundService>();
            containerRegistry.RegisterSingleton<IRoomService, RoomService>();
            containerRegistry.RegisterSingleton<IGameManager, GameManager>();

            // регистрируем View для навигации по Регионам
            containerRegistry.RegisterForNavigation<GameView, GameViewModel>();
            containerRegistry.RegisterForNavigation<RoomView, RoomViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));
        }
    }
}