#nullable enable
using System.Resources;

namespace Common.Core.Localization
{
    public interface ILocalizer
    {
        void ChangeLanguage(string language);

        string? GetExpression(string key);

        void EditLanguage(string language);
        void AddResourceManager(ResourceManager resourceManager);
    }
}