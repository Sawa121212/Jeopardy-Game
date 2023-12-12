using System;
using System.Globalization;
using System.IO;

namespace Common.Ui.Converters
{
    public class FilenameRemoveExtConverter : MarkupConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string filename)
            {
                return Path.GetFileNameWithoutExtension(filename);
            }
            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
