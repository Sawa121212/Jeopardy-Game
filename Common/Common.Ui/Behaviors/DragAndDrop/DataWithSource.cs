using System;
using Avalonia.Controls;

namespace Common.Ui.Behaviors.DragAndDrop
{
    /// <summary>
    /// Вспомогательный класс для перетаскивания элементов.
    /// </summary>
    public class DataWithSource
    {
        /// <summary>
        /// Вспомогательный класс для перетаскивания элементов.
        /// </summary>
        public DataWithSource(Control dragSource, object data)
        {
            DragSource = new WeakReference<Control>(dragSource);
            Data = new WeakReference<object>(data);
        }

        /// <summary>
        /// Слабая ссылка на источник.
        /// </summary>
        public WeakReference<Control> DragSource { get; private set; }

        /// <summary>
        /// Слабая ссылка на перемещаемые данные.
        /// </summary>
        public WeakReference<object> Data { get; private set; }

        /// <summary>
        /// Освобождение ссылок.
        /// </summary>
        public void Clear()
        {
            DragSource?.SetTarget(null);
            DragSource = null;

            Data?.SetTarget(null);
            Data = null;
        }
    }
}
