using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;

namespace Notification.Module.Services
{
    /// <inheritdoc cref="INotificationService"/>
    public class NotificationService : INotificationService
    {
        /// <inheritdoc/>
        public int NotificationTimeout
        {
            get => _notificationTimeout;
            set => _notificationTimeout = (value < 0) ? 0 : value;
        }

        /// <inheritdoc/>
        public void SetHostWindow(TopLevel hostWindow)
        {
            if (hostWindow is not Window window)
            {
                return;
            }

            WindowNotificationManager? notificationManager = new(window)
            {
                Position = NotificationPosition.BottomRight,
                MaxItems = 4,
                Margin = new Thickness(0, 0, 15, 40)
            };

            _notificationManager = notificationManager;
        }

        /// <inheritdoc/>
        public void Show(string title, string message, NotificationType notificationType, Action? onClick = null)
        {
            if (_notificationManager is { } notificationManager)
            {
                notificationManager.Show(
                    new Avalonia.Controls.Notifications.Notification(
                        title,
                        message,
                        notificationType,
                        TimeSpan.FromSeconds(_notificationTimeout),
                        onClick));
            }
            else
            {
                //throw new Exception("Host window not set");
            }
        }

        private int _notificationTimeout = 10;
        private WindowNotificationManager? _notificationManager;
    }
}