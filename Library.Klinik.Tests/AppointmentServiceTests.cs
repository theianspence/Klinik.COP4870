using Library.Klinik.Services;

namespace Library.Klinik.Tests;

public class AppointmentServiceTests
{
    [Fact]
    public void CreateAppointment_Should_Create_Valid_Appointment()
    {
        // Arrange
        var service = new AppointmentService();
        var patientId = 1;
        var physicianId = 1;
        var appointmentDateTime = new DateTime(2025, 12, 5, 10, 0, 0); // Friday 10am
        var reason = "Annual Checkup";

        // Act
        var appointment = service.CreateAppointment(patientId, physicianId, appointmentDateTime, reason);

        // Assert
        Assert.NotNull(appointment);
        Assert.Equal(1, appointment.Id);
        Assert.Equal(patientId, appointment.PatientId);
        Assert.Equal(physicianId, appointment.PhysicianId);
        Assert.Equal(reason, appointment.Reason);
    }

    [Fact]
    public void CreateAppointment_Should_Reject_Weekend_Appointment()
    {
        // Arrange
        var service = new AppointmentService();
        var patientId = 1;
        var physicianId = 1;
        var appointmentDateTime = new DateTime(2025, 12, 6, 10, 0, 0); // Saturday 10am

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            service.CreateAppointment(patientId, physicianId, appointmentDateTime, "Checkup"));
        Assert.Contains("between 8am-5pm", exception.Message);
    }

    [Fact]
    public void CreateAppointment_Should_Reject_Before_8am()
    {
        // Arrange
        var service = new AppointmentService();
        var patientId = 1;
        var physicianId = 1;
        var appointmentDateTime = new DateTime(2025, 12, 5, 7, 0, 0); // Friday 7am

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            service.CreateAppointment(patientId, physicianId, appointmentDateTime, "Checkup"));
        Assert.Contains("between 8am-5pm", exception.Message);
    }

    [Fact]
    public void CreateAppointment_Should_Reject_After_5pm()
    {
        // Arrange
        var service = new AppointmentService();
        var patientId = 1;
        var physicianId = 1;
        var appointmentDateTime = new DateTime(2025, 12, 5, 17, 0, 0); // Friday 5pm

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            service.CreateAppointment(patientId, physicianId, appointmentDateTime, "Checkup"));
        Assert.Contains("between 8am-5pm", exception.Message);
    }

    [Fact]
    public void CreateAppointment_Should_Allow_4_59pm()
    {
        // Arrange
        var service = new AppointmentService();
        var patientId = 1;
        var physicianId = 1;
        var appointmentDateTime = new DateTime(2025, 12, 5, 16, 59, 0); // Friday 4:59pm

        // Act
        var appointment = service.CreateAppointment(patientId, physicianId, appointmentDateTime, "Checkup");

        // Assert
        Assert.NotNull(appointment);
    }

    [Fact]
    public void CreateAppointment_Should_Prevent_Double_Booking()
    {
        // Arrange
        var service = new AppointmentService();
        var patientId1 = 1;
        var patientId2 = 2;
        var physicianId = 1;
        var appointmentDateTime = new DateTime(2025, 12, 5, 10, 0, 0); // Friday 10am

        // Act
        var firstAppointment = service.CreateAppointment(patientId1, physicianId, appointmentDateTime, "Checkup");

        // Assert
        Assert.NotNull(firstAppointment);
        var exception = Assert.Throws<InvalidOperationException>(() =>
            service.CreateAppointment(patientId2, physicianId, appointmentDateTime, "Checkup"));
        Assert.Contains("already booked", exception.Message);
    }

    [Fact]
    public void CreateAppointment_Should_Allow_Different_Hours_Same_Day()
    {
        // Arrange
        var service = new AppointmentService();
        var patientId1 = 1;
        var patientId2 = 2;
        var physicianId = 1;
        var appointmentDateTime1 = new DateTime(2025, 12, 5, 10, 0, 0); // Friday 10am
        var appointmentDateTime2 = new DateTime(2025, 12, 5, 11, 0, 0); // Friday 11am

        // Act
        var appointment1 = service.CreateAppointment(patientId1, physicianId, appointmentDateTime1, "Checkup");
        var appointment2 = service.CreateAppointment(patientId2, physicianId, appointmentDateTime2, "Checkup");

        // Assert
        Assert.NotNull(appointment1!);
        Assert.NotNull(appointment2!);
        Assert.NotEqual(appointment1.Id, appointment2.Id);
    }

    [Fact]
    public void CreateAppointment_Should_Allow_Same_Hour_Different_Day()
    {
        // Arrange
        var service = new AppointmentService();
        var patientId1 = 1;
        var patientId2 = 2;
        var physicianId = 1;
        var appointmentDateTime1 = new DateTime(2025, 12, 5, 10, 0, 0); // Friday 10am
        var appointmentDateTime2 = new DateTime(2025, 12, 8, 10, 0, 0); // Monday 10am

        // Act
        var appointment1 = service.CreateAppointment(patientId1, physicianId, appointmentDateTime1, "Checkup");
        var appointment2 = service.CreateAppointment(patientId2, physicianId, appointmentDateTime2, "Checkup");

        // Assert
        Assert.NotNull(appointment1!);
        Assert.NotNull(appointment2!);
    }

    [Fact]
    public void GetAppointmentById_Should_Return_Correct_Appointment()
    {
        // Arrange
        var service = new AppointmentService();
        var appointmentDateTime = new DateTime(2025, 12, 5, 10, 0, 0);
        var appointment = service.CreateAppointment(1, 1, appointmentDateTime, "Checkup");

        // Act
        var retrieved = service.GetAppointmentById(appointment.Id);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(appointment.Id, retrieved.Id);
    }

    [Fact]
    public void GetAppointmentById_Should_Return_Null_For_Nonexistent_Appointment()
    {
        // Arrange
        var service = new AppointmentService();

        // Act
        var retrieved = service.GetAppointmentById(999);

        // Assert
        Assert.Null(retrieved);
    }

    [Fact]
    public void GetAllAppointments_Should_Return_All_Created_Appointments()
    {
        // Arrange
        var service = new AppointmentService();
        var dateTime1 = new DateTime(2025, 12, 5, 10, 0, 0);
        var dateTime2 = new DateTime(2025, 12, 5, 11, 0, 0);
        service.CreateAppointment(1, 1, dateTime1, "Checkup");
        service.CreateAppointment(2, 2, dateTime2, "Checkup");

        // Act
        var appointments = service.GetAllAppointments();

        // Assert
        Assert.NotNull(appointments);
        Assert.Equal(2, appointments.Count);
    }

    [Fact]
    public void UpdateAppointment_Should_Update_Valid_Appointment()
    {
        // Arrange
        var service = new AppointmentService();
        var dateTime1 = new DateTime(2025, 12, 5, 10, 0, 0);
        var dateTime2 = new DateTime(2025, 12, 5, 14, 0, 0);
        var appointment = service.CreateAppointment(1, 1, dateTime1, "Checkup");

        // Act
        var result = service.UpdateAppointment(appointment.Id, 2, 2, dateTime2, "Follow-up");
        var updated = service.GetAppointmentById(appointment.Id);

        // Assert
        Assert.True(result);
        Assert.NotNull(updated);
        Assert.Equal(2, updated.PatientId);
        Assert.Equal(2, updated.PhysicianId);
    }

    [Fact]
    public void UpdateAppointment_Should_Return_False_For_Nonexistent_Appointment()
    {
        // Arrange
        var service = new AppointmentService();
        var dateTime = new DateTime(2025, 12, 5, 10, 0, 0);

        // Act
        var result = service.UpdateAppointment(999, 1, 1, dateTime, "Checkup");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DeleteAppointment_Should_Remove_Appointment()
    {
        // Arrange
        var service = new AppointmentService();
        var dateTime = new DateTime(2025, 12, 5, 10, 0, 0);
        var appointment = service.CreateAppointment(1, 1, dateTime, "Checkup");

        // Act
        var result = service.DeleteAppointment(appointment.Id);
        var retrieved = service.GetAppointmentById(appointment.Id);

        // Assert
        Assert.True(result);
        Assert.Null(retrieved);
    }

    [Fact]
    public void DeleteAppointment_Should_Return_False_For_Nonexistent_Appointment()
    {
        // Arrange
        var service = new AppointmentService();

        // Act
        var result = service.DeleteAppointment(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetAppointmentsForPhysician_Should_Return_Physician_Appointments()
    {
        // Arrange
        var service = new AppointmentService();
        var dateTime1 = new DateTime(2025, 12, 5, 10, 0, 0);
        var dateTime2 = new DateTime(2025, 12, 5, 11, 0, 0);
        var dateTime3 = new DateTime(2025, 12, 5, 14, 0, 0);
        service.CreateAppointment(1, 1, dateTime1, "Checkup");
        service.CreateAppointment(2, 1, dateTime2, "Checkup");
        service.CreateAppointment(3, 2, dateTime3, "Checkup");

        // Act
        var appointments = service.GetAppointmentsForPhysician(1);

        // Assert
        Assert.NotNull(appointments!);
        Assert.Equal(2, appointments.Count);
        Assert.All(appointments, a => Assert.Equal(1, a.PhysicianId));
    }

    [Fact]
    public void GetAppointmentsForPatient_Should_Return_Patient_Appointments()
    {
        // Arrange
        var service = new AppointmentService();
        var dateTime1 = new DateTime(2025, 12, 5, 10, 0, 0);
        var dateTime2 = new DateTime(2025, 12, 5, 14, 0, 0);
        var dateTime3 = new DateTime(2025, 12, 8, 10, 0, 0);
        service.CreateAppointment(1, 1, dateTime1, "Checkup");
        service.CreateAppointment(1, 2, dateTime2, "Follow-up");
        service.CreateAppointment(2, 1, dateTime3, "Checkup");

        // Act
        var appointments = service.GetAppointmentsForPatient(1);

        // Assert
        Assert.NotNull(appointments);
        Assert.Equal(2, appointments.Count);
        Assert.All(appointments, a => Assert.Equal(1, a.PatientId));
    }
}
