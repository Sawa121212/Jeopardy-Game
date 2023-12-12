/*
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.ListViews
{
    /// <summary>
    /// Растягивает указанную колонку на всю оставшуюся ширину
    /// </summary>
    public class ListViewResizeColumnBehavior : Behavior<ListView>
    {
        public static readonly DependencyProperty StretchColumnIndexProperty = DependencyProperty.Register(
            "StretchColumnIndex", typeof(int), typeof(ListViewResizeColumnBehavior), new PropertyMetadata(default(int)));

        public int StretchColumnIndex
        {
            get => (int)GetValue(StretchColumnIndexProperty);
            set => SetValue(StretchColumnIndexProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SizeChanged += OnSizeChanged;
            AssociatedObject.Loaded += OnLoaded;
        }
        
        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= OnSizeChanged;
            AssociatedObject.Loaded -= OnLoaded;
            base.OnDetaching();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (sizeChangedEventArgs.WidthChanged)
            {
                UpdateSize();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            UpdateSize();
        }

        private void UpdateSize()
        {
            if (AssociatedObject.View is GridView view)
            {
                if (AssociatedObject.ActualWidth > 20)
                {
                    view.Columns[StretchColumnIndex].Width = AssociatedObject.ActualWidth - 20;
                    for (var i = 0; i < view.Columns.Count; i++)
                    {
                        if (i == StretchColumnIndex) continue;
                        if (view.Columns[StretchColumnIndex].Width - view.Columns[i].ActualWidth > 20)
                            view.Columns[StretchColumnIndex].Width -= view.Columns[i].ActualWidth;
                    }
                }
            }
        }
    }
}
*/
