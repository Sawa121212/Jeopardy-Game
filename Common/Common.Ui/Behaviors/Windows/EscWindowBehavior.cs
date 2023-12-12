using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.Windows
{
    public class EscWindowBehavior : Behavior<Window>
    {

        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.KeyDown += OnPreviewKeyDown;
            }
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.KeyDown -= OnPreviewKeyDown;
            }

            base.OnDetaching();
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                AssociatedObject?.Close();
        }
    }
}