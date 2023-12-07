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
        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.KeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.System && keyEventArgs.SystemKey == Key.F4)
            {
                keyEventArgs.Handled = true;
            }
        }

        protected override void OnCleanup()
        {
            AssociatedObject.KeyDown -= OnKeyDown;
            base.OnCleanup();
        }
    }
}
*/
