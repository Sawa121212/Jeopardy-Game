/*
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.TreeViews
{
    /// <summary>
    /// Обеспечивает правильную горизонтальную прокрутку дерева с заголовком
    /// </summary>
    public class TreeViewScrollHeaderBehavior : Behavior<TreeView>
    {
        private ScrollViewer _headerScrollViewer;
        private ScrollViewer _contentScrollViewer;

        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.Loaded += OnLoaded;

        }

        protected override void OnCleanup()
        {
            AssociatedObject.Loaded -= OnLoaded;

            if (_contentScrollViewer != null)
                _contentScrollViewer.ScrollChanged -= OnContentScrollChanged;

            base.OnCleanup();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _headerScrollViewer = (ScrollViewer)AssociatedObject.Template.FindName("ScrollViewer_Header", AssociatedObject);
            _contentScrollViewer = (ScrollViewer)AssociatedObject.Template.FindName("ScrollViewer_Content", AssociatedObject);
            if (_contentScrollViewer != null)
                _contentScrollViewer.ScrollChanged += OnContentScrollChanged;
        }

        private void OnContentScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_headerScrollViewer != null)
            {
                _headerScrollViewer.Width = e.ViewportWidth;
                _headerScrollViewer.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }
    }
}
*/
