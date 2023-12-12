using Common.Core.Localization;

namespace Infrastructure.Interfaces.Services.Settings
{
    public interface IResourceService
    {
        void AddResourceProvider(IResourceProvider resourceProvider);
    
        void ChangeResourcesEvent();
    }
}