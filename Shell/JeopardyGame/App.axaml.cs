using System;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Common.Core.Localization;
using JeopardyGame.Properties;
using JeopardyGame.Views;
using ModuleA;
using ModuleB;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using TelegramAPI;
using TelegramAPI.Test;
using TopicDb.Module;
using IResourceProvider = Common.Core.Localization.IResourceProvider;

namespace JeopardyGame
{
    public class App : PrismApplication
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            base.Initialize(); // <-- Required
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<ShellView>();
        }

        /// <summary>
        /// Регистрация служб приложения
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry

                // Register Services
                .RegisterSingleton<ILocalizer, Localizer>()
                .RegisterSingleton<IResourceProvider, ResourceProvider>(Assembly.GetExecutingAssembly().FullName)

                // Views - Generic
                .Register<ShellView>()

                // Views - Region Navigation
                //containerRegistry.RegisterForNavigation<DashboardView, DashboardViewModel>() 
                ;
        }

        /// <summary>
        /// Регистрация модулей приложения
        /// </summary>
        /// <param name="moduleCatalog"></param>
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog
                .AddModule<TopicDbModule>()
                .AddModule<ModuleAModule>()
                .AddModule<ModuleBModule>()
                .AddModule<TelegramApiTestModule>();

            base.ConfigureModuleCatalog(moduleCatalog);
        }

        /// <summary>Called after <seealso cref="Initialize"/>.</summary>
        //protected override void OnInitialized()
        //{
        //    // Register initial Views to Region.
        //    var regionManager = Container.Resolve<IRegionManager>();
        //    regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(DashboardView));
        //    regionManager.RegisterViewWithRegion(RegionNames.SidebarRegion, typeof(SidebarView));
        //}

        /// <summary>
        /// ViewModel Locator
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                string? viewName = viewType.FullName;
                string? viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;

                string viewModelName = string.Format(
                    viewName != null && viewName.EndsWith("View", StringComparison.OrdinalIgnoreCase)
                        ? "{0}Model, {1}"
                        : "{0}ViewModel, {1}",
                    viewName, viewAssemblyName);
                return Type.GetType(viewModelName);
            });

            ViewModelLocationProvider.SetDefaultViewModelFactory(type => Container.Resolve(type));
        }
    }
}