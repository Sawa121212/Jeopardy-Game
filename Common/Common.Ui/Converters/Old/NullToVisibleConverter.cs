//using System;
//using System.Globalization;
//using Avalonia.Data;
//using Avalonia.Data.Converters;
//using Avalonia.Markup.Xaml;

//namespace Common.Ui.Converters
//{
//    public class NullToVisibleConverter : MarkupExtension, IValueConverter
//    {
//        public override object ProvideValue(IServiceProvider serviceProvider)
//        {
//            return this;
//        }

//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            return value != null ? Visibility.Collapsed : Visibility.Visible;

//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotSupportedException();
//        }
//    }
//}
