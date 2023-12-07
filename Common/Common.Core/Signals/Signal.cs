using System;

namespace Common.Core.Signals
{
    public class Signal<T> : SignalBase, ISignal<T>
    {
        private Action<T> _action;
        public void Clear()
        {
            _action = null;
        }

        public void Subscribe(Action<T> action)
        {
            _action = action;
        }

        public void Raise(T param)
        {
            if (_action != null)
                _action.Invoke(param);
        }
    }
}