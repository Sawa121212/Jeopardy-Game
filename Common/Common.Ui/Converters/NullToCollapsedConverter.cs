//using System;
//using System.Globalization;
//using Avalonia.Data;
//using Avalonia.Data.Converters;
//using Avalonia.Markup.Xaml;

//namespace Common.Ui.Converters
//{
//    /// <summary>
//    /// Если значение равно null, свернуть.
//    /// </summary>
//    public class NullToCollapsedConverter : MarkupExtension, IValueConverter
//    {
//        public override object ProvideValue(IServiceProvider serviceProvider)
//        {
//            return this;
//        }

//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            return value != null ? Visibility.Visible : Visibility.Collapsed;
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotSupportedException();
//        }
//    }
//}
