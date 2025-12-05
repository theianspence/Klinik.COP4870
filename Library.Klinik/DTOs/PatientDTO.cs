namespace Library.Klinik.DTOs;

/// <summary>
/// Data Transfer Object for Patient - used for API requests/responses
/// </summary>
public class PatientDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Race { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public List<string> MedicalNotes { get; set; } = new();
}
