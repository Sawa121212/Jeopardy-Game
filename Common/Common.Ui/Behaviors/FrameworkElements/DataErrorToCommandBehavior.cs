/*using System.Windows.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.FrameworkElements
{
    public class DataErrorToCommandBehavior: Behavior<FrameworkElement>
    {
        private RoutedEventHandler _handler;

        public static readonly DependencyProperty ErrorChangedCommandProperty = DependencyProperty.Register(
            nameof(ErrorChangedCommand), typeof(ICommand), typeof(DataErrorToCommandBehavior), new PropertyMetadata(default(ICommand)));

        public ICommand ErrorChangedCommand
        {
            get => (ICommand) GetValue(ErrorChangedCommandProperty);
            set => SetValue(ErrorChangedCommandProperty, value);
        }
        
        protected override void OnAttached()
        {
            base.OnAttached();
            // Initialize the handler for the Validation Error Event
            _handler = new RoutedEventHandler(OnValidationRaised);
            // Add the handler to the event from the element which is attaching this behavior
            AssociatedObject.AddHandler(Validation.ErrorEvent, _handler);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            // Remove the event handler from the associated object
            AssociatedObject.RemoveHandler(Validation.ErrorEvent, _handler);
        }
        
        private void OnValidationRaised(object sender, RoutedEventArgs e)
        {
            if (e is ValidationErrorEventArgs args)
            {
                // You can add only Exception validation errors if you want..

                if (args.Action == ValidationErrorEventAction.Added)
                    ErrorChangedCommand?.Invoke();
                else if (args.Action == ValidationErrorEventAction.Removed)
                    ErrorChangedCommand?.Invoke();
            }
        }
    }
}*/