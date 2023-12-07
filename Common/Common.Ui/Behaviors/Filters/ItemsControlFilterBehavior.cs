/*using System;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.Filters
{
    /// <summary>
    /// Обеспечивает фильтрацию для элементов ItemsControl
    /// </summary>
    public class ItemsControlFilterBehavior : Behavior<ItemsControl>
    {
        /// <summary>
        /// Текст фильтра.
        /// </summary>
        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(
            nameof(SearchText), typeof(string), typeof(ItemsControlFilterBehavior), new PropertyMetadata(default(string), SearchTextChanged));

        /// <summary>
        /// Текст фильтра.
        /// </summary>
        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }

        private static void SearchTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ItemsControlFilterBehavior behavior)
            {
                if (e.NewValue is string text)
                    behavior.OnSearchTextChanged(text);
            }
        }

        private void OnSearchTextChanged(string searchText)
        {
            if (AssociatedObject != null)
                Filter(AssociatedObject.Items, searchText);
        }

        private void Filter(ItemCollection items, string searchText)
        {
            var collectionViewSource = CollectionViewSource.GetDefaultView(items);
            collectionViewSource.Filter = item =>
                item.ToString().IndexOf(searchText, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}*/