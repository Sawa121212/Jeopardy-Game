/*
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using AvaloniaUI.Ribbon;

namespace Common.Ui.Behaviors.Ribbons
{
    /// <summary>
    /// Растягивает заголовок до ширины приложения.
    /// </summary>
    public class RibbonTitleStretchBehavior : Behavior<Ribbon>
    {
        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (AssociatedObject.Title is Grid grid)
            {
                grid.Width = AssociatedObject.ActualWidth - 5 < 10 ? 10 : AssociatedObject.ActualWidth - 5;

                var quickAccessBar = AssociatedObject.FindVisualChildren<RibbonQuickAccessToolBar>().FirstOrDefault();
                if (quickAccessBar != null)
                {
                    if (quickAccessBar.ActualWidth + 5 < grid.Width)
                    {
                        grid.Width -= quickAccessBar.ActualWidth + 5;
                    }
                }
            }
        }

        protected override void OnCleanup()
        {
            AssociatedObject.SizeChanged -= OnSizeChanged;
            base.OnCleanup();
        }
    }
}
*/
