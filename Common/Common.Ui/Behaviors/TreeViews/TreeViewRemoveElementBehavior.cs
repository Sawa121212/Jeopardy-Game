/*using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.TreeViews
{
    public class TreeViewRemoveElementBehavior : Behavior<TreeView>
    {
        public static readonly DependencyProperty DeleteItemCommandProperty = DependencyProperty.Register(
            nameof(Command), typeof(ICommand), typeof(TreeViewRemoveElementBehavior), new PropertyMetadata(default(ICommand)));

        public ICommand Command
        {
            get => (ICommand)GetValue(DeleteItemCommandProperty);
            set => SetValue(DeleteItemCommandProperty, value);
        }

        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
        }

        protected override void OnCleanup()
        {
            AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
            base.OnCleanup();
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (AssociatedObject.SelectedItem != null) //При активации вкладки бросаем событие 
                    Command?.Invoke(AssociatedObject.SelectedItem);
            }
        }
    }
}*/