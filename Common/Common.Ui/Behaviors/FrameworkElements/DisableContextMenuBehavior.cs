/*
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.FrameworkElements
{
    /// <summary>
    /// Блокировать открытие контекстного меню если выставлен флаг BlockMenuOpening.
    /// </summary>
    public class DisableContextMenuBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty BlockMenuOpeningProperty = DependencyProperty.Register(
            "BlockMenuOpening", typeof(bool), typeof(DisableContextMenuBehavior), new PropertyMetadata(default(bool)));

        public bool BlockMenuOpening
        {
            get => (bool) GetValue(BlockMenuOpeningProperty);
            set => SetValue(BlockMenuOpeningProperty, value);
        }
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ContextMenuOpening += OnContextMenuOpening;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ContextMenuOpening -= OnContextMenuOpening;
            base.OnDetaching();
        }

        private void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (BlockMenuOpening)
                e.Handled = true;
        }
    }
}
*/
