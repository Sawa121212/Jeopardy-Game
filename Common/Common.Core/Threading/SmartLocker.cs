using System.Threading;

namespace Common.Core.Threading
{
    /// <summary>
    /// Блокировка потока с заданием времени ожидания.
    /// При отсутствии конкуренции работает блокировка в пользовательском режиме
    /// в ином случае подключается режим ядра. 
    /// </summary>
    public class SmartLocker
    {
        // Количество ожидающих.
        private int _waiters;

        // Количество счастливчиков дождавшихся доступа.
        private int _luckies;

        // Блокировка режима ядра.
        private readonly AutoResetEvent _waiterLocker;

        public SmartLocker()
        {
            _waiterLocker = new AutoResetEvent(false);
        }

        /// <summary>
        /// Блокировка ресурса с таймаутом.
        /// </summary>
        /// <param name="milliseconds">Таймаут ожидания в миллисекундах.</param>
        /// <returns>True - доступ получен.</returns>
        public bool Enter(int milliseconds = 0)
        {
            // Поток хочет получить блокировку
            if (Interlocked.Increment(ref _waiters) == 1)
            {
                return true; // Блокировка свободна, конкуренции нет, возвращаем управление
            }

            // Блокировка захвачена другим потоком (конкуренция), приходится ждать.
            if (_waiterLocker.WaitOne(milliseconds)) // Значительное снижение производительности из-за режима ядра.
            {
                Interlocked.Increment(ref _luckies); // В рядах счастливчиков прибыло.
                return true; // Дождались своего ресурса.
            }

            //Так как своего ресурса не дождались, то выходим обратно и подтираем за собой.
            Interlocked.Decrement(ref _waiters);
            return false;
        }

        /// <summary>
        /// Ресурс свободен. 
        /// </summary>
        public void Leave()
        {
            bool lucky = false;
            if (Volatile.Read(ref _luckies) > 0)
            {
                lucky = true;
                Interlocked.Decrement(ref _luckies); // Счастливчик покидает здание.
            }

            if (Interlocked.Decrement(ref _waiters) > 0 || lucky)
            {
                // Есть заблокированные потоки или счастливчик выходит.
                _waiterLocker.Set(); // Значительное снижение производительности
            }
        }

        /// <summary>
        /// Ресурс используется.
        /// </summary>
        public bool IsBusy => (Volatile.Read(ref _luckies) > 0) || (Volatile.Read(ref _waiters) > 0);
    }
}