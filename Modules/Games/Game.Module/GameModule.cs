using System.Resources;
using Common.Core.Localization;
using Game.Domain.Data;
using Game.Infrastructure.Interfaces.Mangers;
using Game.Infrastructure.Interfaces.Services;
using Game.Infrastructure.Mangers;
using Game.Infrastructure.Services;
using Game.Module.Properties;
using Game.Ui.Views;
using Game.Ui.Views.GameControls;
using Game.Ui.Views.GameControls.GamePages;
using Game.Ui.Views.GameControls.GamePages.Rounds;
using Game.Ui.Views.GameControls.GamePages.Topics;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Game.Module
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
            containerRegistry.RegisterForNavigation<GameWinnerView, GameWinnerViewModel>();
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