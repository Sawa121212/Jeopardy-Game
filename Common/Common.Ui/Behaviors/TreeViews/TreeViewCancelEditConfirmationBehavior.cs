/*
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.TreeViews
{
    /// <summary>
    /// При наличии несохраненных изменений у элемента дерева вызывает диалог с подтверждением.
    /// </summary>
    public class TreeViewCancelEditConfirmationBehavior : Behavior<TreeView>
    {
        /// <summary>
        /// Команда вызывается при продолжении без сохранения данных.
        /// </summary>
        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(
            "CancelCommand", typeof(ICommand), typeof(TreeViewCancelEditConfirmationBehavior), new PropertyMetadata(default(ICommand)));

        /// <summary>
        ///  Команда вызывается при продолжении без сохранения данных.
        /// </summary>
        public ICommand CancelCommand
        {
            get => (ICommand) GetValue(CancelCommandProperty);
            set => SetValue(CancelCommandProperty, value);
        }
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseLeftButtonDown += PreviewMouseLeftButtonDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewMouseLeftButtonDown -= PreviewMouseLeftButtonDown;
        }

        private void PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeView treeView)
            {
                if (treeView.SelectedItem is IEditable editable && editable.IsEdit)
                {
                    if (MessageBox.Show("Имеются несохраненные изменения. Продолжить без сохранения данных?", "Подтверждение",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        editable.CancelEdit();
                        CancelCommand?.Invoke();
                        var treeViewItem = treeView.GetTreeViewItemClicked((FrameworkElement)e.OriginalSource);
                        if (treeViewItem != null)
                            treeViewItem.IsSelected = true;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
*/
