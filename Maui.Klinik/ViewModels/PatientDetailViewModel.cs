using Library.Klinik.Models;
using Library.Klinik.Services;
using Maui.Klinik.Services;
using System.Windows.Input;

namespace Maui.Klinik.ViewModels;

[QueryProperty(nameof(PatientId), "id")]
public class PatientDetailViewModel : BaseViewModel
{
    private readonly PatientService _patientService;
    private readonly PatientApiService? _apiService;
    private readonly bool _useApi;
    private int _patientId;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _address = string.Empty;
    private DateTime _dateOfBirth = DateTime.Now.AddYears(-30);
    private string _race = string.Empty;
    private string _gender = string.Empty;
    private bool _isEditMode;

    public int PatientId
    {
        get => _patientId;
        set
        {
            if (SetProperty(ref _patientId, value))
            {
                LoadPatient(value);
            }
        }
    }

    public string FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }

    public string LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }

    public string Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set => SetProperty(ref _dateOfBirth, value);
    }

    public string Race
    {
        get => _race;
        set => SetProperty(ref _race, value);
    }

    public string Gender
    {
        get => _gender;
        set => SetProperty(ref _gender, value);
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

    public string PageTitle => IsEditMode ? "Edit Patient" : "New Patient";

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public PatientDetailViewModel(PatientService patientService, PatientApiService? apiService = null)
    {
        _patientService = patientService;
        _apiService = apiService;
        _useApi = apiService != null;
        SaveCommand = new Command(async () => await Save());
        CancelCommand = new Command(async () => await Cancel());
    }

    private async void LoadPatient(int id)
    {
        if (id > 0)
        {
            Patient? patient = null;
            
            if (_useApi && _apiService != null)
            {
                var patientDto = await _apiService.GetPatientByIdAsync(id);
                if (patientDto != null)
                {
                    patient = PatientApiService.ToModel(patientDto);
                }
            }
            else
            {
                patient = _patientService.GetPatientById(id);
            }
            
            if (patient != null)
            {
                IsEditMode = true;
                FirstName = patient.FirstName;
                LastName = patient.LastName;
                Address = patient.Address;
                DateOfBirth = patient.DateOfBirth;
                Race = patient.Race;
                Gender = patient.Gender;
                OnPropertyChanged(nameof(PageTitle));
            }
        }
        else
        {
            IsEditMode = false;
            OnPropertyChanged(nameof(PageTitle));
        }
    }

    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
        {
            await Shell.Current.DisplayAlert("Error", "First Name and Last Name are required", "OK");
            return;
        }

        try
        {
            if (_useApi && _apiService != null)
            {
                if (IsEditMode)
                {
                    var updateDto = new Library.Klinik.DTOs.UpdatePatientDTO
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        Address = Address,
                        DateOfBirth = DateOfBirth,
                        Race = Race,
                        Gender = Gender,
                        MedicalNotes = new List<string>()
                    };
                    await _apiService.UpdatePatientAsync(PatientId, updateDto);
                }
                else
                {
                    var createDto = new Library.Klinik.DTOs.CreatePatientDTO
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        Address = Address,
                        DateOfBirth = DateOfBirth,
                        Race = Race,
                        Gender = Gender,
                        MedicalNotes = new List<string>()
                    };
                    await _apiService.CreatePatientAsync(createDto);
                }
            }
            else
            {
                if (IsEditMode)
                {
                    _patientService.UpdatePatient(PatientId, FirstName, LastName, Address, DateOfBirth, Race, Gender);
                }
                else
                {
                    _patientService.CreatePatient(FirstName, LastName, Address, DateOfBirth, Race, Gender);
                }
            }

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to save patient: {ex.Message}", "OK");
        }
    }

    private async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
