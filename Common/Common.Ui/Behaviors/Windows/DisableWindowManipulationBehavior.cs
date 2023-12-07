/*
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.Windows
{
    public class DisableWindowManipulationBehavior : Behavior<Window>
    {
        private WindowStyle _storedWindowStyle;

        protected override void OnAttached()
        {
            base.OnAttached();
            _storedWindowStyle = AssociatedObject.WindowStyle;
            AssociatedObject.WindowStyle = WindowStyle.None;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.WindowStyle = _storedWindowStyle;
            base.OnDetaching();
        }
    }
}
*/
