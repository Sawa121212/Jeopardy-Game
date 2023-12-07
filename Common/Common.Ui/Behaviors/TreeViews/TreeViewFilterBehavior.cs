/*using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using Common.Extensions;

namespace Common.Ui.Behaviors.TreeViews
{
    /// <summary>
    /// Обеспечивает фильтрацию по Name и Description дерева из NodeElement.
    /// </summary>
    public class TreeViewFilterBehavior : Behavior<TreeView>
    {
        public static readonly DependencyProperty FilterTextProperty = DependencyProperty.Register(nameof(FilterText), typeof(string),
            typeof(TreeViewFilterBehavior), new PropertyMetadata(default(string), FilterTextChanged));

        public string FilterText
        {
            get => (string)GetValue(FilterTextProperty);
            set => SetValue(FilterTextProperty, value);
        }

        /// <inheritdoc />
        protected override void OnSetup()
        {
            base.OnSetup();
        }

        /// <inheritdoc />
        protected override void OnCleanup()
        {
            base.OnCleanup();
        }

        private static void FilterTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeViewFilterBehavior behavior)
            {
                behavior.OnFilterTextChanged();
            }
        }

        private void OnFilterTextChanged()
        {
            if (AssociatedObject == null)
                return;

            CreateFilter(AssociatedObject.Items.Cast<NodeElement>(), FilterText);
        }

        private bool CreateFilter(IEnumerable<NodeElement> items, string filterText)
        {
            var visible = false;
            var collectionView = CollectionViewSource.GetDefaultView(items);
            if (collectionView != null)
            {
                collectionView.Filter = item =>
                {
                    if (!(item is NodeElement element))
                        return false;

                    if (CreateFilter(element.Children, filterText))
                    {
                        visible = true;
                        return true;
                    }

                    if (IsContains(element, filterText))
                    {
                        visible = true;
                        return true;
                    }

                    return false;
                };
            }

            if (filterText.IsNullOrEmpty())
                return true;

            return visible;
        }

        private bool IsContains(NodeElement item, string filterText)
        {
            if (!item.Name.IsNullOrEmpty() && item.Name.ToLower().Contains(filterText.ToLower()))
                return true;

            if (!item.Description.IsNullOrEmpty() && item.Description.ToLower().Contains(filterText.ToLower()))
                return true;
            return false;
        }
    }
}*/