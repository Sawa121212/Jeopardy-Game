/*using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Helpers;

namespace Common.Ui.Behaviors.TreeViews
{
    /// <summary>
    /// Находит, разворачивает и выделяет в дереве указанный элемент.
    /// </summary>
    public class TreeViewExpandSelectedElementBehavior : Behavior<TreeView>
    {
        private const int MaxRecursiveCount = 100;
        private int _recursiveEnties = 0;
        private bool _virtualized;

        /// <summary>
        /// Элемент который необходимо выделить в дереве.
        /// </summary>
        public static readonly DependencyProperty SelectedNodeElementProperty = DependencyProperty.Register(nameof(SelectedNodeElement),
            typeof(object), typeof(TreeViewExpandSelectedElementBehavior), new PropertyMetadata(default(object), SelectedItemChanged));

        /// <summary>
        /// Элемент который необъодимо выделить в дереве.
        /// </summary>
        public object SelectedNodeElement
        {
            get => (object)GetValue(SelectedNodeElementProperty);
            set => SetValue(SelectedNodeElementProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            var virtualizing = AssociatedObject.FindVisualChild<VirtualizingStackPanel>();
            if (virtualizing != null)
                _virtualized = true;
        }

        private static void SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeViewExpandSelectedElementBehavior behavior)
            {
                var treeView = behavior.AssociatedObject;
                if (treeView != null)
                {
                    if (e.NewValue is NodeElement nodeElement)
                    {
                        TreeViewHelper.SelectElement(behavior.AssociatedObject, nodeElement);
                    }
                }
            }
        }
    }
}*/