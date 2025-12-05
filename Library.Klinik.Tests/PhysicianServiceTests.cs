using Library.Klinik.Services;

namespace Library.Klinik.Tests;

public class PhysicianServiceTests
{
    [Fact]
    public void CreatePhysician_Should_Create_Physician_With_Correct_Id()
    {
        // Arrange
        var service = new PhysicianService();
        var firstName = "Dr. John";
        var lastName = "Smith";
        var licenseNumber = "MD123456";
        var gradDate = new DateTime(2010, 6, 15);

        // Act
        var physician = service.CreatePhysician(firstName, lastName, licenseNumber, gradDate);

        // Assert
        Assert.NotNull(physician);
        Assert.Equal(1, physician.Id);
        Assert.Equal(firstName, physician.FirstName);
        Assert.Equal(lastName, physician.LastName);
        Assert.Equal(licenseNumber, physician.LicenseNumber);
        Assert.Equal(gradDate, physician.GraduationDate);
    }

    [Fact]
    public void CreatePhysician_Should_Increment_Id()
    {
        // Arrange
        var service = new PhysicianService();

        // Act
        var physician1 = service.CreatePhysician("Dr. John", "Smith", "MD123456", new DateTime(2010, 6, 15));
        var physician2 = service.CreatePhysician("Dr. Jane", "Doe", "MD654321", new DateTime(2012, 5, 20));

        // Assert
        Assert.Equal(1, physician1.Id);
        Assert.Equal(2, physician2.Id);
    }

    [Fact]
    public void GetPhysicianById_Should_Return_Correct_Physician()
    {
        // Arrange
        var service = new PhysicianService();
        var physician = service.CreatePhysician("Dr. John", "Smith", "MD123456", new DateTime(2010, 6, 15));

        // Act
        var retrieved = service.GetPhysicianById(physician.Id);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(physician.Id, retrieved.Id);
        Assert.Equal(physician.FirstName, retrieved.FirstName);
    }

    [Fact]
    public void GetPhysicianById_Should_Return_Null_For_Nonexistent_Physician()
    {
        // Arrange
        var service = new PhysicianService();

        // Act
        var retrieved = service.GetPhysicianById(999);

        // Assert
        Assert.Null(retrieved);
    }

    [Fact]
    public void GetAllPhysicians_Should_Return_All_Created_Physicians()
    {
        // Arrange
        var service = new PhysicianService();
        service.CreatePhysician("Dr. John", "Smith", "MD123456", new DateTime(2010, 6, 15));
        service.CreatePhysician("Dr. Jane", "Doe", "MD654321", new DateTime(2012, 5, 20));

        // Act
        var physicians = service.GetAllPhysicians();

        // Assert
        Assert.NotNull(physicians);
        Assert.Equal(2, physicians.Count);
    }

    [Fact]
    public void UpdatePhysician_Should_Update_Physician_Information()
    {
        // Arrange
        var service = new PhysicianService();
        var physician = service.CreatePhysician("Dr. John", "Smith", "MD123456", new DateTime(2010, 6, 15));

        // Act
        var result = service.UpdatePhysician(physician.Id, "Dr. Jane", "Doe", "MD654321", new DateTime(2012, 5, 20));
        var updated = service.GetPhysicianById(physician.Id);

        // Assert
        Assert.True(result);
        Assert.NotNull(updated);
        Assert.Equal("Dr. Jane", updated.FirstName);
        Assert.Equal("Doe", updated.LastName);
    }

    [Fact]
    public void UpdatePhysician_Should_Return_False_For_Nonexistent_Physician()
    {
        // Arrange
        var service = new PhysicianService();

        // Act
        var result = service.UpdatePhysician(999, "Dr. John", "Smith", "MD123456", new DateTime(2010, 6, 15));

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DeletePhysician_Should_Remove_Physician()
    {
        // Arrange
        var service = new PhysicianService();
        var physician = service.CreatePhysician("Dr. John", "Smith", "MD123456", new DateTime(2010, 6, 15));

        // Act
        var result = service.DeletePhysician(physician.Id);
        var retrieved = service.GetPhysicianById(physician.Id);

        // Assert
        Assert.True(result);
        Assert.Null(retrieved);
    }

    [Fact]
    public void DeletePhysician_Should_Return_False_For_Nonexistent_Physician()
    {
        // Arrange
        var service = new PhysicianService();

        // Act
        var result = service.DeletePhysician(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AddSpecialization_Should_Add_Specialization_To_Physician()
    {
        // Arrange
        var service = new PhysicianService();
        var physician = service.CreatePhysician("Dr. John", "Smith", "MD123456", new DateTime(2010, 6, 15));

        // Act
        var result = service.AddSpecialization(physician.Id, "Cardiology");
        var specializations = service.GetSpecializations(physician.Id);

        // Assert
        Assert.True(result);
        Assert.NotNull(specializations);
        Assert.Single(specializations);
        Assert.Contains("Cardiology", specializations);
    }

    [Fact]
    public void AddSpecialization_Should_Not_Add_Duplicate_Specialization()
    {
        // Arrange
        var service = new PhysicianService();
        var physician = service.CreatePhysician("Dr. John", "Smith", "MD123456", new DateTime(2010, 6, 15));
        service.AddSpecialization(physician.Id, "Cardiology");

        // Act
        service.AddSpecialization(physician.Id, "Cardiology");
        var specializations = service.GetSpecializations(physician.Id);

        // Assert
        Assert.Single(specializations);
    }

    [Fact]
    public void AddSpecialization_Should_Return_False_For_Nonexistent_Physician()
    {
        // Arrange
        var service = new PhysicianService();

        // Act
        var result = service.AddSpecialization(999, "Cardiology");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetSpecializations_Should_Return_Multiple_Specializations()
    {
        // Arrange
        var service = new PhysicianService();
        var physician = service.CreatePhysician("Dr. John", "Smith", "MD123456", new DateTime(2010, 6, 15));
        service.AddSpecialization(physician.Id, "Cardiology");
        service.AddSpecialization(physician.Id, "Internal Medicine");

        // Act
        var specializations = service.GetSpecializations(physician.Id);

        // Assert
        Assert.Equal(2, specializations.Count);
    }

    [Fact]
    public void GetSpecializations_Should_Return_Empty_List_For_Nonexistent_Physician()
    {
        // Arrange
        var service = new PhysicianService();

        // Act
        var specializations = service.GetSpecializations(999);

        // Assert
        Assert.NotNull(specializations);
        Assert.Empty(specializations);
    }
}
