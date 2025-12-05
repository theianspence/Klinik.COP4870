using System.Globalization;

namespace Maui.Klinik.Converters;

public class BoolToSortTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isAscending)
        {
            return isAscending ? "↑ Asc" : "↓ Desc";
        }
        return "Sort";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
