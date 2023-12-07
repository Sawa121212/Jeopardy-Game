using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.DataGrids
{
    public class DataGridDoubleClickBehavior : Behavior<DataGrid>
    {
        public static readonly StyledProperty<ICommand> DoubleClickCommandProperty =
            AvaloniaProperty.Register<DataGridDoubleClickBehavior, ICommand>(nameof(DoubleClickCommand), default, true);

        public static readonly StyledProperty<object> CommandParameterProperty =
            AvaloniaProperty.Register<DataGridDoubleClickBehavior, object>(nameof(CommandParameter), default, true);


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

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.DoubleTapped += DoubleClickEvent;
            }
        }


        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.DoubleTapped -= DoubleClickEvent;
            }

            base.OnDetaching();
        }

        private void DoubleClickEvent(object sender, RoutedEventArgs e)
        {
            /*if (CommandParameter is NodeElementSelector parameter)
            {
                DoubleClickCommand?.Invoke(parameter);
                return;
            }

            if (AssociatedObject != null)
            {
                var row = AssociatedObject.TryFindFromPoint<DataGrid>(e.GetPosition(AssociatedObject));
                if (row != null && row.DataContext is NodeElementSelector selector)
                {
                    DoubleClickCommand?.Invoke(selector);
                }
            }*/
        }
    }
}