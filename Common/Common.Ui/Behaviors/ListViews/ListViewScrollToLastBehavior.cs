/*using System.Collections.Specialized;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.ListViews
{
    public class ListViewScrollToLastBehavior : Behavior<ListView>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnLoaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Unloaded -= OnUnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var incc = AssociatedObject.ItemsSource as INotifyCollectionChanged;
            if (incc == null) return;

            incc.CollectionChanged += OnCollectionChanged;
        }

        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            var incc = AssociatedObject.ItemsSource as INotifyCollectionChanged;
            if (incc == null) return;

            incc.CollectionChanged -= OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var count = AssociatedObject.Items.Count;
                if (AssociatedObject.Items.Count > 0)
                {
                    var item = AssociatedObject.Items[count - 1];
                    AssociatedObject.ScrollIntoView(item);
                }
            }
        }
    }
}*/