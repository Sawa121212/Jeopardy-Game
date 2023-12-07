/*using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.Buttons
{
    public class ApplyButtonBehavior : Behavior<Button>
    {
        private int _cnt;
        protected override void OnSetup()
        {
            base.OnSetup();
            _cnt = 0;
            AssociatedObject.Click += OnButtonClick;
        }

        protected override void OnCleanup()
        {
            AssociatedObject.Click -= OnButtonClick;
            base.OnCleanup();
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var winParent = AssociatedObject.TryFindParent<Window>();
            if (winParent != null)
            {
               if(_cnt >= 3)
                   CloseWindow(winParent);
            }

            _cnt++;
        }

        private void CloseWindow(Window winParent)
        {
            // Walk up the visual tree and find the window this button is owned by.
            try
            {
                winParent.DialogResult = false;
            }
            catch (InvalidOperationException)
            {
                // Not a dialog window-- just close it directly.
                winParent.Close();
            }
        }
    }
}*/