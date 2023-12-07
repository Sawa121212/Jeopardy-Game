using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.DatePickers
{
    public class DatePickersDateChangedBehavior : Behavior<DatePicker>
    {
        public static readonly StyledProperty<ICommand> CommandProperty =
            AvaloniaProperty.Register<DatePickersDateChangedBehavior, ICommand>(nameof(Command), default, true);

        public static readonly StyledProperty<object> CommandParameterProperty =
            AvaloniaProperty.Register<DatePickersDateChangedBehavior, object>(nameof(CommandParameter), default, true);

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
                AssociatedObject.SelectedDateChanged += OnSelectedDateChanged;
            }
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.SelectedDateChanged -= OnSelectedDateChanged;
            }

            base.OnDetaching();
        }

        private void OnSelectedDateChanged(object sender, DatePickerSelectedValueChangedEventArgs datePickerSelectedValueChangedEventArgs)
        {
            Command?.Execute(CommandParameter ?? null);
        }
    }
}