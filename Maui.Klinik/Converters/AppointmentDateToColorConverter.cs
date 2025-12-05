using System.Globalization;

namespace Maui.Klinik.Converters;

public class AppointmentDateToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime appointmentDate)
        {
            // Appointments that are today get a light green background
            if (appointmentDate.Date == DateTime.Today)
            {
                return Colors.LightGreen;
            }
        }
        return Colors.White;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
