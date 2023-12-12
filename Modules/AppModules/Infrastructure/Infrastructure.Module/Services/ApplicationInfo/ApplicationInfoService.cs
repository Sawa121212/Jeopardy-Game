using Infrastructure.Interfaces.Services;

namespace Infrastructure.Module.Services.ApplicationInfo
{
    public class ApplicationInfoService : IApplicationInfoService
    {
        /// <summary>
        /// Название приложения.
        /// </summary>
        private string  _applicationName;
        private string _applicationVersion;

        /// <inheritdoc />
        public string GetApplicationName()
        {
            return $"{_applicationName}";
        }

        /// <inheritdoc />
        public string GetApplicationVersion()
        {
            return _applicationVersion;
        }

        /// <inheritdoc />
        public string GetFullApplicationName()
        {
            return $"{_applicationName} v{_applicationVersion}";
        }

        /// <inheritdoc />
        public string GetApplicationTitle()
        {
            /*if(_licenseInformationService == null)
                return GetFullApplicationName();*/
            return $"{_applicationName} v{_applicationVersion} ";
        }

        /// <inheritdoc />
        public void SetApplicationName(string applicationName)
        {
            _applicationName = applicationName;
        }

        /// <inheritdoc />
        public void SetApplicationVersion(string applicationVersion)
        {
            _applicationVersion = applicationVersion;
        }
    }

}