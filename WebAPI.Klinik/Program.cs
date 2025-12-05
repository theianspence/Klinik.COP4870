using Library.Klinik.Models;
using Library.Klinik.Services;
using Library.Klinik.DTOs;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configure JSON serialization options
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// Add services to the container.
builder.Services.AddOpenApi();

// Add CORS for MAUI app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register services as singletons
builder.Services.AddSingleton<PatientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Patient API Endpoints

// GET: /api/patients - Get all patients
app.MapGet("/api/patients", (PatientService patientService) =>
{
    var patients = patientService.GetAllPatients();
    var patientDTOs = patients.Select(PatientMapper.ToDTO).ToList();
    return Results.Ok(patientDTOs);
})
.WithName("GetAllPatients")
.WithTags("Patients");

// GET: /api/patients/summary - Get all patients (summary view)
app.MapGet("/api/patients/summary", (PatientService patientService) =>
{
    var patients = patientService.GetAllPatients();
    var summaries = patients.Select(PatientMapper.ToSummaryDTO).ToList();
    return Results.Ok(summaries);
})
.WithName("GetPatientsSummary")
.WithTags("Patients");

// GET: /api/patients/{id} - Get patient by ID
app.MapGet("/api/patients/{id}", (PatientService patientService, int id) =>
{
    var patient = patientService.GetPatientById(id);
    if (patient == null)
    {
        return Results.NotFound(new { message = $"Patient with ID {id} not found" });
    }
    var patientDTO = PatientMapper.ToDTO(patient);
    return Results.Ok(patientDTO);
})
.WithName("GetPatientById")
.WithTags("Patients");

// POST: /api/patients - Create a new patient
app.MapPost("/api/patients", (PatientService patientService, CreatePatientDTO dto) =>
{
    try
    {
        var patient = PatientMapper.ToModel(dto);
        var createdPatient = patientService.AddPatient(patient);
        var patientDTO = PatientMapper.ToDTO(createdPatient);
        return Results.Created($"/api/patients/{createdPatient.Id}", patientDTO);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
})
.WithName("CreatePatient")
.WithTags("Patients");

// PUT: /api/patients/{id} - Update an existing patient
app.MapPut("/api/patients/{id}", (PatientService patientService, int id, UpdatePatientDTO dto) =>
{
    var existingPatient = patientService.GetPatientById(id);
    if (existingPatient == null)
    {
        return Results.NotFound(new { message = $"Patient with ID {id} not found" });
    }

    try
    {
        PatientMapper.UpdateModel(existingPatient, dto);
        var success = patientService.UpdatePatient(existingPatient);

        if (success)
        {
            var updatedPatient = patientService.GetPatientById(id);
            var patientDTO = PatientMapper.ToDTO(updatedPatient!);
            return Results.Ok(patientDTO);
        }
        return Results.BadRequest(new { message = "Failed to update patient" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
})
.WithName("UpdatePatient")
.WithTags("Patients");

// DELETE: /api/patients/{id} - Delete a patient
app.MapDelete("/api/patients/{id}", (PatientService patientService, int id) =>
{
    var patient = patientService.GetPatientById(id);
    if (patient == null)
    {
        return Results.NotFound(new { message = $"Patient with ID {id} not found" });
    }

    var success = patientService.DeletePatient(id);
    if (success)
    {
        return Results.Ok(new { message = $"Patient with ID {id} deleted successfully" });
    }
    return Results.BadRequest(new { message = "Failed to delete patient" });
})
.WithName("DeletePatient")
.WithTags("Patients");

// GET: /api/patients/search?query={query} - Search for patients
app.MapGet("/api/patients/search", (PatientService patientService, string query) =>
{
    var allPatients = patientService.GetAllPatients();
    var filteredPatients = allPatients.Where(p =>
        p.FirstName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
        p.LastName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
        p.Address.Contains(query, StringComparison.OrdinalIgnoreCase)
    ).ToList();

    var patientDTOs = filteredPatients.Select(PatientMapper.ToDTO).ToList();
    return Results.Ok(patientDTOs);
})
.WithName("SearchPatients")
.WithTags("Patients");

app.Run();
