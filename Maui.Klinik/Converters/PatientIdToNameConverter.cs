using System.Globalization;
using Library.Klinik.Services;

namespace Maui.Klinik.Converters;

public class PatientIdToNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int patientId)
        {
            // This won't work directly - we need to use a different approach
            // We'll handle this in the ViewModel instead
            return $"Patient ID: {patientId}";
        }
        return "Unknown";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
