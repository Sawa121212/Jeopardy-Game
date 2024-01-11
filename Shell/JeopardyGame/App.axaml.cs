using System;
using System.Reflection;
using System.Resources;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Common.Core.Localization;
using Common.Core.Prism.Regions;
using Common.Ui.Managers;
using Common.Ui.Parameters;
using Confirmation.Module;
using Confirmation.Module.Services;
using Game;
using Infrastructure.Interfaces.Managers;
using Infrastructure.Interfaces.Services;
using Infrastructure.Module;
using Infrastructure.Module.Managers;
using Infrastructure.Module.Services;
using Infrastructure.Module.Services.ApplicationInfo;
using JeopardyGame.Properties;
using JeopardyGame.Views;
using ModuleB;
using Notification.Module;
using Notification.Module.Services;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using TelegramAPI.Test;
using TopicDb.Module;
using Users.Module;
using IResourceProvider = Common.Core.Localization.IResourceProvider;
using ShellView = JeopardyGame.Views.Shell.ShellView;

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
            MainWindow = Container.Resolve<ShellView>();
            MainWindow.Loaded += MainWindow_Loaded;
            return MainWindow;
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
                .RegisterSingleton<IResourceService, ResourceService>()
                .RegisterSingleton<IPresentationParameters, PresentationParameters>()
                .RegisterSingleton<IDialogManager, DialogManager<ShellView>>()
                .RegisterSingleton<IApplicationInfoService, ApplicationInfoService>()
                .RegisterSingleton<IProtobufSerializeService, ProtobufSerializeService>()
                .RegisterSingleton<IPathService, PathService>()
                .RegisterSingleton<ISerializableSettingsManager, SerializableSettingsManager>()

                // Notification
                .RegisterSingleton<INotificationService, NotificationService>()
                .RegisterSingleton<IConfirmationService, ConfirmationService>();

            // Views - Generic
            containerRegistry.Register<ShellView>();

            // Views - Region Navigation
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
            containerRegistry.RegisterForNavigation<PlayInfoView>();
        }

        /// <summary>
        /// Регистрация модулей приложения
        /// </summary>
        /// <param name="moduleCatalog"></param>
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog
                .AddModule<ConfirmationModule>()
                .AddModule<NotificationModule>()
                .AddModule<InfrastructureModule>()
                .AddModule<TopicDbModule>()
                .AddModule<UsersModule>()
                .AddModule<GameModule>()
                .AddModule<ModuleBModule>()
                .AddModule<TelegramApiTestModule>();

            base.ConfigureModuleCatalog(moduleCatalog);
        }


        protected override void InitializeShell(AvaloniaObject shell)
        {
            base.InitializeShell(shell);

            // Добавим локализацию
            Container.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));
        }


        private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime {MainWindow: not null} desktop)
            {
                Container.Resolve<INotificationService>().SetHostWindow(desktop.MainWindow);
            }
        }

        /// <summary>Called after <seealso cref="Initialize"/>.</summary>
        protected override void OnInitialized()
        {
            Container.Resolve<IRegionManager>()
                .RegisterViewWithRegion(RegionNameService.ShellRegionName, nameof(MainView))
                .RegisterViewWithRegion(RegionNameService.ContentRegionName, nameof(PlayInfoView));
        }

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

        private Window MainWindow { get; set; }
    }
}