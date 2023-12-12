using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;

namespace Common.Core.IO
{
    /// <summary>
    /// Специальные папки Windows.
    /// </summary>
    /// TODO: Реализовать кроссплатформенность
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class SpecFolder
    {
        #region [---------- Вспомогательное ----------]
        
        /// <summary>
        /// Retrieves the full path of a known folder identified by the folder's KNOWNFOLDERID.
        /// </summary>
        [DllImport("shell32.dll")]
        internal static extern int SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
            uint dwFlags,
            IntPtr hToken,
            out IntPtr ppszPath
        );
        
        internal const uint KF_FLAG_DONT_VERIFY = 0x00004000;
        
        #endregion

        #region [---------- Получение путей к специальным папкам ОС ----------]

                
        /// <summary>
        /// Получает полный путь к временной директории.
        /// </summary>
        // TODO: Прежнее местоположение: DataBase.Domain/DataBase.Domain/Utils/ScadaAppDirs.cs.
        public static string GetTmpPath()
        {
            string? tmpPath = Path.GetTempPath();
            return tmpPath;
        }
        
        /// <summary>
        /// Возвращает путь к папке "Загрузки".
        /// </summary>
        // TODO: Прежнее местоположение: DataBase.Domain/External/ScadaPathUtils/ScadaPathProvider.cs.
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public static string GetDownloadsPath()
        {
            if (Components.SystemInfo.OsPlatformEx.IsWindows)
            {
                const int S_OK = 0;

                Guid downloadsFolderGuid = Guid.Parse("{374DE290-123F-4565-9164-39C4925E467B}");

                if (S_OK != SHGetKnownFolderPath(downloadsFolderGuid, KF_FLAG_DONT_VERIFY, IntPtr.Zero, out IntPtr pPath))
                {
                    return null;
                }

                string? path = Marshal.PtrToStringUni(pPath);
                Marshal.FreeCoTaskMem(pPath);
                
                return path;
            }

            if (Components.SystemInfo.OsPlatformEx.IsUnix)
            {
                // TODO: Возвратить путь к Загрузкам в Unix-системе
                return  string.Empty;
            }
            
            return  string.Empty;
        }
        
        #endregion    
    }
}