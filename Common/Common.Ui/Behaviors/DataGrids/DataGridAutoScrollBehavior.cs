/*
using System;
using System.Collections.Specialized;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.DataGrids
{
    /// <summary>
    /// При изменении коллекции прокручивает DataGrid до последнего элемента.
    /// </summary>
    [Obsolete("Use Cheaz.Wpf.Toolkit.DataGrids.ScrollableToLastDataGrid")]
    public class DataGridAutoScrollBehavior : Behavior<DataGrid>
    {
        public static readonly DependencyProperty AutoScrollToEndProperty = DependencyProperty.Register(
            "AutoScrollToEnd", typeof(bool), typeof(DataGridAutoScrollBehavior), new PropertyMetadata(default(bool)));
        
        public bool AutoScrollToEnd
        {
            get => (bool) GetValue(AutoScrollToEndProperty);
            set => SetValue(AutoScrollToEndProperty, value);
        }
        
        protected override void OnSetup()
        {
            base.OnSetup();
            var propertyDescriptor = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            propertyDescriptor?.AddValueChanged(AssociatedObject, ItemSourceChanged);


            if (AssociatedObject.ItemsSource is INotifyCollectionChanged collection)
            {
                collection.CollectionChanged += CollectionChanged;
            }
        }
        
        protected override void OnCleanup()
        {
            var propertyDescriptor = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            propertyDescriptor?.RemoveValueChanged(AssociatedObject, ItemSourceChanged);

            if (AssociatedObject.ItemsSource is INotifyCollectionChanged collection)
                collection.CollectionChanged -= CollectionChanged;

            base.OnCleanup();
        }

        private void ItemSourceChanged(object sender, EventArgs e)
        {
            if (AssociatedObject.ItemsSource is INotifyCollectionChanged collection)
            {
                collection.CollectionChanged += CollectionChanged;
            }
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (AutoScrollToEnd)
            {
                if (AssociatedObject is ItemsControl itemsControl)
                {
                    if (!itemsControl.Items.IsEmpty)
                    {
                        var lastItem = itemsControl.Items[itemsControl.Items.Count - 1];
                        itemsControl.Items.MoveCurrentTo(lastItem);
                        AssociatedObject.ScrollIntoView(lastItem);
                    }
                }
            }
        }
    }
}
*/
