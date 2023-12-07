/*using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Input;
using Common.Ui.Helpers;

namespace Common.Ui.Behaviors.TreeViews
{
    public class TreeViewRemoveAndSelectElementBehavior : BehaviorBase <TreeView>
    {
        public static readonly DependencyProperty RemoveCommandProperty = DependencyProperty.Register(
            nameof(RemoveCommand), typeof(ICommand), typeof(TreeViewRemoveAndSelectElementBehavior), new PropertyMetadata(default(ICommand)));

        public ICommand RemoveCommand
        {
            get => (ICommand) GetValue(RemoveCommandProperty);
            set => SetValue(RemoveCommandProperty, value);
        }
        
        /// <inheritdoc />
        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.PreviewKeyUp +=OnPreviewKeyUp;    
        }

        /// <inheritdoc />
        protected override void OnCleanup()
        {
            AssociatedObject.PreviewKeyUp -=OnPreviewKeyUp;    
            base.OnCleanup();
        }
        
        private void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if(RemoveCommand == null)
                return;

            if (e.Key != Key.Delete) 
                return;

            if (!(AssociatedObject.SelectedItem is NodeElement nodeElement)) 
                return;

            if (nodeElement.IsRequired) 
                return;

            if (!(e.OriginalSource is TreeViewItem treeViewItem)) 
                return;

            if (!nodeElement.Equals(treeViewItem.DataContext)) 
                return;
            
            var nodeParent = nodeElement.Parent;
            if (nodeParent == null) 
                return;
            
            var removeChildIndex = nodeParent.Children.IndexOf(nodeElement);
            var treeViewItemParent = ItemsControl.ItemsControlFromItemContainer(treeViewItem);
            
            if (treeViewItemParent == null) 
                return;

            if (!nodeParent.Equals(treeViewItemParent.DataContext)) 
                return;
            
            RemoveCommand.Invoke(nodeElement);
                                            
            var childrenCount = nodeParent.Children.Count;
            if (childrenCount > removeChildIndex)
            {
                //при удаленни остаемся на том же индексе
                var element = nodeParent.Children.ElementAt(removeChildIndex);
                if (element != null)
                {
                    TreeViewHelper.SelectChildElement(treeViewItemParent, element);   
                }
            }
            else if (childrenCount > 0)
            {
                //или индекс выделяем последний элемент того же родителя.
                var element = nodeParent.Children.ElementAt(childrenCount - 1);
                if (element != null)
                {
                    TreeViewHelper.SelectChildElement(treeViewItemParent, nodeParent.Children.ElementAt(childrenCount - 1));   
                }
            }
        }
    }
}*/