using Library.Klinik.Models;
using Library.Klinik.Services;
using Maui.Klinik.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maui.Klinik.ViewModels;

public class PatientListViewModel : BaseViewModel
{
    private readonly PatientService _patientService;
    private readonly PatientApiService? _apiService;
    private readonly bool _useApi;
    private ObservableCollection<Patient> _patients;
    private string _searchText = string.Empty;
    private string _sortProperty = "LastName";
    private bool _sortAscending = true;

    public ObservableCollection<Patient> Patients
    {
        get => _patients;
        set => SetProperty(ref _patients, value);
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                FilterAndSortPatients();
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
                FilterAndSortPatients();
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
                FilterAndSortPatients();
            }
        }
    }

    public ICommand AddPatientCommand { get; }
    public ICommand EditPatientCommand { get; }
    public ICommand DeletePatientCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ToggleSortCommand { get; }

    public PatientListViewModel(PatientService patientService, PatientApiService? apiService = null)
    {
        _patientService = patientService;
        _apiService = apiService;
        _useApi = apiService != null;
        _patients = new ObservableCollection<Patient>();

        AddPatientCommand = new Command(async () => await AddPatient());
        EditPatientCommand = new Command<Patient>(async (patient) => await EditPatient(patient));
        DeletePatientCommand = new Command<Patient>(async (patient) => await DeletePatient(patient));
        RefreshCommand = new Command(async () => await LoadPatientsAsync());
        ToggleSortCommand = new Command(() => SortAscending = !SortAscending);

        _ = LoadPatientsAsync();
    }

    private List<Patient> _allPatients = new();

    private async Task LoadPatientsAsync()
    {
        try
        {
            if (_useApi && _apiService != null)
            {
                var patientDtos = await _apiService.GetAllPatientsAsync();
                _allPatients = patientDtos.Select(PatientApiService.ToModel).ToList();
            }
            else
            {
                _allPatients = _patientService.GetAllPatients();
            }
            FilterAndSortPatients();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load patients: {ex.Message}", "OK");
        }
    }

    private void FilterAndSortPatients()
    {
        var allPatients = _allPatients.ToList();

        // Filter
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            allPatients = allPatients.Where(p =>
                p.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                p.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Sort
        allPatients = SortProperty switch
        {
            "FirstName" => SortAscending
                ? allPatients.OrderBy(p => p.FirstName).ToList()
                : allPatients.OrderByDescending(p => p.FirstName).ToList(),
            "LastName" => SortAscending
                ? allPatients.OrderBy(p => p.LastName).ToList()
                : allPatients.OrderByDescending(p => p.LastName).ToList(),
            "DateOfBirth" => SortAscending
                ? allPatients.OrderBy(p => p.DateOfBirth).ToList()
                : allPatients.OrderByDescending(p => p.DateOfBirth).ToList(),
            _ => allPatients
        };

        Patients = new ObservableCollection<Patient>(allPatients);
    }

    private async Task AddPatient()
    {
        await Shell.Current.GoToAsync("patientdetail");
    }

    private async Task EditPatient(Patient patient)
    {
        if (patient == null) return;
        await Shell.Current.GoToAsync($"patientdetail?id={patient.Id}");
    }

    private async Task DeletePatient(Patient patient)
    {
        if (patient == null) return;

        bool answer = await Shell.Current.DisplayAlert(
            "Delete Patient",
            $"Are you sure you want to delete {patient.FirstName} {patient.LastName}?",
            "Yes", "No");

        if (answer)
        {
            if (_useApi && _apiService != null)
            {
                await _apiService.DeletePatientAsync(patient.Id);
            }
            else
            {
                _patientService.DeletePatient(patient.Id);
            }
            await LoadPatientsAsync();
        }
    }
}
