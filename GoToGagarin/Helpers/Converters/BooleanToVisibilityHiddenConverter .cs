using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace GoToGagarin.Helpers.Converters;

public class BooleanToVisibilityHiddenConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var isVisible = value is bool boolValue && boolValue;
        return isVisible ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is Visibility visibility && visibility == Visibility.Visible;
    }
}