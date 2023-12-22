using Common.Core.Interfaces.Settings;
using Material.Styles.Themes.Base;
using ProtoBuf;

namespace Infrastructure.Domain.Settings
{
    [ProtoContract]
    public class ApplicationSettings : ISettings
    {
        [ProtoMember(1)]
        private string _cultureInfo;

        [ProtoMember(2)]
        private BaseThemeMode _themeMode;

        [ProtoMember(3)]
        private bool _hideToSystemTray;

        /// <summary>
        /// Скрыть в системном трее
        /// </summary>
        public string UsedCulture
        {
            get => _cultureInfo;
            set => _cultureInfo = value;
        }

        /// <summary>
        /// Тема приложения
        /// </summary>
        public BaseThemeMode ThemeMode
        {
            get => _themeMode;
            set => _themeMode = value;
        }
    }
}