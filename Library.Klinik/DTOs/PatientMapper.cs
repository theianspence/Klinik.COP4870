namespace Library.Klinik.DTOs;

using Library.Klinik.Models;

/// <summary>
/// Mapper for converting between Patient domain models and DTOs
/// </summary>
public static class PatientMapper
{
    /// <summary>
    /// Convert Patient model to PatientDTO
    /// </summary>
    public static PatientDTO ToDTO(Patient patient)
    {
        return new PatientDTO
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Address = patient.Address,
            DateOfBirth = patient.DateOfBirth,
            Race = patient.Race,
            Gender = patient.Gender,
            MedicalNotes = new List<string>(patient.MedicalNotes)
        };
    }

    /// <summary>
    /// Convert Patient model to PatientSummaryDTO
    /// </summary>
    public static PatientSummaryDTO ToSummaryDTO(Patient patient)
    {
        var age = DateTime.Now.Year - patient.DateOfBirth.Year;
        if (DateTime.Now.DayOfYear < patient.DateOfBirth.DayOfYear)
            age--;

        return new PatientSummaryDTO
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            FullName = patient.FullName,
            DateOfBirth = patient.DateOfBirth,
            Age = age,
            Gender = patient.Gender
        };
    }

    /// <summary>
    /// Convert CreatePatientDTO to Patient model
    /// </summary>
    public static Patient ToModel(CreatePatientDTO dto)
    {
        return new Patient
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Address = dto.Address,
            DateOfBirth = dto.DateOfBirth,
            Race = dto.Race,
            Gender = dto.Gender,
            MedicalNotes = new List<string>(dto.MedicalNotes)
        };
    }

    /// <summary>
    /// Update Patient model from UpdatePatientDTO
    /// </summary>
    public static void UpdateModel(Patient patient, UpdatePatientDTO dto)
    {
        patient.FirstName = dto.FirstName;
        patient.LastName = dto.LastName;
        patient.Address = dto.Address;
        patient.DateOfBirth = dto.DateOfBirth;
        patient.Race = dto.Race;
        patient.Gender = dto.Gender;
        patient.MedicalNotes = new List<string>(dto.MedicalNotes);
    }

    /// <summary>
    /// Convert PatientDTO to Patient model
    /// </summary>
    public static Patient ToModel(PatientDTO dto)
    {
        return new Patient
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Address = dto.Address,
            DateOfBirth = dto.DateOfBirth,
            Race = dto.Race,
            Gender = dto.Gender,
            MedicalNotes = new List<string>(dto.MedicalNotes)
        };
    }
}
