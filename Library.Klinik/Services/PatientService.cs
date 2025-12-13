namespace Library.Klinik.Services;

using Library.Klinik.Models;
using System.Data;
using MySqlConnector;
using System.Text.Json;
using System.Text.Json.Serialization;

public class PatientService
{
    private List<Patient> patients = new();
    private int nextPatientId = 1;
    private readonly string? connectionString;
    private readonly bool useDatabase;

    public PatientService(string? mySqlConnectionString = null)
    {
        connectionString = string.IsNullOrWhiteSpace(mySqlConnectionString) ? null : mySqlConnectionString;
        useDatabase = connectionString != null;

        if (useDatabase)
        {
            // Ensure table exists
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Patients (
                Id INT PRIMARY KEY AUTO_INCREMENT,
                FirstName VARCHAR(200) NOT NULL,
                LastName VARCHAR(200) NOT NULL,
                Address TEXT,
                DateOfBirth DATETIME,
                Race VARCHAR(100),
                Gender VARCHAR(100),
                MedicalNotes TEXT
            );";
            cmd.ExecuteNonQuery();
            // Load existing patients from DB into memory cache
            patients = LoadAllFromDatabase();
            nextPatientId = patients.Count == 0 ? 1 : patients.Max(p => p.Id) + 1;
        }
    }

    public Patient CreatePatient(string firstName, string lastName, string address, 
                                  DateTime dateOfBirth, string race, string gender)
    {
        var patient = new Patient
        {
            FirstName = firstName,
            LastName = lastName,
            Address = address,
            DateOfBirth = dateOfBirth,
            Race = race,
            Gender = gender
        };

        if (useDatabase)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Patients (FirstName, LastName, Address, DateOfBirth, Race, Gender, MedicalNotes)
                                VALUES (@fn, @ln, @addr, @dob, @race, @gender, @notes); SELECT LAST_INSERT_ID();";
            cmd.Parameters.AddWithValue("@fn", patient.FirstName);
            cmd.Parameters.AddWithValue("@ln", patient.LastName);
            cmd.Parameters.AddWithValue("@addr", patient.Address);
            cmd.Parameters.AddWithValue("@dob", patient.DateOfBirth);
            cmd.Parameters.AddWithValue("@race", patient.Race);
            cmd.Parameters.AddWithValue("@gender", patient.Gender);
            cmd.Parameters.AddWithValue("@notes", JsonSerializer.Serialize(patient.MedicalNotes));
            var idObj = cmd.ExecuteScalar();
            if (idObj != null && int.TryParse(idObj.ToString(), out var id))
            {
                patient.Id = id;
            }
            patients.Add(patient);
            return patient;
        }

        // fallback in-memory
        patient.Id = nextPatientId++;
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
        if (useDatabase)
        {
            // Insert into DB and set Id
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Patients (FirstName, LastName, Address, DateOfBirth, Race, Gender, MedicalNotes)
                                VALUES (@fn, @ln, @addr, @dob, @race, @gender, @notes); SELECT LAST_INSERT_ID();";
            cmd.Parameters.AddWithValue("@fn", patient.FirstName);
            cmd.Parameters.AddWithValue("@ln", patient.LastName);
            cmd.Parameters.AddWithValue("@addr", patient.Address);
            cmd.Parameters.AddWithValue("@dob", patient.DateOfBirth);
            cmd.Parameters.AddWithValue("@race", patient.Race);
            cmd.Parameters.AddWithValue("@gender", patient.Gender);
            cmd.Parameters.AddWithValue("@notes", JsonSerializer.Serialize(patient.MedicalNotes));
            var idObj = cmd.ExecuteScalar();
            if (idObj != null && int.TryParse(idObj.ToString(), out var id))
            {
                patient.Id = id;
            }
            patients.Add(patient);
            return patient;
        }

