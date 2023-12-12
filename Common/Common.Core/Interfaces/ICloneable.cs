using System;

namespace Common.Core.Interfaces
{
    public interface ICloneable<out T> : ICloneable where T : ICloneable<T>
    {
        /// <summary>
        /// Клонирование сущности.
        /// </summary>
        /// <returns>Копия сущности.</returns>
        new T Clone();
    }
}