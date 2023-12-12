using Prism.Ioc;

namespace Common.Core
{
    public static class EnumExtensions
    {
        public static void TryRegister<T>(this IContainerRegistry containerRegistry)
        {
            if (!containerRegistry.IsRegistered<T>())
            {
                containerRegistry.Register(typeof(T));
            }
        }

        public static void TryRegisterSingleton<TFrom, TTo>(this IContainerRegistry containerRegistry) where TTo : TFrom
        {
            if (!containerRegistry.IsRegistered<TFrom>())
            {
                containerRegistry.RegisterSingleton(typeof(TFrom), typeof(TTo));
            }
        }

        public static void TryRegisterForNavigation<Type, Name>(this IContainerRegistry containerRegistry)
        {
            if (!containerRegistry.IsRegistered<Type>())
            {
                containerRegistry.Register(typeof(object), typeof(Type), nameof(Name));
            }
        }
    }
}