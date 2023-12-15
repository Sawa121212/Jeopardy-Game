using System.Resources;
using Common.Core.Localization;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using TelegramAPI.Test.Properties;
using TelegramAPI.Test.Views;

namespace TelegramAPI.Test
{
    /// <summary>
    /// Модуль C
    /// </summary>
    public class TelegramApiTestModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public TelegramApiTestModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // регистрируем View для навигации по Регионам
            containerRegistry.RegisterForNavigation<TelegramTestView>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));

            // Зарегистрировать View к региону. Теперь при запуске ПО View будет показано
            //_regionManager.RegisterViewWithRegion("RegionC", typeof(TabCView));
        }
    }
}