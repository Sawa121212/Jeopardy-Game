/*using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.TreeViews
{
    public class TreeViewDoubleClickBehavior : Behavior<TreeView>
    {
        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.Register(
            nameof(DoubleClickCommand), typeof(ICommand), typeof(TreeViewDoubleClickBehavior), new PropertyMetadata(default(ICommand)));

        public ICommand DoubleClickCommand
        {
            get => (ICommand) GetValue(DoubleClickCommandProperty);
            set => SetValue(DoubleClickCommandProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter), typeof(object), typeof(TreeViewDoubleClickBehavior), new PropertyMetadata(default(object)));

        public object CommandParameter
        {
            get => (object) GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
        
        /// <inheritdoc />
        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.PreviewMouseDoubleClick += OnPreviewMouseDoubleClick;
        }

        /// <inheritdoc />
        protected override void OnCleanup()
        {
            AssociatedObject.PreviewMouseDoubleClick -= OnPreviewMouseDoubleClick;
            base.OnCleanup();
        }
        
        private void OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject dependencyObject)
            {
                var item = dependencyObject.TryFindParent<TreeViewItem>();
                if (item != null)
                {
                    item.IsSelected = true;
                    if (DoubleClickCommand != null)
                    {
                        if(CommandParameter != null)
                            DoubleClickCommand.Invoke(CommandParameter);
                        else 
                            DoubleClickCommand.Invoke();
                    }

                    e.Handled = false;
                }
            }
        }
    }
}*/