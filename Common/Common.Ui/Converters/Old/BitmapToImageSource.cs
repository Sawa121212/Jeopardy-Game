/*
using System;
using System.Drawing;
using System.Drawing.Imaging;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace Common.Ui.Converters
{
    /// <summary>
    /// Преобразовывает Bitmap в ImageSource для XAML.
    /// <see>http://stackoverflow.com/questions/3427034/using-xaml-to-bind-to-a-system-drawing-image-into-a-system-windows-image-control</see>
    /// <see>http://stackoverflow.com/questions/94456/load-a-wpf-bitmapimage-from-a-system-drawing-bitmap</see>
    /// <see>http://stackoverflow.com/questions/30727343/fast-converting-bitmap-to-bitmapsource-wpf"</see>
    /// </summary>
    [ValueConversion(typeof(Bitmap), typeof(ImageSource))]
    public class BitmapToImageSource : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            var bmp = value as Bitmap;
            if (bmp == null)
                return null;

            var bitmapData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, 
                bmp.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height, 96, 96, PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bmp.UnlockBits(bitmapData);
            return bitmapSource;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
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
