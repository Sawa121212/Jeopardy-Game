/*
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.TabControls
{
    public class SelectedTabBehavior : Behavior<TabControl>
    {
        public static readonly DependencyProperty SelectedCommandProperty = DependencyProperty.Register(
            "SelectedCommand", typeof(ICommand), typeof(SelectedTabBehavior), new PropertyMetadata(default(ICommand)));

        public ICommand SelectedCommand
        {
            get => (ICommand)GetValue(SelectedCommandProperty);
            set => SetValue(SelectedCommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
            base.OnDetaching();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (AssociatedObject != null)
            {
                SelectedCommand?.Invoke(AssociatedObject.SelectedItem);
            }
        }
    }
}
*/
