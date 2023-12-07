/*
using System;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using Common.Ui.ScreenHelper;

namespace Common.Ui.Behaviors.Windows
{
    /// <summary>
    /// Поведение блокировки сворачивания окна.
    /// </summary>
    public class DisableMinimizeBehavior : Behavior<Window>
    {
        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject.IsLoaded)
            {
                DisableMinimizeButton();
                RemoveMinimizeMenu();
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
            EnableMinimizeButton();
            ResetMenu();
            base.OnDetaching();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            DisableMinimizeButton();
            RemoveMinimizeMenu();
        }

        /// <summary>
        /// Блокировка кнопки "свернуть".
        /// </summary>
        /// <remarks>для DXWindow это невозможно сделать через SetWindowLongPtr</remarks>
        private void DisableMinimizeButton()
        {
            try
            {
                var hwnd = NativeMethods.GetWindowHandler(AssociatedObject);
                var currentStyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_STYLE);
                NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, currentStyle & ~NativeMethods.WS_MINIMIZEBOX);
            }
            catch (Win32Exception)
            {
                
            }
        }

        /// <summary>
        /// Блокировка кнопки "свернуть".
        /// </summary>
        /// <remarks>для DXWindow это невозможно сделать через SetWindowLongPtr</remarks>
        private void EnableMinimizeButton()
        {
            try
            {
                var hwnd = NativeMethods.GetWindowHandler(AssociatedObject);
                var currentStyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_STYLE);
                NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, currentStyle | NativeMethods.WS_MINIMIZEBOX);
            }
            catch (Win32Exception)
            {
                
            }
        }

        /// <summary>
        /// Блокировка пункта "свернуть" в меню окна.
        /// </summary>
        private void RemoveMinimizeMenu()
        {
            var hwnd = NativeMethods.GetWindowHandler(AssociatedObject);
            var hMenu = NativeMethods.GetSystemMenu(hwnd, false);
            if (hMenu == IntPtr.Zero)
            {
                NativeMethods.ThrowLastWin32Exception("Failed to get system menu");
            }

            if (NativeMethods.RemoveMenu(hMenu, (uint)NativeMethods.MenuCommand.SC_MINIMIZE, NativeMethods.MF_BYCOMMAND) == 0)
            {
                NativeMethods.ThrowLastWin32Exception("Failed to remove minimize menu");
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
