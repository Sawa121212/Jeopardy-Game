using System;
using Avalonia;
using Avalonia.Controls;
using Common.Core.Signals;

namespace Common.Ui.Behaviors.Windows
{
    public class CloseSignalWindowBehavior : BehaviorBase<Window>
    {
        static CloseSignalWindowBehavior() => ResultProperty.Changed.Subscribe(OnCloseWindow);

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
            if (e.Sender is not CloseSignalWindowBehavior behavior)
            {
                return;
            }

            if (e.NewValue is ISignal<bool?> signal)
            {
                signal.Subscribe(behavior.Close);
            }
        }

        /// Обратите внимание, что после закрытия окна его нельзя отобразить снова.
        /// Если вы хотите повторно показать окно, вам следует использовать Hide метод.
        /// https://docs.avaloniaui.net/ru/docs/reference/controls/window#show-hide-and-close-a-window
        private void Close(bool? obj)
        {
            AssociatedObject?.Hide();
            //AssociatedObject?.Close();
        }
    }
}