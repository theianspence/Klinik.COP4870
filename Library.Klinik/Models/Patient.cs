namespace Library.Klinik.Models;

public class Patient
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Race { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public List<string> MedicalNotes { get; set; } = new();

    public string FullName => $"{FirstName} {LastName}";

    public override string ToString()
    {
        return $"ID: {Id}, Name: {FirstName} {LastName}, DOB: {DateOfBirth:MM/dd/yyyy}, " +
               $"Address: {Address}, Race: {Race}, Gender: {Gender}";
    }
}
