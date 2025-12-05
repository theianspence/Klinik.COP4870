namespace Library.Klinik.Models;

public class Physician
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime GraduationDate { get; set; }
    public List<string> Specializations { get; set; } = new();

    public string FullName => $"{FirstName} {LastName}";

    public override string ToString()
    {
        return $"ID: {Id}, Name: {FirstName} {LastName}, License: {LicenseNumber}, " +
               $"Graduation: {GraduationDate:MM/dd/yyyy}, Specializations: {string.Join(", ", Specializations)}";
    }
}
