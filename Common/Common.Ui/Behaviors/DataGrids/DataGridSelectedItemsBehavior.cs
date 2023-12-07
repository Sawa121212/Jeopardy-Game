/*
using System.Collections;
using System.Collections.Specialized;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.DataGrids
{
    /// <summary>
    /// Предоставляет коллекцию выделенных элементов в DataGrid.
    /// </summary>
    public class DataGridSelectedItemsBehavior : Behavior<DataGrid>
    {
        private SimpleMonitor _monitor;

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItems", typeof(IList),
                typeof(DataGridSelectedItemsBehavior), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertySelectedItemsChanged));

        private static void PropertySelectedItemsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            NotifyCollectionChangedEventHandler handler = (s, e) => SelectedItemsChanged(dependencyObject, e);

            if (dependencyPropertyChangedEventArgs.OldValue is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -= handler;
            }

            if (dependencyPropertyChangedEventArgs.NewValue is INotifyCollectionChanged newCollection)
            {
                var behavior = dependencyObject as DataGridSelectedItemsBehavior;
                if (behavior?.AssociatedObject != null)
                {
                    if (dependencyPropertyChangedEventArgs.NewValue is IList list)
                    {
                        foreach (var item in list)
                        {
                            if (!behavior.AssociatedObject.SelectedItems.Contains(item))
                                behavior.AssociatedObject.SelectedItems.Add(item);
                        }
                    }
                }
                newCollection.CollectionChanged += handler;
            }
        }

        public IList SelectedItems
        {
            get => (IList)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            _monitor = new SimpleMonitor();
            AssociatedObject.PreviewMouseDown += OnPreviewMouseDown;
            AssociatedObject.MouseEnter += OnMouseEnter;
            AssociatedObject.SelectionChanged += OnSelectionChanged;
            AssociatedObject.PreviewMouseRightButtonUp += OnPreviewMouseRightButtonUp;
        }

        private void OnPreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject != null)
            {
                AssociatedObject.PreviewMouseDown -= OnPreviewMouseDown;
                AssociatedObject.MouseEnter -= OnMouseEnter;
                AssociatedObject.SelectionChanged -= OnSelectionChanged;
            }
        }

        private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.OriginalSource is DataGridRow row 
                && (mouseEventArgs.LeftButton == MouseButtonState.Pressed
                || mouseEventArgs.RightButton == MouseButtonState.Pressed))
            {
                row.IsSelected = !row.IsSelected;
            }
            mouseEventArgs.Handled = true;
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.LeftButton == MouseButtonState.Pressed 
                || mouseButtonEventArgs.RightButton == MouseButtonState.Pressed )
            {
                if (mouseButtonEventArgs.OriginalSource is DependencyObject dep)
                {
                    var row = dep.TryFindParent<DataGridRow>();
                    if (row != null)
                    {
                        row.IsSelected = !row.IsSelected;
                    }
                }
                mouseButtonEventArgs.Handled = true;
            }
        }

        private static void SelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is DataGridSelectedItemsBehavior behavior)
            {
                var selectedItems = behavior.AssociatedObject.SelectedItems;
               
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems)
                    {
                        if (selectedItems.Contains(item))
                        {
                            selectedItems.Remove(item);
                        }
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        if (!selectedItems.Contains(item))
                        {
                            selectedItems.Add(item);
                        }
                    }
                }
            }
        }
        
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_monitor.Busy)
            {
                _monitor.Enter();
                if (e.AddedItems != null && e.AddedItems.Count > 0 && SelectedItems != null)
                {
                    foreach (var item in e.AddedItems)
                    {
                        if (!SelectedItems.Contains(item))
                            SelectedItems.Add(item);
                    }

                }

                if (e.RemovedItems != null && e.RemovedItems.Count > 0 && SelectedItems != null)
                {
                    foreach (var item in e.RemovedItems)
                    {
                        if (SelectedItems.Contains(item))
                            SelectedItems.Remove(item);
                    }
                }
                _monitor.Dispose();
            }
        }
    }
}
*/
