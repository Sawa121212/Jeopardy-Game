/*
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.GridSplitters
{
    public class ResizeGridSplitterBehavior : Behavior<GridSplitter>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            ((Grid)AssociatedObject.Parent).SizeChanged += GridSizeChanged;
        }

        private void GridSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            var grid = (Grid)sender;
            if (grid != null)
            {
                if (AssociatedObject != null)
                {

                    if (AssociatedObject.VerticalAlignment == VerticalAlignment.Stretch)
                        AssociatedObject.Height = grid.Height;
                    if (AssociatedObject.HorizontalAlignment == HorizontalAlignment.Stretch)
                        AssociatedObject.Width = grid.Width;
                }
            }
        }
    }
}
*/
