/*
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.ComboBoxes
{
    public class DisableWheelBehavior : Behavior<ComboBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseWheel += OnPreviewMouseWheel;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseWheel -= OnPreviewMouseWheel;
            base.OnDetaching();
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (this.AssociatedObject.IsDropDownOpen == false)
            {
                e.Handled = true;

                if (AssociatedObject.Parent is FrameworkElement parentElement)
                {
                    parentElement.RaiseEvent(new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                    {
                        RoutedEvent = UIElement.MouseWheelEvent,
                        Source = sender
                    });
                }
            }
        }
    }
}
*/
