using Library.Klinik.DTOs;
using Library.Klinik.Utilities;
using Library.Klinik.Models;

namespace Maui.Klinik.Services;

public class PatientApiService
{
    private readonly WebRequestHandler _webRequestHandler;

    public PatientApiService(string baseUrl = "localhost", int port = 5149)
    {
        _webRequestHandler = new WebRequestHandler(baseUrl, port.ToString());
    }

    public async Task<List<PatientDTO>> GetAllPatientsAsync()
    {
        var json = await _webRequestHandler.Get("/api/patients");
        if (json != null)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<PatientDTO>>(json) ?? new List<PatientDTO>();
        }
        return new List<PatientDTO>();
    }

    public async Task<PatientDTO?> GetPatientByIdAsync(int id)
    {
        var json = await _webRequestHandler.Get($"/api/patients/{id}");
        if (json != null)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<PatientDTO>(json);
        }
        return null;
    }

    public async Task<PatientDTO?> CreatePatientAsync(CreatePatientDTO dto)
    {
        var json = await _webRequestHandler.Post("/api/patients", dto);
        if (json != null && json != "ERROR")
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<PatientDTO>(json);
        }
        return null;
    }

    public async Task<PatientDTO?> UpdatePatientAsync(int id, UpdatePatientDTO dto)
    {
        var json = await _webRequestHandler.Put($"/api/patients/{id}", dto);
        if (json != null && json != "ERROR")
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<PatientDTO>(json);
        }
        return null;
    }

    public async Task<bool> DeletePatientAsync(int id)
    {
        try
        {
            var result = await _webRequestHandler.Delete($"/api/patients/{id}");
            return result != null;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<PatientDTO>> SearchPatientsAsync(string query)
    {
        var json = await _webRequestHandler.Get($"/api/patients/search?query={query}");
        if (json != null)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<PatientDTO>>(json) ?? new List<PatientDTO>();
        }
        return new List<PatientDTO>();
    }

    public async Task<List<PatientSummaryDTO>> GetPatientsSummaryAsync()
    {
        var json = await _webRequestHandler.Get("/api/patients/summary");
        if (json != null)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<PatientSummaryDTO>>(json) ?? new List<PatientSummaryDTO>();
        }
        return new List<PatientSummaryDTO>();
    }

    // Helper methods to convert between DTOs and Models for MAUI compatibility
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

    public static CreatePatientDTO ToCreateDTO(Patient patient)
    {
        return new CreatePatientDTO
        {
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Address = patient.Address,
            DateOfBirth = patient.DateOfBirth,
            Race = patient.Race,
            Gender = patient.Gender,
            MedicalNotes = new List<string>(patient.MedicalNotes)
        };
    }

    public static UpdatePatientDTO ToUpdateDTO(Patient patient)
    {
        return new UpdatePatientDTO
        {
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Address = patient.Address,
            DateOfBirth = patient.DateOfBirth,
            Race = patient.Race,
            Gender = patient.Gender,
            MedicalNotes = new List<string>(patient.MedicalNotes)
        };
    }
}
