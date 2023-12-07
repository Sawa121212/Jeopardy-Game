/*
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.TreeViews
{
    public class TreeViewRightClickToSelectBehavior : Behavior<TreeView>
    {
        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.PreviewMouseRightButtonDown += OnMouseRightButtonDown;
        }

        protected override void OnCleanup()
        {
            AssociatedObject.PreviewMouseRightButtonDown -= OnMouseRightButtonDown;
            base.OnCleanup();
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
