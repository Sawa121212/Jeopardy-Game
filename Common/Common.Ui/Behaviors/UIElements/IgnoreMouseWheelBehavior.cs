/*
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.UIElements
{
    /// <summary>
    /// Пробрасывает дальше по дереву событие скоролинга колесом.
    /// </summary>
    public class IgnoreMouseWheelBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty BlockSignalProperty = DependencyProperty.Register(
            nameof(BlockSignal), typeof(bool), typeof(IgnoreMouseWheelBehavior), new PropertyMetadata(default(bool)));

        public bool BlockSignal
        {
            get => (bool)GetValue(BlockSignalProperty);
            set => SetValue(BlockSignalProperty, value);
        }
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
            if (BlockSignal)
                return;

            e.Handled = true;
            var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent
            };
            AssociatedObject.RaiseEvent(e2);
        }
    }
}
*/
