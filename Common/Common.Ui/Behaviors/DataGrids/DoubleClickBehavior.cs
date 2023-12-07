/*using System.Windows.Input;
using Avalonia.Styling;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.DataGrids
{
    public class DoubleClickBehavior : Behavior<Selector>
    {
        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.Register(
            nameof(DoubleClickCommand), typeof(ICommand), typeof(DoubleClickBehavior), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter), typeof(object), typeof(DoubleClickBehavior), new PropertyMetadata(default(object)));

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

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseDoubleClick += DoubleClickEvent;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseDoubleClick -= DoubleClickEvent;
            base.OnDetaching();
        }

        private void DoubleClickEvent(object sender, MouseButtonEventArgs e)
        {
            if (CommandParameter != null)
            {
                DoubleClickCommand?.Invoke(CommandParameter);
                return;
            }
            if (AssociatedObject != null)
            {
                var row = AssociatedObject.TryFindFromPoint<Selector>(e.GetPosition(AssociatedObject));
                if (row != null && row.DataContext != null)
                {
                    DoubleClickCommand?.Invoke(row.DataContext);
                }
            }
        }
    }
    /*public class DoubleClickListViewBehavior : Behavior<ListView>
    {
        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.Register(
            nameof(DoubleClickCommand), typeof(ICommand), typeof(DoubleClickListViewBehavior), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter), typeof(object), typeof(DoubleClickListViewBehavior), new PropertyMetadata(default(object)));

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

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseDoubleClick += DoubleClickEvent;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseDoubleClick -= DoubleClickEvent;
            base.OnDetaching();
        }

        private void DoubleClickEvent(object sender, MouseButtonEventArgs e)
        {
            if (CommandParameter != null)
            {
                DoubleClickCommand?.Invoke(CommandParameter);
                return;
            }
            if (AssociatedObject != null)
            {
                var row = AssociatedObject.TryFindFromPoint<DataGrid>(e.GetPosition(AssociatedObject));
                if (row != null && row.DataContext != null)
                {
                    DoubleClickCommand?.Invoke(row.DataContext);
                }
            }
        }
    }#1#
}*/