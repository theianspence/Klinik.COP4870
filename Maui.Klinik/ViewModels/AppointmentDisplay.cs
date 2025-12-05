using Library.Klinik.Models;

namespace Maui.Klinik.ViewModels;

public class AppointmentDisplay
{
    public Appointment Appointment { get; set; } = null!;
    public string PatientName { get; set; } = string.Empty;
    public string PhysicianName { get; set; } = string.Empty;
    public DateTime StartTime => Appointment.AppointmentDateTime;
    public int Id => Appointment.Id;
    public string Room => Appointment.Room ?? "Not assigned";
}
