/*
using System;
using System.Drawing;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Common.Ui.ScreenHelper;

namespace Common.Ui.Converters
{
    /// <summary>
    /// Преобразовывает Icon из ресурсов в ImageSource для XAML.
    /// <seealso cref="BitmapToImageSource">По аналогии.</seealso>
    /// </summary>
    [ValueConversion(typeof(Icon), typeof(ImageSource))]
    public class IconToImageSource : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            var icon = value as Icon;
            if (icon == null)
                return null;

            using (var bmp = icon.ToBitmap())
            {

                var hBitmap = bmp.GetHbitmap();
                try
                {
                    return Imaging.CreateBitmapSourceFromHBitmap(
                        hBitmap,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height));
                }
                finally
                {
                    NativeMethods.DeleteObject(hBitmap);
                }
            }
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
*/
