/*
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.Windows
{

    /// <summary>
    /// Перехватывает нажатие Alt+F4, не давая закрыться окну.
    /// </summary>
    public class DisableClosingWindowBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.System && keyEventArgs.SystemKey == Key.F4)
            {
                keyEventArgs.Handled = true;
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.KeyDown -= OnKeyDown;
            base.OnDetaching();
        }
    }
}
*/
