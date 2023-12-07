/*using System.Windows.Input;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.ListViews
{
    /// <summary>
    /// Двойной клик по элементу превращается в команду.
    /// </summary>
    public class ListViewDoubleClickBehavior : Behavior<ListView>
    {
        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.Register(
            nameof(DoubleClickCommand), typeof(ICommand), typeof(ListViewDoubleClickBehavior), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter), typeof(object), typeof(ListViewDoubleClickBehavior), new PropertyMetadata(default(object)));

        public object CommandParameter
        {
            get => (object)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public ICommand DoubleClickCommand
        {
            get => (ICommand)GetValue(DoubleClickCommandProperty);
            set => SetValue(DoubleClickCommandProperty, value);
        }

        /// <inheritdoc />
        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.PreviewMouseDoubleClick += OnPreviewMouseDoubleClick;
        }

        /// <inheritdoc />
        protected override void OnCleanup()
        {
            AssociatedObject.PreviewMouseDoubleClick -= OnPreviewMouseDoubleClick;
            base.OnCleanup();
        }

        private void OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = AssociatedObject?.TryFindFromPoint<ListViewItem>(e.GetPosition(AssociatedObject));
            if (item?.DataContext != null)
            {
                DoubleClickCommand?.Invoke(CommandParameter ?? item.DataContext);
            }
        }
    }
}*/