using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.DataGrids
{
    public class DataGridSelectedCommandBehavior : Behavior<DataGrid>
    {
        public static readonly StyledProperty<ICommand> CommandProperty =
            AvaloniaProperty.Register<DataGridSelectedCommandBehavior, ICommand>(nameof(Command), default, true);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.SelectionChanged += OnSelectionChanged;
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AssociatedObject != null) 
            {
                Command?.Execute(AssociatedObject.SelectedItem);
            }
            /*var selectedItem = AssociatedObject.SelectedItem;
                if (selectedItem != null)
                {
                    Command?.Invoke(AssociatedObject.SelectedItem);
                }*/
        }
    }
}