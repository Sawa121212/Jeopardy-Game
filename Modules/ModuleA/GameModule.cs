using System.Resources;
using Common.Core.Localization;
using Game.Data;
using Game.Mangers;
using Game.Views;
using Game.Properties;
using Game.Services;
using Game.Views.GameControls;
using Game.Views.GameControls.GamePages;
using Game.Views.GameControls.GamePages.Rounds;
using Game.Views.GameControls.GamePages.Topics;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

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
            containerRegistry.RegisterForNavigation<AllTopicsNameView, AllTopicsNameViewModel>();
            containerRegistry.RegisterForNavigation<RoundLevelView, RoundLevelViewModel>();
            containerRegistry.RegisterForNavigation<CorrectAnswerView>();
            containerRegistry.RegisterForNavigation<DisplayedQuestionView>();
            containerRegistry.RegisterForNavigation<BaseRoundControlView>();
            containerRegistry.RegisterForNavigation<TopicsNameCarouselControlView, TopicsNameCarouselControlViewModel>();
            containerRegistry.RegisterForNavigation<GameView, GameViewModel>();
            containerRegistry.RegisterForNavigation<RoomView, RoomViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));

            containerProvider.Resolve<IRegionManager>()
                .RegisterViewWithRegion(GameRegionNameService.ContentRegionName, nameof(BaseRoundControlView));
        }
    }
}