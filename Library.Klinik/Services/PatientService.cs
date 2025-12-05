namespace Library.Klinik.Services;

using Library.Klinik.Models;

public class PatientService
{
    private List<Patient> patients = new();
    private int nextPatientId = 1;

    public Patient CreatePatient(string firstName, string lastName, string address, 
                                  DateTime dateOfBirth, string race, string gender)
    {
        var patient = new Patient
        {
            Id = nextPatientId++,
            FirstName = firstName,
            LastName = lastName,
            Address = address,
            DateOfBirth = dateOfBirth,
            Race = race,
            Gender = gender
        };
        patients.Add(patient);
        return patient;
    }

    public Patient AddPatient(Patient patient)
    {
        // Assign a new ID if not set or if it already exists
        if (patient.Id <= 0 || GetPatientById(patient.Id) != null)
        {
            patient.Id = nextPatientId++;
        }
        else
        {
            // Update nextPatientId if the provided ID is higher
            if (patient.Id >= nextPatientId)
            {
                nextPatientId = patient.Id + 1;
            }
        }
        patients.Add(patient);
        return patient;
    }

    public Patient? GetPatientById(int id)
    {
        return patients.FirstOrDefault(p => p.Id == id);
    }

    public List<Patient> GetAllPatients()
    {
        return new List<Patient>(patients);
    }

    public bool UpdatePatient(int id, string firstName, string lastName, string address,
                              DateTime dateOfBirth, string race, string gender)
    {
        var patient = GetPatientById(id);
        if (patient == null) return false;

        patient.FirstName = firstName;
        patient.LastName = lastName;
        patient.Address = address;
        patient.DateOfBirth = dateOfBirth;
        patient.Race = race;
        patient.Gender = gender;
        return true;
    }

    public bool UpdatePatient(Patient updatedPatient)
    {
        var patient = GetPatientById(updatedPatient.Id);
        if (patient == null) return false;

        patient.FirstName = updatedPatient.FirstName;
        patient.LastName = updatedPatient.LastName;
        patient.Address = updatedPatient.Address;
        patient.DateOfBirth = updatedPatient.DateOfBirth;
        patient.Race = updatedPatient.Race;
        patient.Gender = updatedPatient.Gender;
        patient.MedicalNotes = updatedPatient.MedicalNotes;
        return true;
    }

    public bool DeletePatient(int id)
    {
        var patient = GetPatientById(id);
        if (patient == null) return false;
        patients.Remove(patient);
        return true;
    }

    public bool AddMedicalNote(int patientId, string diagnosis, string prescription)
    {
        var patient = GetPatientById(patientId);
        if (patient == null) return false;

        var note = $"[{DateTime.Now:MM/dd/yyyy HH:mm}] Diagnosis: {diagnosis}, Prescription: {prescription}";
        patient.MedicalNotes.Add(note);
        return true;
    }

    public List<string> GetMedicalNotes(int patientId)
    {
        var patient = GetPatientById(patientId);
        return patient?.MedicalNotes ?? new List<string>();
    }
}
