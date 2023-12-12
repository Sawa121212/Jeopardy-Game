/*
using System;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.Buttons
{
    public class CloseDialogWithCommandCancelationBehavior : Behavior<Button>
    {
        /// <summary>
        /// Команда при нажатии на кнопку.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(CloseDialogWithCommandCancelationBehavior), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// DialogResult dependency property
        /// </summary>
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.Register("DialogResult", typeof(bool), typeof(CloseDialogWithCommandCancelationBehavior), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Запрет закрытия окна.
        /// </summary>
        public static readonly DependencyProperty CancelProperty = DependencyProperty.Register(
            "Cancel", typeof(bool), typeof(CloseDialogWithCommandCancelationBehavior), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Команда при нажатии на кнопку.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Запрет закрытия окна.
        /// </summary>
        public bool Cancel
        {
            get => (bool)GetValue(CancelProperty);
            set => SetValue(CancelProperty, value);
        }

        /// <summary>
        /// Dismiss dialog result value - this is assigned to the window.DialogResult to close the window.
        /// </summary>
        public bool DialogResult
        {
            get => (bool)GetValue(DialogResultProperty);
            set => SetValue(DialogResultProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Click += OnButtonClick;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Click += OnButtonClick;
            base.OnDetaching();
        }

        /// <summary>
        /// This dismisses the associated window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var winParent = AssociatedObject.TryFindParent<Window>();
            if (winParent != null)
            {
                if (Command is IAsyncCommand asyncCommand)
                {
                    await asyncCommand.ExecuteAsync(null);
                }
                else
                {
                    Command?.Invoke();
                }
                if (!Cancel)
                {
                    CloseWindow(winParent);
                }
            }
        }

        private void CloseWindow(Window winParent)
        {
            // Walk up the visual tree and find the window this button is owned by.
            try
            {
                winParent.DialogResult = DialogResult;
            }
            catch (InvalidOperationException)
            {
                // Not a dialog window-- just close it directly.
                winParent.Close();
            }
        }
    }
}
*/
