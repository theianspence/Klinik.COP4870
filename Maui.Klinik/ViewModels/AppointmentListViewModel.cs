using System.Collections.ObjectModel;
using System.Windows.Input;
using Library.Klinik.Models;
using Library.Klinik.Services;

namespace Maui.Klinik.ViewModels;

public class AppointmentListViewModel : BaseViewModel
{
    private readonly AppointmentService _appointmentService;
    private readonly PatientService _patientService;
    private readonly PhysicianService _physicianService;
    private ObservableCollection<Appointment> _appointments;
    private ObservableCollection<AppointmentDisplay> _filteredAppointments;
    private string _searchText = string.Empty;
    private string _sortProperty = "StartTime";
    private bool _sortAscending = true;

    public AppointmentListViewModel(
        AppointmentService appointmentService,
        PatientService patientService,
        PhysicianService physicianService)
    {
        _appointmentService = appointmentService;
        _patientService = patientService;
        _physicianService = physicianService;
        _appointments = new ObservableCollection<Appointment>();
        _filteredAppointments = new ObservableCollection<AppointmentDisplay>();

        AddAppointmentCommand = new Command(async () => await AddAppointment());
        EditAppointmentCommand = new Command<AppointmentDisplay>(async (appointment) => await EditAppointment(appointment));
        DeleteAppointmentCommand = new Command<AppointmentDisplay>(async (appointment) => await DeleteAppointment(appointment));
        RefreshCommand = new Command(LoadAppointments);
        ToggleSortDirectionCommand = new Command(ToggleSortDirection);

        LoadAppointments();
    }

    public ObservableCollection<AppointmentDisplay> FilteredAppointments
    {
        get => _filteredAppointments;
        set => SetProperty(ref _filteredAppointments, value);
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                FilterAndSortAppointments();
            }
        }
    }

    public string SortProperty
    {
        get => _sortProperty;
        set
        {
            if (SetProperty(ref _sortProperty, value))
            {
                FilterAndSortAppointments();
            }
        }
    }

    public bool SortAscending
    {
        get => _sortAscending;
        set
        {
            if (SetProperty(ref _sortAscending, value))
            {
                FilterAndSortAppointments();
            }
        }
    }

    public ICommand AddAppointmentCommand { get; }
    public ICommand EditAppointmentCommand { get; }
    public ICommand DeleteAppointmentCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ToggleSortDirectionCommand { get; }

    private void LoadAppointments()
    {
        _appointments.Clear();
        var appointments = _appointmentService.GetAllAppointments();
        foreach (var appointment in appointments)
        {
            _appointments.Add(appointment);
        }
        FilterAndSortAppointments();
    }

    private void FilterAndSortAppointments()
    {
        var filtered = _appointments.AsEnumerable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(a =>
            {
                var patient = _patientService.GetPatientById(a.PatientId);
                var physician = _physicianService.GetPhysicianById(a.PhysicianId);
                return (patient != null && patient.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                       (physician != null && physician.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            });
        }

        // Apply sorting
        filtered = SortProperty switch
        {
            "StartTime" => SortAscending
                ? filtered.OrderBy(a => a.AppointmentDateTime)
                : filtered.OrderByDescending(a => a.AppointmentDateTime),
            "Patient" => SortAscending
                ? filtered.OrderBy(a => _patientService.GetPatientById(a.PatientId)?.FullName)
                : filtered.OrderByDescending(a => _patientService.GetPatientById(a.PatientId)?.FullName),
            "Physician" => SortAscending
                ? filtered.OrderBy(a => _physicianService.GetPhysicianById(a.PhysicianId)?.FullName)
                : filtered.OrderByDescending(a => _physicianService.GetPhysicianById(a.PhysicianId)?.FullName),
            _ => filtered
        };

        FilteredAppointments.Clear();
        foreach (var appointment in filtered)
        {
            var display = new AppointmentDisplay
            {
                Appointment = appointment,
                PatientName = _patientService.GetPatientById(appointment.PatientId)?.FullName ?? "Unknown",
                PhysicianName = _physicianService.GetPhysicianById(appointment.PhysicianId)?.FullName ?? "Unknown"
            };
            FilteredAppointments.Add(display);
        }
    }

    public string GetPatientName(int patientId)
    {
        return _patientService.GetPatientById(patientId)?.FullName ?? "Unknown";
    }

    public string GetPhysicianName(int physicianId)
    {
        return _physicianService.GetPhysicianById(physicianId)?.FullName ?? "Unknown";
    }

    private async Task AddAppointment()
    {
        await Shell.Current.GoToAsync("appointmentdetail");
    }

    private async Task EditAppointment(AppointmentDisplay appointmentDisplay)
    {
        if (appointmentDisplay == null) return;
        await Shell.Current.GoToAsync($"appointmentdetail?id={appointmentDisplay.Appointment.Id}");
    }

    private async Task DeleteAppointment(AppointmentDisplay appointmentDisplay)
    {
        if (appointmentDisplay == null) return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Confirm Delete",
            $"Delete appointment for {appointmentDisplay.PatientName} with Dr. {appointmentDisplay.PhysicianName} on {appointmentDisplay.StartTime:g}?",
            "Yes", "No");

        if (confirm)
        {
            _appointmentService.DeleteAppointment(appointmentDisplay.Appointment.Id);
            LoadAppointments();
        }
    }

    private void ToggleSortDirection()
    {
        SortAscending = !SortAscending;
    }
}
