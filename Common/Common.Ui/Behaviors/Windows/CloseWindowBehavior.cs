/*
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.Windows
{
    /// <summary>
    /// Обеспечивает закрытие окна с подтверждением.
    /// </summary>
    public class CloseWindowBehavior : Behavior<Window>
    {
        private static string DEFAULT_QUERY_CAPTION = "Подтверждение";
        private static string DEFAULT_QUERY_TEXT = "Вы действительно хотите выполнить указанную операцию?";
        private static MessageBoxButton DEFAULT_BUTTONS = MessageBoxButton.OKCancel;
        private static MessageBoxImage DEFAULT_IMAGE = MessageBoxImage.Question;
        private static MessageBoxResult DEFAULT_EXPECTED_RESULT = MessageBoxResult.OK;

#pragma warning disable 1591
        public static readonly DependencyProperty DialogTextProperty =
            DependencyProperty.Register(nameof(DialogText), typeof(string), typeof(CloseWindowBehavior),
                new PropertyMetadata(DEFAULT_QUERY_TEXT));

        public static readonly DependencyProperty DialogCaptionProperty =
            DependencyProperty.Register(nameof(DialogCaption), typeof(string), typeof(CloseWindowBehavior),
                new PropertyMetadata(DEFAULT_QUERY_CAPTION));

        public static readonly DependencyProperty DialogButtonsProperty =
            DependencyProperty.Register(nameof(DialogButtons), typeof(MessageBoxButton), typeof(CloseWindowBehavior),
                new PropertyMetadata(DEFAULT_BUTTONS));

        public static readonly DependencyProperty DialogImageProperty =
            DependencyProperty.Register(nameof(DialogImage), typeof(MessageBoxImage), typeof(CloseWindowBehavior),
                new PropertyMetadata(DEFAULT_IMAGE));

        public static readonly DependencyProperty ExpectedDialogResultProperty =
            DependencyProperty.Register(nameof(ExpectedDialogResult), typeof(MessageBoxResult), typeof(CloseWindowBehavior),
                new PropertyMetadata(DEFAULT_EXPECTED_RESULT));

        public static readonly DependencyProperty IsHasChangesProperty = DependencyProperty.Register(
            nameof(IsHasChanges), typeof(bool), typeof(CloseWindowBehavior), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            nameof(IsBusy), typeof(bool), typeof(CloseWindowBehavior), new PropertyMetadata(default(bool)));


        public static readonly DependencyProperty HasChangesDialogTextProperty = DependencyProperty.Register(
            nameof(HasChangesDialogText), typeof(string), typeof(CloseWindowBehavior), new PropertyMetadata(DEFAULT_QUERY_TEXT));
        
#pragma warning restore 1591

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Closing += OnClosing;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Closing -= OnClosing;
        }

        /// <summary>
        /// Заголовок диалогового окна подтверждения.
        /// </summary>
        public string DialogCaption
        {
            get => (string)GetValue(DialogCaptionProperty);
            set => SetValue(DialogCaptionProperty, value);
        }

        /// <summary>
        /// Текст диалогового окна подтверждения.
        /// </summary>
        public string DialogText
        {
            get => (string)GetValue(DialogTextProperty);
            set => SetValue(DialogTextProperty, value);
        }

        /// <summary>
        /// Кнопки диалогового окна подтверждения.
        /// </summary>
        public MessageBoxButton DialogButtons
        {
            get => (MessageBoxButton)GetValue(DialogButtonsProperty);
            set => SetValue(DialogButtonsProperty, value);
        }

        /// <summary>
        /// Иконка диалогового окна подтверждения.
        /// </summary>
        public MessageBoxImage DialogImage
        {
            get => (MessageBoxImage)GetValue(DialogImageProperty);
            set => SetValue(DialogImageProperty, value);
        }

        /// <summary>
        /// Ожидаемый код результата для выполнения действия.
        /// </summary>
        public MessageBoxResult ExpectedDialogResult
        {
            get => (MessageBoxResult)GetValue(ExpectedDialogResultProperty);
            set => SetValue(ExpectedDialogResultProperty, value);
        }
        

        public bool IsHasChanges
        {
            get => (bool)GetValue(IsHasChangesProperty);
            set => SetValue(IsHasChangesProperty, value);
        }
        
        public bool IsBusy
        {
            get => (bool) GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }
        
        public string HasChangesDialogText
        {
            get => (string)GetValue(HasChangesDialogTextProperty);
            set => SetValue(HasChangesDialogTextProperty, value);
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsBusy)
            {
                e.Cancel = true;
                return;
            }

            if (AssociatedObject != null)
            {
                if (!AssociatedObject.IsLoaded) 
                {
                    return;
                }
            }

            if (IsHasChanges)
                e.Cancel = ExpectedDialogResult != MessageBox.Show(HasChangesDialogText, DialogCaption, DialogButtons, DialogImage);
            else
                e.Cancel = ExpectedDialogResult != MessageBox.Show(DialogText, DialogCaption, DialogButtons, DialogImage);

        }
    }
}
*/
