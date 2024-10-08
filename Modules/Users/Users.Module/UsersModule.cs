using Prism.Ioc;
using Prism.Modularity;

namespace Users.Module
{
    /// <summary>
    /// Модуль управления БД пользователей
    /// </summary>
    public class UsersModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            //containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));
        }
    }
}