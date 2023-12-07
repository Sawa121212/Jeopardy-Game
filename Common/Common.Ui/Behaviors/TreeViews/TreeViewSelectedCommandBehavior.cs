/*using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.TreeViews
{
    public class TreeViewSelectedCommandBehavior : Behavior<TreeView>
    {
        private object _pressedElement;

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command), typeof(ICommand), typeof(TreeViewSelectedCommandBehavior), new PropertyMetadata(default(ICommand)));

        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.PreviewMouseDown += OnMouseDown;
            AssociatedObject.PreviewMouseUp += OnMouseUp;
        }
        
        protected override void OnCleanup()
        {
            AssociatedObject.PreviewMouseDown -= OnMouseDown;
            AssociatedObject.PreviewMouseUp -= OnMouseUp;
            base.OnCleanup();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource != null)
            {
                if (e.OriginalSource is FrameworkElement frameworkElement)
                {
                    _pressedElement = frameworkElement.DataContext;
                }
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource != null)
            {
                if (e.OriginalSource is FrameworkElement frameworkElement)
                {
                    if (frameworkElement.DataContext != null)
                    {
                        if (frameworkElement.DataContext.Equals(_pressedElement))
                        {
                            Command?.Invoke(frameworkElement.DataContext);
                        }
                    }
                }
            }
        }
    }
}*/