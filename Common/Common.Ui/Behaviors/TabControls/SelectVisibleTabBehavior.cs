/*
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.TabControls
{
    public class SelectVisibleTabBehavior : Behavior<TabControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }
        
        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            base.OnDetaching();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (AssociatedObject != null)
            {
                foreach (var item in AssociatedObject.Items)
                {
                    if (item is TabItem tabItem)
                    {
                        if (tabItem.IsVisible)
                        {
                            tabItem.IsSelected = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}
*/
