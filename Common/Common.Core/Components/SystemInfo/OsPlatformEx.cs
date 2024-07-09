using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Common.Core.Components.SystemInfo
{

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class OsPlatformEx
    {
        /// <summary> 
        /// Получить текущую платформу ОС. 
        /// </summary> 
        public static OSPlatform Current
        {
            get
            {
                OSPlatform osPlatform = OSPlatform.Create("Other Platform");
                // Check if it's Windows 
                bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                osPlatform = isWindows ? OSPlatform.Windows : osPlatform;
                // Check if it's OSx 
                bool isOsx = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
                osPlatform = isOsx ? OSPlatform.OSX : osPlatform;
                // Check if it's Linux 
                bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
                osPlatform = isLinux ? OSPlatform.Linux : osPlatform;

                return osPlatform;
            }
        }

        /// <summary>
        /// Проверяет, является ли текущая платформа Unix совместимой.
        /// </summary>
        public static bool IsUnix
        {
            get
            {
                OSPlatform osPlatform = Current;
                return osPlatform == OSPlatform.Linux || osPlatform == OSPlatform.OSX;
            }
        }

        /// <summary>
        /// Проверяет, является ли текущая платформа Windows.
        /// </summary>
        public static bool IsWindows => Current == OSPlatform.Windows;
    }
}