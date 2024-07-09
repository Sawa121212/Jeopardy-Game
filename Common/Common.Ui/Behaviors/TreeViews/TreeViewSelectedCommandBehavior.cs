using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.TreeViews
{
    public class TreeViewSingleSelectionCommandBehavior : Behavior<TreeView>
    {
        public static readonly StyledProperty<ICommand> CommandProperty =
            AvaloniaProperty.Register<TreeViewSingleSelectionCommandBehavior, ICommand>(nameof(Command), default, true);

        public static readonly StyledProperty<object> CommandParameterProperty =
            AvaloniaProperty.Register<TreeViewSingleSelectionCommandBehavior, object>(nameof(CommandParameter), default, true);

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public ICommand Command
        {
            get => GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.SelectionChanged += OnSelectionChangedUp;
            }
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.SelectionChanged -= OnSelectionChangedUp;
            }

            base.OnDetaching();
        }

        private void OnSelectionChangedUp(object sender, SelectionChangedEventArgs e)
        {
            /*if (CommandParameter != null)
            {
                Command?.Invoke(CommandParameter);
            }
            else
            {
                if (AssociatedObject?.SelectedItem != null)
                {
                    Command?.Invoke(AssociatedObject.SelectedItem);
                }
                else
                {*/
            if (AssociatedObject?.SelectedItems != null)
            {
                Command?.Invoke(AssociatedObject.SelectedItems[0]);
            }
            /*}
        }*/
        }
    }
}