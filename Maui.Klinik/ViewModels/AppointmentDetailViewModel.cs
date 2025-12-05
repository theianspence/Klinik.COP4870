using System.Collections.ObjectModel;
using System.Windows.Input;
using Library.Klinik.Models;
using Library.Klinik.Services;
using Maui.Klinik.Services;

namespace Maui.Klinik.ViewModels;

[QueryProperty(nameof(AppointmentId), "id")]
public class AppointmentDetailViewModel : BaseViewModel
{
    private readonly AppointmentService _appointmentService;
    private readonly PatientService _patientService;
    private readonly PhysicianService _physicianService;
    private readonly PatientApiService? _patientApiService;
    
    private int _appointmentId;
    private DateTime _appointmentDate = DateTime.Today;
    private TimeSpan _appointmentTime = new TimeSpan(9, 0, 0);
    private Patient? _selectedPatient;
    private Physician? _selectedPhysician;
    private bool _isEditMode;
    private string _room = string.Empty;
    private string _reason = string.Empty;

    public AppointmentDetailViewModel(
        AppointmentService appointmentService,
        PatientService patientService,
        PhysicianService physicianService,
        PatientApiService? patientApiService = null)
    {
        _appointmentService = appointmentService;
        _patientService = patientService;
        _physicianService = physicianService;
        _patientApiService = patientApiService;

        SaveCommand = new Command(async () => await SaveAppointment());
        CancelCommand = new Command(async () => await Cancel());

        _ = LoadPatientsAsync();
        LoadPhysicians();
    }

    public string AppointmentId
    {
        set
        {
            if (int.TryParse(value, out int id))
            {
                if (SetProperty(ref _appointmentId, id, nameof(AppointmentId)))
                {
                    _ = LoadAppointmentAsync(id);
                }
            }
        }
    }

    public ObservableCollection<Patient> Patients { get; } = new();
    public ObservableCollection<Physician> Physicians { get; } = new();

    public Patient? SelectedPatient
    {
        get => _selectedPatient;
        set => SetProperty(ref _selectedPatient, value);
    }

    public Physician? SelectedPhysician
    {
        get => _selectedPhysician;
        set => SetProperty(ref _selectedPhysician, value);
    }

    public DateTime AppointmentDate
    {
        get => _appointmentDate;
        set => SetProperty(ref _appointmentDate, value);
    }

    public TimeSpan AppointmentTime
    {
        get => _appointmentTime;
        set => SetProperty(ref _appointmentTime, value);
    }

    public string Room
    {
        get => _room;
        set => SetProperty(ref _room, value);
    }

    public string Reason
    {
        get => _reason;
        set => SetProperty(ref _reason, value);
    }

    public bool IsEditMode
    {
        get => _isEditMode;
        set
        {
            if (SetProperty(ref _isEditMode, value))
            {
                OnPropertyChanged(nameof(PageTitle));
            }
        }
    }

    public string PageTitle => IsEditMode ? "Edit Appointment" : "Add Appointment";

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    private async Task LoadPatientsAsync()
    {
        Patients.Clear();
        if (_patientApiService != null)
        {
            try
            {
                var patientDtos = await _patientApiService.GetAllPatientsAsync();
                foreach (var dto in patientDtos)
                {
                    Patients.Add(PatientApiService.ToModel(dto));
                }
            }
            catch
            {
                // Fall back to local service if API fails
                var patients = _patientService.GetAllPatients();
                foreach (var patient in patients)
                {
                    Patients.Add(patient);
                }
            }
        }
        else
        {
            var patients = _patientService.GetAllPatients();
            foreach (var patient in patients)
            {
                Patients.Add(patient);
            }
        }
    }

    private void LoadPhysicians()
    {
        Physicians.Clear();
        var physicians = _physicianService.GetAllPhysicians();
        foreach (var physician in physicians)
        {
            Physicians.Add(physician);
        }
    }

    private async Task LoadAppointmentAsync(int id)
    {
        // Wait for patients to load first
        while (Patients.Count == 0 && _patientApiService != null)
        {
            await Task.Delay(50);
        }

        var appointment = _appointmentService.GetAppointmentById(id);
        if (appointment != null)
        {
            IsEditMode = true;
            AppointmentDate = appointment.AppointmentDateTime.Date;
            AppointmentTime = appointment.AppointmentDateTime.TimeOfDay;
            SelectedPatient = Patients.FirstOrDefault(p => p.Id == appointment.PatientId);
            SelectedPhysician = Physicians.FirstOrDefault(p => p.Id == appointment.PhysicianId);
            Room = appointment.Room;
            Reason = appointment.Reason;
            OnPropertyChanged(nameof(PageTitle));
        }
    }

    private async Task SaveAppointment()
    {
        if (SelectedPatient == null || SelectedPhysician == null)
        {
            await Shell.Current.DisplayAlert("Error", "Please select both a patient and a physician.", "OK");
            return;
        }

        var startTime = AppointmentDate.Date + AppointmentTime;

        // Validate business hours (8am-5pm Monday-Friday)
        if (startTime.DayOfWeek == DayOfWeek.Saturday || startTime.DayOfWeek == DayOfWeek.Sunday)
        {
            await Shell.Current.DisplayAlert("Error", "Appointments can only be scheduled Monday through Friday.", "OK");
            return;
        }

        if (startTime.Hour < 8 || startTime.Hour >= 17)
        {
            await Shell.Current.DisplayAlert("Error", "Appointments must be between 8:00 AM and 5:00 PM.", "OK");
            return;
        }

        try
        {
            if (IsEditMode)
            {
                _appointmentService.UpdateAppointment(_appointmentId, SelectedPatient.Id, SelectedPhysician.Id, startTime, Reason, Room);
            }
            else
            {
                _appointmentService.CreateAppointment(SelectedPatient.Id, SelectedPhysician.Id, startTime, Reason, Room);
            }

            await Shell.Current.GoToAsync("..");
        }
        catch (InvalidOperationException ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        catch (ArgumentException ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
