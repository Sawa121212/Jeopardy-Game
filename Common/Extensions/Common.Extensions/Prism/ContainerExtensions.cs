using System;
using Prism.Ioc;

namespace Common.Extensions.Prism
{
    /// <summary>
    /// Методы расширения для работы с <see cref="IContainerProvider"/>
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Безопасная изъятие из контейнера.
        /// </summary>
        /// <typeparam name="T">Зарегистрированный интерфейс.</typeparam>
        /// <param name="container">Контейнер.</param>
        /// <returns></returns>
        public static T TryResolveEx<T>(this IContainerProvider container) where T : class
        {
            return container.IsRegistered<T>()
                ? container.Resolve<T>()
                : null;
        }

        public static void TryRegisterScoped<T>(this IContainerRegistry container) where T : class
        {
            if (!container.IsRegistered<T>())
                container.RegisterScoped<T>();
        }

        public static void TryRegister<T>(this IContainerRegistry container) where T : class
        {
            if (!container.IsRegistered<T>())
                container.Register<T>();
        }

        public static void TryRegisterSingleton<T>(this IContainerRegistry container) where T : class
        {
            if (!container.IsRegistered<T>())
                container.RegisterSingleton<T>();
        }


        public static void TryRegisterSingleton<T>(
            this IContainerRegistry container,
            Func<IContainerProvider, object> factoryMethod)
        {
            if (!container.IsRegistered<T>())
                container.RegisterSingleton(typeof(T), factoryMethod);
        }

        public static void TryRegisterSingleton<TFrom, TTo>(this IContainerRegistry container) where TTo : TFrom
        {
            if (!container.IsRegistered<TFrom>())
                container.RegisterSingleton(typeof(TFrom), typeof(TTo));
        }

        /// <summary>
        /// Безопасная изъятие из контейнера.
        /// </summary>
        /// <param name="container">Контейнер.</param>
        /// <param name="t"></param>
        /// <returns></returns>
        /*public static object TryResolveEx(this IContainerProvider container, Type T)
        {
            return container.IsRegistered<T>
                ? container.Resolve(t)
                : null;
        }*/
    }
}