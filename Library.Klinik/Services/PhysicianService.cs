namespace Library.Klinik.Services;

using Library.Klinik.Models;

public class PhysicianService
{
    private List<Physician> physicians = new();
    private int nextPhysicianId = 1;

    public Physician CreatePhysician(string firstName, string lastName, string licenseNumber,
                                     DateTime graduationDate)
    {
        var physician = new Physician
        {
            Id = nextPhysicianId++,
            FirstName = firstName,
            LastName = lastName,
            LicenseNumber = licenseNumber,
            GraduationDate = graduationDate
        };
        physicians.Add(physician);
        return physician;
    }

    public Physician? GetPhysicianById(int id)
    {
        return physicians.FirstOrDefault(p => p.Id == id);
    }

    public List<Physician> GetAllPhysicians()
    {
        return new List<Physician>(physicians);
    }

    public bool UpdatePhysician(int id, string firstName, string lastName, string licenseNumber,
                                DateTime graduationDate)
    {
        var physician = GetPhysicianById(id);
        if (physician == null) return false;

        physician.FirstName = firstName;
        physician.LastName = lastName;
        physician.LicenseNumber = licenseNumber;
        physician.GraduationDate = graduationDate;
        return true;
    }

    public bool DeletePhysician(int id)
    {
        var physician = GetPhysicianById(id);
        if (physician == null) return false;
        physicians.Remove(physician);
        return true;
    }

    public bool AddSpecialization(int physicianId, string specialization)
    {
        var physician = GetPhysicianById(physicianId);
        if (physician == null) return false;

        if (!physician.Specializations.Contains(specialization))
        {
            physician.Specializations.Add(specialization);
        }
        return true;
    }

    public List<string> GetSpecializations(int physicianId)
    {
        var physician = GetPhysicianById(physicianId);
        return physician?.Specializations ?? new List<string>();
    }
}
