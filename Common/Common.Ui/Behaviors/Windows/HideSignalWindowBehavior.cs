using System;
using Avalonia;
using Avalonia.Controls;
using Common.Core.Signals;

namespace Common.Ui.Behaviors.Windows
{
    public class HideSignalWindowBehavior : BehaviorBase<Window>
    {
        static HideSignalWindowBehavior() => ResultProperty.Changed.Subscribe(OnCloseWindow);

        public static readonly StyledProperty<ISignal<bool?>> ResultProperty =
            AvaloniaProperty.Register<CloseSignalWindowBehavior, ISignal<bool?>>(nameof(Result));
        
        /// <summary>
        /// Сигнал на закрытие окна.  
        /// </summary>
        public ISignal<bool?> Result
        {
            get => GetValue(ResultProperty);
            set => SetValue(ResultProperty, value);
        }

        private static void OnCloseWindow(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is HideSignalWindowBehavior behavior)
            {
                if (e.NewValue is ISignal<bool?> signal)
                {
                    signal.Subscribe(behavior.Hide);
                }
            }
        }

        private void Hide(bool? obj)
        {
            AssociatedObject?.Hide();
            //AssociatedObject?.Close();
        }
    }
}
