namespace Library.Klinik.DTOs;

/// <summary>
/// Lightweight DTO for patient list views (excludes medical notes for performance)
/// </summary>
public class PatientSummaryDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
}
