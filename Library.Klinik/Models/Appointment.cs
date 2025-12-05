namespace Library.Klinik.Models;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int PhysicianId { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"ID: {Id}, Patient ID: {PatientId}, Physician ID: {PhysicianId}, " +
               $"DateTime: {AppointmentDateTime:MM/dd/yyyy HH:mm}, Room: {Room}, Reason: {Reason}";
    }
}
