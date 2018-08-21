using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DataBaseTool.Converters
{
    public class HiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int re = (int)value;
            if (re == 0)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(Visibility) && (Visibility)value == Visibility.Hidden || (Visibility)value == Visibility.Collapsed)
            {
                return 0;
            }
            if (value.GetType() == typeof(Visibility) && (Visibility)value == Visibility.Visible)
            {
                return 1;
            }
            return DependencyProperty.UnsetValue;
        }
    }
    public class IsNullConverter : IValueConverter
    {
        private object ConvertValue;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ConvertValue = value;
            if (value == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(bool) && (bool)value == true)
            {
                return ConvertValue;
            }
            else
            {
                return null;
            }
        }
    }
    /// <summary>
    /// 父控件长宽度 - 1
    /// </summary>
    public class ParentWHConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(double) && (double)value > 1)
            {
                return (double)value - 2;
            }
            else
            {
                return value;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(double) && (double)value > -1)
            {
                return (double)value + 2;
            }
            else
            {
                return value;
            }
        }
    }
}