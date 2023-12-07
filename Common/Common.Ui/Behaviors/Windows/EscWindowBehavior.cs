/*using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.Windows
{
    public class EscWindowBehavior : Behavior<Window>
    {
        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
        }

        protected override void OnCleanup()
        {
            AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
            base.OnCleanup();
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                AssociatedObject.Close();
        }
    }
}*/