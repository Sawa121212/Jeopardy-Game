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

        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnCleanup()
        {
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
            base.OnCleanup();
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
