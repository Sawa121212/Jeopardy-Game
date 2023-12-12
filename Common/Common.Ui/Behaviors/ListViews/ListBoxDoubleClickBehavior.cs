/*using System.Windows.Input;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.ListViews
{
    /// <summary>
    /// Двойной клик по элементу превращается в команду.
    /// </summary>
    public class ListViewDoubleClickBehavior : Behavior<ListView>
    {
        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.Register(
            nameof(DoubleClickCommand), typeof(ICommand), typeof(ListViewDoubleClickBehavior), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter), typeof(object), typeof(ListViewDoubleClickBehavior), new PropertyMetadata(default(object)));

        public object CommandParameter
        {
            get => (object)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public ICommand DoubleClickCommand
        {
            get => (ICommand)GetValue(DoubleClickCommandProperty);
            set => SetValue(DoubleClickCommandProperty, value);
        }

        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseDoubleClick += OnPreviewMouseDoubleClick;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseDoubleClick -= OnPreviewMouseDoubleClick;
            base.OnDetaching();
        }

        private void OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = AssociatedObject?.TryFindFromPoint<ListViewItem>(e.GetPosition(AssociatedObject));
            if (item?.DataContext != null)
            {
                DoubleClickCommand?.Invoke(CommandParameter ?? item.DataContext);
            }
        }
    }
}*/

using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.ListViews
{
    /// <summary>
    /// Двойной клик по элементу превращается в команду
    /// </summary>
    public class ListBoxDoubleClickBehavior : Behavior<ListBox>
    {
        public static readonly StyledProperty<ICommand> DoubleClickCommandProperty =
            AvaloniaProperty.Register<ListBoxDoubleClickBehavior, ICommand>(nameof(DoubleClickCommand), default, true);

        public static readonly StyledProperty<object> CommandParameterProperty =
            AvaloniaProperty.Register<ListBoxDoubleClickBehavior, object>(nameof(CommandParameter), default, true);

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public ICommand DoubleClickCommand
        {
            get => GetValue(DoubleClickCommandProperty);
            set => SetValue(DoubleClickCommandProperty, value);
        }

        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.DoubleTapped += OnDoubleTapped;
            }
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.DoubleTapped -= OnDoubleTapped;
            }

            base.OnDetaching();
        }

        private void OnDoubleTapped(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject?.SelectedItem != null)
            {
                DoubleClickCommand?.Invoke(AssociatedObject.SelectedItem);
            }
            else
            {
                DoubleClickCommand?.Invoke();
            }
        }
    }
}