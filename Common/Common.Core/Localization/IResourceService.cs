namespace Common.Core.Localization
{
    public interface IResourceService
    {
        void AddResourceProvider(IResourceProvider? resourceProvider);
    
        void ChangeResourcesEvent();
    }
}