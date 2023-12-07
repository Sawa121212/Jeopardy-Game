/*
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.ComboBoxes
{
    public class DisableWheelBehavior : Behavior<ComboBox>
    {
        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.PreviewMouseWheel += OnPreviewMouseWheel;
        }

        protected override void OnCleanup()
        {
            AssociatedObject.PreviewMouseWheel -= OnPreviewMouseWheel;
            base.OnCleanup();
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