        patients.Add(patient);
        return patient;
    }

    public Patient? GetPatientById(int id)
    {
        if (useDatabase)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, FirstName, LastName, Address, DateOfBirth, Race, Gender, MedicalNotes FROM Patients WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return ReadPatientFromReader(reader);
            }
            return null;
        }

        return patients.FirstOrDefault(p => p.Id == id);
    }

    public List<Patient> GetAllPatients()
    {
        if (useDatabase)
        {
            return LoadAllFromDatabase();
        }

        return new List<Patient>(patients);
    }

    public bool UpdatePatient(int id, string firstName, string lastName, string address,
                              DateTime dateOfBirth, string race, string gender)
    {
        if (useDatabase)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE Patients SET FirstName=@fn, LastName=@ln, Address=@addr, DateOfBirth=@dob, Race=@race, Gender=@gender
                                WHERE Id=@id";
            cmd.Parameters.AddWithValue("@fn", firstName);
            cmd.Parameters.AddWithValue("@ln", lastName);
            cmd.Parameters.AddWithValue("@addr", address);
            cmd.Parameters.AddWithValue("@dob", dateOfBirth);
            cmd.Parameters.AddWithValue("@race", race);
            cmd.Parameters.AddWithValue("@gender", gender);
            cmd.Parameters.AddWithValue("@id", id);
            var affected = cmd.ExecuteNonQuery();
            return affected > 0;
        }

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
        if (useDatabase)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE Patients SET FirstName=@fn, LastName=@ln, Address=@addr, DateOfBirth=@dob, Race=@race, Gender=@gender, MedicalNotes=@notes
                                WHERE Id=@id";
            cmd.Parameters.AddWithValue("@fn", updatedPatient.FirstName);
            cmd.Parameters.AddWithValue("@ln", updatedPatient.LastName);
            cmd.Parameters.AddWithValue("@addr", updatedPatient.Address);
            cmd.Parameters.AddWithValue("@dob", updatedPatient.DateOfBirth);
            cmd.Parameters.AddWithValue("@race", updatedPatient.Race);
            cmd.Parameters.AddWithValue("@gender", updatedPatient.Gender);
            cmd.Parameters.AddWithValue("@notes", JsonSerializer.Serialize(updatedPatient.MedicalNotes));
            cmd.Parameters.AddWithValue("@id", updatedPatient.Id);
            var affected = cmd.ExecuteNonQuery();
            return affected > 0;
        }

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
        if (useDatabase)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Patients WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            var affected = cmd.ExecuteNonQuery();
            if (affected > 0)
            {
                // update in-memory cache
                patients.RemoveAll(p => p.Id == id);
                return true;
            }
            return false;
        }

        var patient = GetPatientById(id);
        if (patient == null) return false;
        patients.Remove(patient);
        return true;
    }

    public bool AddMedicalNote(int patientId, string diagnosis, string prescription)
    {
        var note = $"[{DateTime.Now:MM/dd/yyyy HH:mm}] Diagnosis: {diagnosis}, Prescription: {prescription}";

        if (useDatabase)
        {
            var patient = GetPatientById(patientId);
            if (patient == null) return false;
            patient.MedicalNotes.Add(note);
            return UpdatePatient(patient);
        }

        var p = GetPatientById(patientId);
        if (p == null) return false;
        p.MedicalNotes.Add(note);
        return true;
    }

    public List<string> GetMedicalNotes(int patientId)
    {
        var patient = GetPatientById(patientId);
        return patient?.MedicalNotes ?? new List<string>();
    }

    private List<Patient> LoadAllFromDatabase()
    {
        var list = new List<Patient>();
        using var conn = new MySqlConnection(connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, FirstName, LastName, Address, DateOfBirth, Race, Gender, MedicalNotes FROM Patients";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(ReadPatientFromReader(reader));
        }
        return list;
    }

    private Patient ReadPatientFromReader(MySqlDataReader reader)
    {
        var p = new Patient
        {
            Id = reader.GetInt32("Id"),
            FirstName = reader.GetString("FirstName") ?? string.Empty,
            LastName = reader.GetString("LastName") ?? string.Empty,
            Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? string.Empty : reader.GetString("Address"),
            DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? DateTime.MinValue : reader.GetDateTime("DateOfBirth"),
            Race = reader.IsDBNull(reader.GetOrdinal("Race")) ? string.Empty : reader.GetString("Race"),
            Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? string.Empty : reader.GetString("Gender"),
            MedicalNotes = new List<string>()
        };
        if (!reader.IsDBNull(reader.GetOrdinal("MedicalNotes")))
        {
            var notesText = reader.GetString("MedicalNotes");
            try
            {
                var notes = JsonSerializer.Deserialize<List<string>>(notesText);
                if (notes != null) p.MedicalNotes = notes;
            }
            catch
            {
                // ignore parse errors
            }
        }
        return p;
    }
}
