/*using Avalonia.Xaml.Interactivity;
using AvaloniaUI.Ribbon;

namespace Common.Ui.Behaviors.Ribbons
{
    public class RibbonContextMenuOpeningBehavior : Behavior<Ribbon>
    {
        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.ContextMenuOpening += OnClosingContextMenu;
        }

        protected override void OnCleanup()
        {
            AssociatedObject.ContextMenuOpening -= OnClosingContextMenu;
            base.OnCleanup();
        }

        private void OnClosingContextMenu(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }
    }
}*/