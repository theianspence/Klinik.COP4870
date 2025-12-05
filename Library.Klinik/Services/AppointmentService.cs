namespace Library.Klinik.Services;

using Library.Klinik.Models;

public class AppointmentService
{
    private List<Appointment> appointments = new();
    private int nextAppointmentId = 1;

    // Valid appointment hours: 8am to 5pm, Monday through Friday
    private const int APPOINTMENT_START_HOUR = 8;
    private const int APPOINTMENT_END_HOUR = 17; // 5pm in 24-hour format

    public Appointment? CreateAppointment(int patientId, int physicianId, DateTime appointmentDateTime, string reason)
    {
        return CreateAppointment(patientId, physicianId, appointmentDateTime, reason, string.Empty);
    }

    public Appointment? CreateAppointment(int patientId, int physicianId, DateTime appointmentDateTime, string reason, string room)
    {
        // Validate appointment time constraints
        if (!IsValidAppointmentTime(appointmentDateTime))
        {
            throw new ArgumentException("Appointment must be scheduled between 8am-5pm on Monday-Friday.");
        }

        // Check for physician double-booking
        if (IsPhysicianBooked(physicianId, appointmentDateTime))
        {
            throw new InvalidOperationException("Physician is already booked at this time.");
        }

        // Check for room double-booking (if room is specified)
        if (!string.IsNullOrWhiteSpace(room) && IsRoomBooked(room, appointmentDateTime))
        {
            throw new InvalidOperationException($"Room {room} is already booked at this time.");
        }

        var appointment = new Appointment
        {
            Id = nextAppointmentId++,
            PatientId = patientId,
            PhysicianId = physicianId,
            AppointmentDateTime = appointmentDateTime,
            Reason = reason,
            Room = room
        };
        appointments.Add(appointment);
        return appointment;
    }

    public Appointment? GetAppointmentById(int id)
    {
        return appointments.FirstOrDefault(a => a.Id == id);
    }

    public List<Appointment> GetAllAppointments()
    {
        return new List<Appointment>(appointments);
    }

    public bool UpdateAppointment(int id, int patientId, int physicianId, DateTime appointmentDateTime, string reason)
    {
        return UpdateAppointment(id, patientId, physicianId, appointmentDateTime, reason, string.Empty);
    }

    public bool UpdateAppointment(int id, int patientId, int physicianId, DateTime appointmentDateTime, string reason, string room)
    {
        var appointment = GetAppointmentById(id);
        if (appointment == null) return false;

        // Validate appointment time constraints
        if (!IsValidAppointmentTime(appointmentDateTime))
        {
            throw new ArgumentException("Appointment must be scheduled between 8am-5pm on Monday-Friday.");
        }

        // Check for physician double-booking (excluding the current appointment)
        if (IsPhysicianBooked(physicianId, appointmentDateTime, id))
        {
            throw new InvalidOperationException("Physician is already booked at this time.");
        }

        // Check for room double-booking (excluding the current appointment, if room is specified)
        if (!string.IsNullOrWhiteSpace(room) && IsRoomBooked(room, appointmentDateTime, id))
        {
            throw new InvalidOperationException($"Room {room} is already booked at this time.");
        }

        appointment.PatientId = patientId;
        appointment.PhysicianId = physicianId;
        appointment.AppointmentDateTime = appointmentDateTime;
        appointment.Reason = reason;
        appointment.Room = room;
        return true;
    }

    public bool DeleteAppointment(int id)
    {
        var appointment = GetAppointmentById(id);
        if (appointment == null) return false;
        appointments.Remove(appointment);
        return true;
    }

    public List<Appointment> GetAppointmentsForPhysician(int physicianId)
    {
        return appointments.Where(a => a.PhysicianId == physicianId).ToList();
    }

    public List<Appointment> GetAppointmentsForPatient(int patientId)
    {
        return appointments.Where(a => a.PatientId == patientId).ToList();
    }

    private bool IsValidAppointmentTime(DateTime appointmentDateTime)
    {
        // Check if it's a weekday (Monday=1, Friday=5)
        DayOfWeek dayOfWeek = appointmentDateTime.DayOfWeek;
        if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
        {
            return false;
        }

        // Check if time is between 8am and 5pm
        int hour = appointmentDateTime.Hour;
        if (hour < APPOINTMENT_START_HOUR || hour >= APPOINTMENT_END_HOUR)
        {
            return false;
        }

        return true;
    }

    private bool IsPhysicianBooked(int physicianId, DateTime appointmentDateTime, int excludeAppointmentId = -1)
    {
        return appointments.Any(a =>
            a.PhysicianId == physicianId &&
            a.AppointmentDateTime.Date == appointmentDateTime.Date &&
            a.AppointmentDateTime.Hour == appointmentDateTime.Hour &&
            a.Id != excludeAppointmentId);
    }

    private bool IsRoomBooked(string room, DateTime appointmentDateTime, int excludeAppointmentId = -1)
    {
        return appointments.Any(a =>
            !string.IsNullOrWhiteSpace(a.Room) &&
            a.Room.Equals(room, StringComparison.OrdinalIgnoreCase) &&
            a.AppointmentDateTime.Date == appointmentDateTime.Date &&
            a.AppointmentDateTime.Hour == appointmentDateTime.Hour &&
            a.Id != excludeAppointmentId);
    }
}
