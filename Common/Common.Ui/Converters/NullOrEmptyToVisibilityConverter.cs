//using System;
//using System.Globalization;
//using Avalonia.Data;
//using Avalonia.Data.Converters;
//using Avalonia.Markup.Xaml;

//namespace Common.Ui.Converters
//{
//    public class NullOrEmptyToVisibilityConverter : MarkupExtension, IValueConverter
//    {
//        public override object ProvideValue(IServiceProvider serviceProvider)
//        {
//            return this;
//        }

//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            var s = value as string;
//            return string.IsNullOrEmpty(s) ? Visibility.Collapsed : Visibility.Visible;
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotSupportedException();
//        }
//    }
//}
