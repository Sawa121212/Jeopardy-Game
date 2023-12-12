/*
using System;
using System.Windows.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.FrameworkElements
{
    public class NotifyLoadBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty LoadCommandProperty = DependencyProperty.Register(
            nameof(LoadCommand), typeof(ICommand), typeof(NotifyLoadBehavior), new PropertyMetadata(default(ICommand)));

        public ICommand LoadCommand
        {
            get => (ICommand)GetValue(LoadCommandProperty);
            set => SetValue(LoadCommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoaded;
            base.OnDetaching();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var source = PresentationSource.FromVisual(AssociatedObject) as HwndSource;
            if(source != null)
                LoadCommand?.Invoke(source.Handle);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            LoadCommand?.Invoke(IntPtr.Zero);
        }
    }
}
*/
