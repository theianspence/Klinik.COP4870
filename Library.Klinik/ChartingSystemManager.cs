namespace Library.Klinik;

using Library.Klinik.Services;

public class ChartingSystemManager
{
    public PatientService PatientService { get; }
    public PhysicianService PhysicianService { get; }
    public AppointmentService AppointmentService { get; }

    public ChartingSystemManager()
    {
        PatientService = new PatientService();
        PhysicianService = new PhysicianService();
        AppointmentService = new AppointmentService();
    }
}
