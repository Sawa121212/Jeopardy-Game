/*using Avalonia.Xaml.Interactivity;
using AvaloniaUI.Ribbon;

namespace Common.Ui.Behaviors.Ribbons
{
    public class RibbonContextMenuOpeningBehavior : Behavior<Ribbon>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ContextMenuOpening += OnClosingContextMenu;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ContextMenuOpening -= OnClosingContextMenu;
            base.OnDetaching();
        }

        private void OnClosingContextMenu(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }
    }
}*/