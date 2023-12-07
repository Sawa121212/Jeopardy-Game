using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.ListViews
{
    /// <summary>
    /// Обеспечивает доступ к выделенным элементам ListView
    /// </summary>
    public class ListBoxSelectedItemsBehavior : Behavior<ListBox>
    {
        public static readonly StyledProperty<ICommand> CommandProperty =
            AvaloniaProperty.Register<ListBoxSelectedItemsBehavior, ICommand>(nameof(Command), default, true);

        public static readonly StyledProperty<object> CommandParameterProperty =
            AvaloniaProperty.Register<ListBoxSelectedItemsBehavior, object>(nameof(CommandParameter), default, true);

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
                AssociatedObject.SelectionChanged += OnSelectionChanged;
            }
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.SelectionChanged -= OnSelectionChanged;
            }

            base.OnDetaching();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (CommandParameter != null)
            {
                Command?.Invoke(CommandParameter);
            }
            else */
            Command?.Execute(AssociatedObject?.SelectedItem);
        }
    }
}