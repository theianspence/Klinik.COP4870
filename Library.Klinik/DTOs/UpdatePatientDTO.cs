namespace Library.Klinik.DTOs;

/// <summary>
/// DTO for updating an existing patient
/// </summary>
public class UpdatePatientDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Race { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public List<string> MedicalNotes { get; set; } = new();
}
