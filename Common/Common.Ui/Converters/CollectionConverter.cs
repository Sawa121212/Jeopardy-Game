using System.Collections;
using Avalonia.Data.Converters;

namespace Common.Ui.Converters
{
    /// <summary>
    /// Provides a set of useful <see cref="IValueConverter"/>s for working with objects.
    /// </summary>
    public static class CollectionConverter
    {
        public static readonly IValueConverter IsAny =
            new FuncValueConverter<object?, bool>(value =>
            {
                if (value is ICollection collection)
                {
                    return collection.Count > 0;
                }

                return false;
            });

        public static readonly IValueConverter IsNotAny =
            new FuncValueConverter<object?, bool>(value =>
            {
                if (value is ICollection collection)
                {
                    return collection.Count == 0;
                }

                return false;
            });
    }
}