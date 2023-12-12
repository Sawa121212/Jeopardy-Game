/*using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.TextBoxs
{
    /// <summary>
    /// Вызывает выделение всего текста при активации.
    /// </summary>
    public class SelectAllTextBoxBehavior : Behavior<TextBox>
    {
        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoad;
            AssociatedObject.GotFocus += OnGotFocus;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(AssociatedObject);
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= OnLoad;
            AssociatedObject.GotFocus -= OnGotFocus;
            base.OnDetaching();
        }
        
        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            TaskHelper.InvokeDispatcher(() =>
            {
                AssociatedObject.SelectAll();
            });
        }
    }
}*/