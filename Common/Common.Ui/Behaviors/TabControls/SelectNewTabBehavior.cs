/*
using System;
using System.Collections.Specialized;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.TabControls
{
    /// <summary>
    /// Вы
    /// </summary>
    public class SelectNewTabBehavior : Behavior<TabControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            var propertyDescriptor = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(TabControl));
            propertyDescriptor?.AddValueChanged(AssociatedObject, ItemSourceChanged);

            if (AssociatedObject.Items is INotifyCollectionChanged observable)
                observable.CollectionChanged += OnCollectionChanged;
           
        }
        
        protected override void OnDetaching()
        {
            if (AssociatedObject.Items is INotifyCollectionChanged observable)
                observable.CollectionChanged -= OnCollectionChanged;
           
            var propertyDescriptor = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(TabControl));
            propertyDescriptor?.RemoveValueChanged(AssociatedObject, ItemSourceChanged);
            base.OnDetaching();
        }

        private void ItemSourceChanged(object sender, EventArgs e)
        {
            if (!AssociatedObject.Items.IsEmpty)
            {
                AssociatedObject.SelectedIndex = AssociatedObject.Items.Count - 1;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!AssociatedObject.Items.IsEmpty)
            {
                AssociatedObject.SelectedIndex = AssociatedObject.Items.Count - 1;
            }
        }
    }
}
*/
