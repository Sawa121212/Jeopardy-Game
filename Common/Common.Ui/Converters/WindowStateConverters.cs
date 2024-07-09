using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Common.Ui.Converters
{
    public static class WindowStateConverters
    {
        public static FuncValueConverter<WindowState, Thickness> ToContentMargin =
            new(state =>
            {
                if (OperatingSystem.IsWindows() && state == WindowState.Maximized)
                {
                    return new Thickness(6);
                }

                if (OperatingSystem.IsLinux() && state != WindowState.Maximized)
                {
                    return new Thickness(6);
                }

                return new Thickness(0);
            });

        public static FuncValueConverter<WindowState, GridLength> ToTitleBarHeight =
            new(_ => new GridLength(30));

        public static FuncValueConverter<WindowState, StreamGeometry> ToMaxOrRestoreIcon =
            new(state =>
            {
                if (state == WindowState.Maximized)
                {
                    return Application.Current?.FindResource("WindowRestore") as StreamGeometry;
                }
                else
                {
                    return Application.Current?.FindResource("WindowMaximize") as StreamGeometry;
                }
            });
    }
}