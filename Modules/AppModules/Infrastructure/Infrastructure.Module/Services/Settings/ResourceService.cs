using System.Collections.ObjectModel;
using System.Linq;
using Common.Core.Localization;
using Common.Extensions.Collections;
using Infrastructure.Interfaces.Services.Settings;

namespace Infrastructure.Module.Services.Settings
{
    public class ResourceService : IResourceService
    {
        private ObservableCollection<IResourceProvider> _resourceProviders;

        public ResourceService()
    {
        _resourceProviders = new ObservableCollection<IResourceProvider>();
    }

        public void AddResourceProvider(IResourceProvider resourceProvider)
    {
        if (resourceProvider != null)
        {
            _resourceProviders.Add(resourceProvider);
        }
    }

        public void ChangeResourcesEvent()
    {
        if (_resourceProviders.Any())
        {
            _resourceProviders.ForEach(e => e.ChangeResources());
        }
    }
    }
}