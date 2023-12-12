/*
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.TreeViews
{
    public class TreeViewRightClickToSelectBehavior : Behavior<TreeView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseRightButtonDown += OnMouseRightButtonDown;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseRightButtonDown -= OnMouseRightButtonDown;
            base.OnDetaching();
        }


        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject dependencyObject)
            {
                var item = dependencyObject.TryFindParent<TreeViewItem>();
                if (item != null)
                {
                    item.IsSelected = true;
                    e.Handled = true;
                }
            }
            
        }
    }
}
*/
