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
        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.Loaded += OnLoad;
            AssociatedObject.GotFocus += OnGotFocus;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(AssociatedObject);
        }

        /// <inheritdoc />
        protected override void OnCleanup()
        {
            AssociatedObject.Loaded -= OnLoad;
            AssociatedObject.GotFocus -= OnGotFocus;
            base.OnCleanup();
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