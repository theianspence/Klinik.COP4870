using Library.Klinik;
using Library.Klinik.Models;
using Library.Klinik.Services;

class Program
{
    private static ChartingSystemManager manager = new();

    static void Main()
    {
        bool running = true;
        while (running)
        {
            DisplayMainMenu();
            string choice = Console.ReadLine() ?? "";
            
            switch (choice)
            {
                case "1":
                    ManagePatients();
                    break;
                case "2":
                    ManagePhysicians();
                    break;
                case "3":
                    ManageAppointments();
                    break;
                case "4":
                    running = false;
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void DisplayMainMenu()
    {
        Console.WriteLine("\n=== Medical Practice Charting System ===");
        Console.WriteLine("1. Manage Patients");
        Console.WriteLine("2. Manage Physicians");
        Console.WriteLine("3. Manage Appointments");
        Console.WriteLine("4. Exit");
        Console.Write("Choose an option: ");
    }

    static void ManagePatients()
    {
        bool patientMenu = true;
        while (patientMenu)
        {
            Console.WriteLine("\n=== Patient Management ===");
            Console.WriteLine("1. Create Patient");
            Console.WriteLine("2. View All Patients");
            Console.WriteLine("3. View Patient Details");
            Console.WriteLine("4. Update Patient");
            Console.WriteLine("5. Delete Patient");
            Console.WriteLine("6. Add Medical Note");
            Console.WriteLine("7. View Medical Notes");
            Console.WriteLine("8. Back to Main Menu");
            Console.Write("Choose an option: ");
            
            string choice = Console.ReadLine() ?? "";
            
            switch (choice)
            {
                case "1":
                    CreatePatient();
                    break;
                case "2":
                    ViewAllPatients();
                    break;
                case "3":
                    ViewPatientDetails();
                    break;
                case "4":
                    UpdatePatient();
                    break;
                case "5":
                    DeletePatient();
                    break;
                case "6":
                    AddMedicalNote();
                    break;
                case "7":
                    ViewMedicalNotes();
                    break;
                case "8":
                    patientMenu = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    static void CreatePatient()
    {
        Console.Write("First Name: ");
        string firstName = Console.ReadLine() ?? "";
        Console.Write("Last Name: ");
        string lastName = Console.ReadLine() ?? "";
        Console.Write("Address: ");
        string address = Console.ReadLine() ?? "";
        Console.Write("Date of Birth (MM/dd/yyyy): ");
        DateTime dob = DateTime.Parse(Console.ReadLine() ?? "");
        Console.Write("Race: ");
        string race = Console.ReadLine() ?? "";
        Console.Write("Gender: ");
        string gender = Console.ReadLine() ?? "";

        var patient = manager.PatientService.CreatePatient(firstName, lastName, address, dob, race, gender);
        Console.WriteLine($"Patient created successfully! ID: {patient.Id}");
    }

    static void ViewAllPatients()
    {
        var patients = manager.PatientService.GetAllPatients();
        if (patients.Count == 0)
        {
            Console.WriteLine("No patients found.");
            return;
        }

        Console.WriteLine("\n=== All Patients ===");
        foreach (var patient in patients)
        {
            Console.WriteLine(patient);
        }
    }

    static void ViewPatientDetails()
    {
        Console.Write("Enter Patient ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        var patient = manager.PatientService.GetPatientById(id);
        
        if (patient == null)
        {
            Console.WriteLine("Patient not found.");
            return;
        }

        Console.WriteLine($"\n{patient}");
        var notes = manager.PatientService.GetMedicalNotes(id);
        if (notes.Count > 0)
        {
            Console.WriteLine("Medical Notes:");
            foreach (var note in notes)
            {
                Console.WriteLine($"  - {note}");
            }
        }
    }

    static void UpdatePatient()
    {
        Console.Write("Enter Patient ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        var patient = manager.PatientService.GetPatientById(id);
        
        if (patient == null)
        {
            Console.WriteLine("Patient not found.");
            return;
        }

        Console.Write("First Name (current: " + patient.FirstName + "): ");
        string firstName = Console.ReadLine() ?? patient.FirstName;
        Console.Write("Last Name (current: " + patient.LastName + "): ");
        string lastName = Console.ReadLine() ?? patient.LastName;
        Console.Write("Address (current: " + patient.Address + "): ");
        string address = Console.ReadLine() ?? patient.Address;
        Console.Write("Date of Birth (current: " + patient.DateOfBirth.ToString("MM/dd/yyyy") + ") MM/dd/yyyy: ");
        DateTime dob = DateTime.Parse(Console.ReadLine() ?? patient.DateOfBirth.ToString("MM/dd/yyyy"));
        Console.Write("Race (current: " + patient.Race + "): ");
        string race = Console.ReadLine() ?? patient.Race;
        Console.Write("Gender (current: " + patient.Gender + "): ");
        string gender = Console.ReadLine() ?? patient.Gender;

        if (manager.PatientService.UpdatePatient(id, firstName, lastName, address, dob, race, gender))
        {
            Console.WriteLine("Patient updated successfully!");
        }
    }

    static void DeletePatient()
    {
        Console.Write("Enter Patient ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        
        if (manager.PatientService.DeletePatient(id))
        {
            Console.WriteLine("Patient deleted successfully!");
        }
        else
        {
            Console.WriteLine("Patient not found.");
        }
    }

    static void AddMedicalNote()
    {
        Console.Write("Enter Patient ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Diagnosis: ");
        string diagnosis = Console.ReadLine() ?? "";
        Console.Write("Prescription: ");
        string prescription = Console.ReadLine() ?? "";

        if (manager.PatientService.AddMedicalNote(id, diagnosis, prescription))
        {
            Console.WriteLine("Medical note added successfully!");
        }
        else
        {
            Console.WriteLine("Patient not found.");
        }
    }

    static void ViewMedicalNotes()
    {
        Console.Write("Enter Patient ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        var notes = manager.PatientService.GetMedicalNotes(id);
        
        if (notes.Count == 0)
        {
            Console.WriteLine("No medical notes found for this patient.");
            return;
        }

        Console.WriteLine("\n=== Medical Notes ===");
        foreach (var note in notes)
        {
            Console.WriteLine(note);
        }
    }

    static void ManagePhysicians()
    {
        bool physicianMenu = true;
        while (physicianMenu)
        {
            Console.WriteLine("\n=== Physician Management ===");
            Console.WriteLine("1. Create Physician");
            Console.WriteLine("2. View All Physicians");
            Console.WriteLine("3. View Physician Details");
            Console.WriteLine("4. Update Physician");
            Console.WriteLine("5. Delete Physician");
            Console.WriteLine("6. Add Specialization");
            Console.WriteLine("7. View Specializations");
            Console.WriteLine("8. Back to Main Menu");
            Console.Write("Choose an option: ");
            
            string choice = Console.ReadLine() ?? "";
            
            switch (choice)
            {
                case "1":
                    CreatePhysician();
                    break;
                case "2":
                    ViewAllPhysicians();
                    break;
                case "3":
                    ViewPhysicianDetails();
                    break;
                case "4":
                    UpdatePhysician();
                    break;
                case "5":
                    DeletePhysician();
                    break;
                case "6":
                    AddSpecialization();
                    break;
                case "7":
                    ViewSpecializations();
                    break;
                case "8":
                    physicianMenu = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    static void CreatePhysician()
    {
        Console.Write("First Name: ");
        string firstName = Console.ReadLine() ?? "";
        Console.Write("Last Name: ");
        string lastName = Console.ReadLine() ?? "";
        Console.Write("License Number: ");
        string licenseNumber = Console.ReadLine() ?? "";
        Console.Write("Graduation Date (MM/dd/yyyy): ");
        DateTime gradDate = DateTime.Parse(Console.ReadLine() ?? "");

        var physician = manager.PhysicianService.CreatePhysician(firstName, lastName, licenseNumber, gradDate);
        Console.WriteLine($"Physician created successfully! ID: {physician.Id}");
    }

    static void ViewAllPhysicians()
    {
        var physicians = manager.PhysicianService.GetAllPhysicians();
        if (physicians.Count == 0)
        {
            Console.WriteLine("No physicians found.");
            return;
        }

        Console.WriteLine("\n=== All Physicians ===");
        foreach (var physician in physicians)
        {
            Console.WriteLine(physician);
        }
    }

    static void ViewPhysicianDetails()
    {
        Console.Write("Enter Physician ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        var physician = manager.PhysicianService.GetPhysicianById(id);
        
        if (physician == null)
        {
            Console.WriteLine("Physician not found.");
            return;
        }

        Console.WriteLine($"\n{physician}");
    }

    static void UpdatePhysician()
    {
        Console.Write("Enter Physician ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        var physician = manager.PhysicianService.GetPhysicianById(id);
        
        if (physician == null)
        {
            Console.WriteLine("Physician not found.");
            return;
        }

        Console.Write("First Name (current: " + physician.FirstName + "): ");
        string firstName = Console.ReadLine() ?? physician.FirstName;
        Console.Write("Last Name (current: " + physician.LastName + "): ");
        string lastName = Console.ReadLine() ?? physician.LastName;
        Console.Write("License Number (current: " + physician.LicenseNumber + "): ");
        string licenseNumber = Console.ReadLine() ?? physician.LicenseNumber;
        Console.Write("Graduation Date (current: " + physician.GraduationDate.ToString("MM/dd/yyyy") + ") MM/dd/yyyy: ");
        DateTime gradDate = DateTime.Parse(Console.ReadLine() ?? physician.GraduationDate.ToString("MM/dd/yyyy"));

        if (manager.PhysicianService.UpdatePhysician(id, firstName, lastName, licenseNumber, gradDate))
        {
            Console.WriteLine("Physician updated successfully!");
        }
    }

    static void DeletePhysician()
    {
        Console.Write("Enter Physician ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        
        if (manager.PhysicianService.DeletePhysician(id))
        {
            Console.WriteLine("Physician deleted successfully!");
        }
        else
        {
            Console.WriteLine("Physician not found.");
        }
    }

    static void AddSpecialization()
    {
        Console.Write("Enter Physician ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Specialization: ");
        string specialization = Console.ReadLine() ?? "";

        if (manager.PhysicianService.AddSpecialization(id, specialization))
        {
            Console.WriteLine("Specialization added successfully!");
        }
        else
        {
            Console.WriteLine("Physician not found.");
        }
    }

    static void ViewSpecializations()
    {
        Console.Write("Enter Physician ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        var specializations = manager.PhysicianService.GetSpecializations(id);
        
        if (specializations.Count == 0)
        {
            Console.WriteLine("No specializations found for this physician.");
            return;
        }

        Console.WriteLine("\n=== Specializations ===");
        foreach (var spec in specializations)
        {
            Console.WriteLine($"  - {spec}");
        }
    }

    static void ManageAppointments()
    {
        bool appointmentMenu = true;
        while (appointmentMenu)
        {
            Console.WriteLine("\n=== Appointment Management ===");
            Console.WriteLine("1. Create Appointment");
            Console.WriteLine("2. View All Appointments");
            Console.WriteLine("3. View Appointment Details");
            Console.WriteLine("4. Update Appointment");
            Console.WriteLine("5. Delete Appointment");
            Console.WriteLine("6. View Physician's Appointments");
            Console.WriteLine("7. View Patient's Appointments");
            Console.WriteLine("8. Back to Main Menu");
            Console.Write("Choose an option: ");
            
            string choice = Console.ReadLine() ?? "";
            
            switch (choice)
            {
                case "1":
                    CreateAppointment();
                    break;
                case "2":
                    ViewAllAppointments();
                    break;
                case "3":
                    ViewAppointmentDetails();
                    break;
                case "4":
                    UpdateAppointment();
                    break;
                case "5":
                    DeleteAppointment();
                    break;
                case "6":
                    ViewPhysicianAppointments();
                    break;
                case "7":
                    ViewPatientAppointments();
                    break;
                case "8":
                    appointmentMenu = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    static void CreateAppointment()
    {
        try
        {
            Console.Write("Patient ID: ");
            int patientId = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Physician ID: ");
            int physicianId = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Appointment Date and Time (MM/dd/yyyy HH:mm): ");
            DateTime appointmentDateTime = DateTime.Parse(Console.ReadLine() ?? "");
            Console.Write("Reason: ");
            string reason = Console.ReadLine() ?? "";

            var appointment = manager.AppointmentService.CreateAppointment(patientId, physicianId, appointmentDateTime, reason);
            if (appointment != null)
            {
                Console.WriteLine($"Appointment created successfully! ID: {appointment.Id}");
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Invalid input: {ex.Message}");
        }
    }

    static void ViewAllAppointments()
    {
        var appointments = manager.AppointmentService.GetAllAppointments();
        if (appointments.Count == 0)
        {
            Console.WriteLine("No appointments found.");
            return;
        }

        Console.WriteLine("\n=== All Appointments ===");
        foreach (var appointment in appointments)
        {
            Console.WriteLine(appointment);
        }
    }

    static void ViewAppointmentDetails()
    {
        Console.Write("Enter Appointment ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        var appointment = manager.AppointmentService.GetAppointmentById(id);
        
        if (appointment == null)
        {
            Console.WriteLine("Appointment not found.");
            return;
        }

        Console.WriteLine($"\n{appointment}");
    }

    static void UpdateAppointment()
    {
        try
        {
            Console.Write("Enter Appointment ID: ");
            int id = int.Parse(Console.ReadLine() ?? "0");
            var appointment = manager.AppointmentService.GetAppointmentById(id);
            
            if (appointment == null)
            {
                Console.WriteLine("Appointment not found.");
                return;
            }

            Console.Write("Patient ID (current: " + appointment.PatientId + "): ");
            int patientId = int.Parse(Console.ReadLine() ?? appointment.PatientId.ToString());
            Console.Write("Physician ID (current: " + appointment.PhysicianId + "): ");
            int physicianId = int.Parse(Console.ReadLine() ?? appointment.PhysicianId.ToString());
            Console.Write("Appointment Date and Time (current: " + appointment.AppointmentDateTime.ToString("MM/dd/yyyy HH:mm") + ") MM/dd/yyyy HH:mm: ");
            DateTime appointmentDateTime = DateTime.Parse(Console.ReadLine() ?? appointment.AppointmentDateTime.ToString("MM/dd/yyyy HH:mm"));
            Console.Write("Reason (current: " + appointment.Reason + "): ");
            string reason = Console.ReadLine() ?? appointment.Reason;

            if (manager.AppointmentService.UpdateAppointment(id, patientId, physicianId, appointmentDateTime, reason))
            {
                Console.WriteLine("Appointment updated successfully!");
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Invalid input: {ex.Message}");
        }
    }

    static void DeleteAppointment()
    {
        Console.Write("Enter Appointment ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        
        if (manager.AppointmentService.DeleteAppointment(id))
        {
            Console.WriteLine("Appointment deleted successfully!");
        }
        else
        {
            Console.WriteLine("Appointment not found.");
        }
    }

    static void ViewPhysicianAppointments()
    {
        Console.Write("Enter Physician ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        var appointments = manager.AppointmentService.GetAppointmentsForPhysician(id);
        
        if (appointments.Count == 0)
        {
            Console.WriteLine("No appointments found for this physician.");
            return;
        }

        Console.WriteLine("\n=== Physician's Appointments ===");
        foreach (var appointment in appointments)
        {
            Console.WriteLine(appointment);
        }
    }

    static void ViewPatientAppointments()
    {
        Console.Write("Enter Patient ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        var appointments = manager.AppointmentService.GetAppointmentsForPatient(id);
        
        if (appointments.Count == 0)
        {
            Console.WriteLine("No appointments found for this patient.");
            return;
        }

        Console.WriteLine("\n=== Patient's Appointments ===");
        foreach (var appointment in appointments)
        {
            Console.WriteLine(appointment);
        }
    }
}
