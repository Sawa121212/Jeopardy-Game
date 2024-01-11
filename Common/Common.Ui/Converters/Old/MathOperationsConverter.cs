using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Common.Ui.Converters.Old
{
    public class MathOperationsConverter : IMultiValueConverter
    {
        //Внимание! Выполняется последовательно игнорируя правила математики
        /*
            <MultiBinding Converter="{StaticResource MathOperationConverter1}">
                <Binding Path="Width"/>
                <Binding >
                    <Binding.Source>
                        <sys:String>-</sys:String>
                    </Binding.Source>
                </Binding>
                <Binding Path="ActualWidth" ElementName="LNodeControl"/>
                <Binding  >
                    <Binding.Source>
                        <sys:String>-</sys:String>
                    </Binding.Source>
                </Binding>
                <Binding  >
                    <Binding.Source>
                        <sys:Double>5</sys:Double>
                    </Binding.Source>
                </Binding>
            </MultiBinding>
        */
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                if (values.Length > 2 && values.Length % 2 == 1)
                {
                    double a = 0.0;

                    {
                        if (values[0] is double val1)
                        {
                            a = val1;
                        }
                    }
                    {
                        if (values[0] is int val1)
                        {
                            a = val1;
                        }
                    }
                    for (int i = 1; i < values.Length; i += 2)
                    {
                        double b = 0.0;
                        {
                            if (values[i + 1] is double val1)
                            {
                                b = val1;
                            }
                        }
                        {
                            if (values[i + 1] is int val1)
                            {
                                b = val1;
                            }
                        }
                        if (values[i] is string chard)
                        {
                            switch (chard[0])
                            {
                                case '+':
                                    a = a + b;
                                    break;
                                case '-':
                                    a = a - b;
                                    break;
                                case '*':
                                    a = a * b;
                                    break;
                                case '/':
                                    a = a / b;
                                    break;
                            }
                        }
                    }
                    return a;
                }
                return null;
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                return null;
            }

            public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
    }
    }