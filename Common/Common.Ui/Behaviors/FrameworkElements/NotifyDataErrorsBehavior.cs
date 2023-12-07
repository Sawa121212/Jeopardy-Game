/*
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.FrameworkElements
{
    /// <summary>
    /// Пробрасывает флаг ошибки с View на ViewModel. 
    /// </summary>
    public class NotifyDataErrorsBehavior : Behavior<FrameworkElement>
    {
        private RoutedEventHandler _handler;

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
                if (AssociatedObject.DataContext is ObservableViewModelEx viewModel)
                {
                    // You can add only Exception validation errors if you want..

                    if (args.Action == ValidationErrorEventAction.Added)
                        viewModel.IsHasErrors = true;
                    else if (args.Action == ValidationErrorEventAction.Removed)
                        viewModel.IsHasErrors = false;
                }
        }
    }
}
*/
