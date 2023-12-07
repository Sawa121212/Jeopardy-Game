/*
using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using Common.Ui.ScreenHelper;

namespace Common.Ui.Behaviors.Windows
{
    public class DisableMaximizeBehavior : Behavior<Window>
    {
        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject.IsLoaded)
            {
                DisableMaximizeButton();
                RemoveFromMenu();
            }
            else
            {
                AssociatedObject.Loaded += OnLoaded;
            }
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            EnableMaximizeButton();
            ResetMenu();
            base.OnDetaching();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            DisableMaximizeButton();
            RemoveFromMenu();
        }

        /// <summary>
        /// Блокировка кнопки "maximize".
        /// </summary>
        /// <remarks>для DXWindow это невозможно сделать через SetWindowLongPtr</remarks>
        private void DisableMaximizeButton()
        {
            var hwnd = NativeMethods.GetWindowHandler(AssociatedObject);
            var style = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_STYLE);
            NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, style & ~NativeMethods.WS_MAXIMIZEBOX);
        }

        private void EnableMaximizeButton()
        {
            var hwnd = NativeMethods.GetWindowHandler(AssociatedObject);
            var currentStyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_STYLE);
            NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, currentStyle | NativeMethods.WS_MAXIMIZEBOX);
        }

        /// <summary>
        /// Блокировка пункта "maximize" в меню окна.
        /// </summary>
        private void RemoveFromMenu()
        {
            var hwnd = NativeMethods.GetWindowHandler(AssociatedObject);
            var hMenu = NativeMethods.GetSystemMenu(hwnd, false);
            if (hMenu == IntPtr.Zero)
            {
                NativeMethods.ThrowLastWin32Exception("Failed to get system menu");
            }

            if (NativeMethods.RemoveMenu(hMenu, (uint)NativeMethods.MenuCommand.SC_MAXIMIZE, NativeMethods.MF_BYCOMMAND) == 0)
            {
                NativeMethods.ThrowLastWin32Exception("Failed to remove maximize menu");
            }

            if (NativeMethods.RemoveMenu(hMenu, (uint)NativeMethods.MenuCommand.SC_RESTORE, NativeMethods.MF_BYCOMMAND) == 0)
            {
                NativeMethods.ThrowLastWin32Exception("Failed to remove maximize menu");
            }

           
        }

        private void ResetMenu()
        {
            var hwnd = NativeMethods.GetWindowHandler(AssociatedObject);
            var hMenu = NativeMethods.GetSystemMenu(hwnd, true);
        }
    }
}
*/
