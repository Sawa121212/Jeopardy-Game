/*
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Avalonia.Data;

namespace Common.Ui.Converters
{
    public class ElementToSequenceConverter : MarkupConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is NodeElement element)
                return new ObservableCollection<NodeElement>(element.ToSequence());
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
*/
