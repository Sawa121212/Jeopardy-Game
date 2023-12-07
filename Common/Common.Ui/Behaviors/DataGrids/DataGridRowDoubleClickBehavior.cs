using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.DataGrids
{
    /// <summary>
    /// Вызывает DoubleClickRowCommand при двойном клике по DataGridRow.
    /// </summary>
    public class DataGridRowDoubleClickBehavior : Behavior<DataGrid>
    {
        public static readonly StyledProperty<ICommand> DoubleClickRowCommandProperty =
            AvaloniaProperty.Register<DataGridRowDoubleClickBehavior, ICommand>(nameof(DoubleClickRowCommand), default, true);

        public static readonly StyledProperty<object> CommandParameterProperty =
            AvaloniaProperty.Register<DataGridRowDoubleClickBehavior, object>(nameof(CommandParameter), default, true);

        public object CommandParameter
        {
            get => (object)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public ICommand DoubleClickRowCommand
        {
            get => (ICommand)GetValue(DoubleClickRowCommandProperty);
            set => SetValue(DoubleClickRowCommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.DoubleTapped -= DoubleClickEvent;
            }
        }

        protected override void OnDetaching()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.DoubleTapped -= DoubleClickEvent;
            }

            base.OnDetaching();
        }


        private void DoubleClickEvent(object sender, RoutedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                /*var row = dataGrid.TryFindFromPoint<DataGridRow>(e.GetPosition(dataGrid));
                if (row != null)
                {
                    DoubleClickRowCommand?.Invoke(CommandParameter ?? row.DataContext);
                }*/
            }
        }
    }
}