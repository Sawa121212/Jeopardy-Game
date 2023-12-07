/*
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Behaviors.Windows;
using ReactiveUI;

namespace Common.Ui.Behaviors.Buttons
{
    /// <summary>
    /// Закрытие приложения с отключением запроса закрытия.
    /// </summary>
    public class CloseAppBehavior : Behavior<Button>
    {
        /// <summary>
        /// Включение/отключение закрытия главного окна приложения
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
            "IsEnabled", typeof(bool), typeof(CloseAppBehavior), new PropertyMetadata(default(bool)));

        public bool IsEnabled
        {
            get => (bool) GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.Click += OnButtonClick;
        }

        protected override void OnCleanup()
        {
            AssociatedObject.Click -= OnButtonClick;
            base.OnCleanup();
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (IsEnabled)
            {
                var shell = Application.Current.MainWindow;
                if (shell != null)
                {
                    RemoveCloseWindowBehavior(shell);
                }
                Application.Current.Shutdown();
            }
        }

        public void RemoveCloseWindowBehavior<TWindow>(TWindow window) where TWindow : Window
        {
            var behaviors = Interaction<,>.GetBehaviors(window);
            var closeBehavior = behaviors.OfType<CloseWindowBehavior>().FirstOrDefault();
            if (closeBehavior != null)
                Interaction.GetBehaviors(window).Remove(closeBehavior);
        }
    }
}
*/
