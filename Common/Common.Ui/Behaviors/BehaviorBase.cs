
using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

//Взято отсюда:
//http://www.pixytech.com/rajnish/2014/03/attached-behaviors-memory-leaks/
//https://gist.github.com/Dalstroem/43f91e371cb4a92156623e3848c4020e

namespace Common.Ui.Behaviors
{
    /// <summary>
    /// Помогает избежать утечки памяти из-за привязки к событиям. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BehaviorBase<T> : Behavior<T> where T : Control
    {
        private bool _isSetup;
        private bool _isHookedUp;
        private WeakReference _weakTarget;

        /// <summary>
        /// Выполнить привязку к событиям тут.. Базовый вызвать раньше остальных
        /// </summary>
        protected virtual void OnSetup() { }

        /// <summary>
        /// Выполнить отвязку от событий тут. Базовый вызвать позже остальных
        /// </summary>
        protected virtual void OnCleanup() { }

        

        /// <summary>
        /// Is invoked when the target has been loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTargetLoaded(object sender, RoutedEventArgs e) { SetupBehavior(); }

        /// <summary>
        /// Is invoked when the target has been unloaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTargetUnloaded(object sender, RoutedEventArgs e) { CleanupBehavior(); }

       
        /// <summary>
        /// Calls the setup method.
        /// </summary>
        private void SetupBehavior()
        {
            if (_isSetup) return;
            _isSetup = true;
            OnSetup();
        }

        /// <summary>
        /// Calls the cleanup method.
        /// </summary>
        private void CleanupBehavior()
        {
            if (!_isSetup) return;
            _isSetup = false;
            OnCleanup();
        }
    }
}

