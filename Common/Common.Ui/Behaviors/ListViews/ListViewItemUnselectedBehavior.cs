/*using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.ListViews
{
    /// <summary>
    /// Убрать выбранный элемент
    /// </summary>
    public class ListViewItemUnselectedBehavior : Behavior<ListView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeave += OnMouseLeave;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeave -= OnMouseLeave;
            base.OnDetaching();
        }

        /// <summary>
        /// Убрать выбранный элемент
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (AssociatedObject == null) return;
            AssociatedObject.SelectedItem = null;
        }
    }
}*/