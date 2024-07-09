using System;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;

namespace Notification.Module.Services
{
    /// <summary>
    /// Сервис отображения уведомления
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Таймер отображения уведомления
        /// </summary>
        int NotificationTimeout { get; set; }

        /// <summary>
        /// Установить главное окно для сообщений
        /// </summary>
        /// <param name="hostWindow">Родительское окно</param>
        void SetHostWindow(TopLevel hostWindow);

        /// <summary>
        /// Показать уведомление
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="message">Текст</param>
        /// <param name="notificationType">Тип уведомления</param>
        /// <param name="onClick">Выполнить действие по клику по уведомлению</param>
        void Show(string title, string message, NotificationType notificationType, Action? onClick = null);
    }
}