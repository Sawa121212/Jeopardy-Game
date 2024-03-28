using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.TextBoxes
{
    public class TextBoxTextChangedBehavior : Behavior<TextBox>
    {
        public static readonly StyledProperty<ICommand> CommandProperty =
            AvaloniaProperty.Register<TextBoxTextChangedBehavior, ICommand>(
                nameof(Command), default, true);

        public static readonly StyledProperty<object> CommandParameterProperty =
            AvaloniaProperty.Register<TextBoxTextChangedBehavior, object>(
                nameof(CommandParameter), default, true);

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
                AssociatedObject.TextChanged += OnTextChanged;
            }
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.TextChanged -= OnTextChanged;
            }

            base.OnDetaching();
        }

        private void OnTextChanged(object? sender, TextChangedEventArgs e)
        {
            Command?.Execute(CommandParameter);
        }

        /*private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Command?.Execute(CommandParameter ?? null);
        }*/
    }
}