/*using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.Windows
{
    /// <summary>
    /// Вызывает команду при закрытии окна.
    /// </summary>
    public class ClosingToCommandBehavior : Behavior<Window>
    {
        public static readonly DependencyProperty ClosingCpmmandProperty = DependencyProperty.Register(
            nameof(ClosingCpmmand), typeof(ICommand), typeof(ClosingToCommandBehavior), new PropertyMetadata(default(ICommand)));

        public ICommand ClosingCpmmand
        {
            get => (ICommand) GetValue(ClosingCpmmandProperty);
            set => SetValue(ClosingCpmmandProperty, value);
        }
        
        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Closing += OnClosing;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        { 
            AssociatedObject.Closing -= OnClosing;
            base.OnDetaching();
        }
        
        private void OnClosing(object sender, CancelEventArgs e)
        {
            ClosingCpmmand?.Invoke();
        }
    }
}*/