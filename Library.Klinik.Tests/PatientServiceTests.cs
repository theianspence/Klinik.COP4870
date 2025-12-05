using Library.Klinik.Services;

namespace Library.Klinik.Tests;

public class PatientServiceTests
{
    [Fact]
    public void CreatePatient_Should_Create_Patient_With_Correct_Id()
    {
        // Arrange
        var service = new PatientService();
        var firstName = "John";
        var lastName = "Doe";
        var address = "123 Main St";
        var dob = new DateTime(1990, 5, 15);
        var race = "Caucasian";
        var gender = "Male";

        // Act
        var patient = service.CreatePatient(firstName, lastName, address, dob, race, gender);

        // Assert
        Assert.NotNull(patient);
        Assert.Equal(1, patient.Id);
        Assert.Equal(firstName, patient.FirstName);
        Assert.Equal(lastName, patient.LastName);
        Assert.Equal(address, patient.Address);
        Assert.Equal(dob, patient.DateOfBirth);
        Assert.Equal(race, patient.Race);
        Assert.Equal(gender, patient.Gender);
    }

    [Fact]
    public void CreatePatient_Should_Increment_Id()
    {
        // Arrange
        var service = new PatientService();

        // Act
        var patient1 = service.CreatePatient("John", "Doe", "123 Main St", new DateTime(1990, 5, 15), "Caucasian", "Male");
        var patient2 = service.CreatePatient("Jane", "Smith", "456 Oak Ave", new DateTime(1992, 3, 20), "African American", "Female");

        // Assert
        Assert.Equal(1, patient1.Id);
        Assert.Equal(2, patient2.Id);
    }

    [Fact]
    public void GetPatientById_Should_Return_Correct_Patient()
    {
        // Arrange
        var service = new PatientService();
        var patient = service.CreatePatient("John", "Doe", "123 Main St", new DateTime(1990, 5, 15), "Caucasian", "Male");

        // Act
        var retrieved = service.GetPatientById(patient.Id);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(patient.Id, retrieved.Id);
        Assert.Equal(patient.FirstName, retrieved.FirstName);
    }

    [Fact]
    public void GetPatientById_Should_Return_Null_For_Nonexistent_Patient()
    {
        // Arrange
        var service = new PatientService();

        // Act
        var retrieved = service.GetPatientById(999);

        // Assert
        Assert.Null(retrieved);
    }

    [Fact]
    public void GetAllPatients_Should_Return_All_Created_Patients()
    {
        // Arrange
        var service = new PatientService();
        service.CreatePatient("John", "Doe", "123 Main St", new DateTime(1990, 5, 15), "Caucasian", "Male");
        service.CreatePatient("Jane", "Smith", "456 Oak Ave", new DateTime(1992, 3, 20), "African American", "Female");

        // Act
        var patients = service.GetAllPatients();

        // Assert
        Assert.NotNull(patients);
        Assert.Equal(2, patients.Count);
    }

    [Fact]
    public void UpdatePatient_Should_Update_Patient_Information()
    {
        // Arrange
        var service = new PatientService();
        var patient = service.CreatePatient("John", "Doe", "123 Main St", new DateTime(1990, 5, 15), "Caucasian", "Male");
        var newFirstName = "Jane";
        var newLastName = "Smith";

        // Act
        var result = service.UpdatePatient(patient.Id, newFirstName, newLastName, "456 Oak Ave", new DateTime(1992, 3, 20), "Asian", "Female");
        var updated = service.GetPatientById(patient.Id);

        // Assert
        Assert.True(result);
        Assert.NotNull(updated);
        Assert.Equal(newFirstName, updated.FirstName);
        Assert.Equal(newLastName, updated.LastName);
    }

    [Fact]
    public void UpdatePatient_Should_Return_False_For_Nonexistent_Patient()
    {
        // Arrange
        var service = new PatientService();

        // Act
        var result = service.UpdatePatient(999, "John", "Doe", "123 Main St", new DateTime(1990, 5, 15), "Caucasian", "Male");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DeletePatient_Should_Remove_Patient()
    {
        // Arrange
        var service = new PatientService();
        var patient = service.CreatePatient("John", "Doe", "123 Main St", new DateTime(1990, 5, 15), "Caucasian", "Male");

        // Act
        var result = service.DeletePatient(patient.Id);
        var retrieved = service.GetPatientById(patient.Id);

        // Assert
        Assert.True(result);
        Assert.Null(retrieved);
    }

    [Fact]
    public void DeletePatient_Should_Return_False_For_Nonexistent_Patient()
    {
        // Arrange
        var service = new PatientService();

        // Act
        var result = service.DeletePatient(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AddMedicalNote_Should_Add_Note_To_Patient()
    {
        // Arrange
        var service = new PatientService();
        var patient = service.CreatePatient("John", "Doe", "123 Main St", new DateTime(1990, 5, 15), "Caucasian", "Male");

        // Act
        var result = service.AddMedicalNote(patient.Id, "Flu", "Tamiflu");
        var notes = service.GetMedicalNotes(patient.Id);

        // Assert
        Assert.True(result);
        Assert.NotNull(notes);
        Assert.Single(notes);
        Assert.Contains("Diagnosis: Flu", notes[0]);
        Assert.Contains("Prescription: Tamiflu", notes[0]);
    }

    [Fact]
    public void AddMedicalNote_Should_Return_False_For_Nonexistent_Patient()
    {
        // Arrange
        var service = new PatientService();

        // Act
        var result = service.AddMedicalNote(999, "Flu", "Tamiflu");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetMedicalNotes_Should_Return_Multiple_Notes()
    {
        // Arrange
        var service = new PatientService();
        var patient = service.CreatePatient("John", "Doe", "123 Main St", new DateTime(1990, 5, 15), "Caucasian", "Male");
        service.AddMedicalNote(patient.Id, "Flu", "Tamiflu");
        service.AddMedicalNote(patient.Id, "Headache", "Aspirin");

        // Act
        var notes = service.GetMedicalNotes(patient.Id);

        // Assert
        Assert.Equal(2, notes.Count);
    }

    [Fact]
    public void GetMedicalNotes_Should_Return_Empty_List_For_Nonexistent_Patient()
    {
        // Arrange
        var service = new PatientService();

        // Act
        var notes = service.GetMedicalNotes(999);

        // Assert
        Assert.NotNull(notes);
        Assert.Empty(notes);
    }
}
