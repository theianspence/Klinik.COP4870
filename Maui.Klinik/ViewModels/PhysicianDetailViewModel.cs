using System.Windows.Input;
using Library.Klinik.Models;
using Library.Klinik.Services;

namespace Maui.Klinik.ViewModels;

[QueryProperty(nameof(PhysicianId), "id")]
public class PhysicianDetailViewModel : BaseViewModel
{
    private readonly PhysicianService _physicianService;
    private int _physicianId;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _licenseNumber = string.Empty;
    private DateTime _graduationDate = DateTime.Today;
    private string _specializations = string.Empty;
    private bool _isEditMode;

    public PhysicianDetailViewModel(PhysicianService physicianService)
    {
        _physicianService = physicianService;
        SaveCommand = new Command(async () => await SavePhysician());
        CancelCommand = new Command(async () => await Cancel());
    }

    public string PhysicianId
    {
        set
        {
            if (int.TryParse(value, out int id))
            {
                if (SetProperty(ref _physicianId, id, nameof(PhysicianId)))
                {
                    LoadPhysician(id);
                }
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

    public string LicenseNumber
    {
        get => _licenseNumber;
        set => SetProperty(ref _licenseNumber, value);
    }

    public DateTime GraduationDate
    {
        get => _graduationDate;
        set => SetProperty(ref _graduationDate, value);
    }

    public string Specializations
    {
        get => _specializations;
        set => SetProperty(ref _specializations, value);
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

    public string PageTitle => IsEditMode ? "Edit Physician" : "Add Physician";

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    private void LoadPhysician(int id)
    {
        var physician = _physicianService.GetPhysicianById(id);
        if (physician != null)
        {
            IsEditMode = true;
            FirstName = physician.FirstName;
            LastName = physician.LastName;
            LicenseNumber = physician.LicenseNumber;
            GraduationDate = physician.GraduationDate;
            Specializations = string.Join(", ", physician.Specializations);
            OnPropertyChanged(nameof(PageTitle));
        }
    }

    private async Task SavePhysician()
    {
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) ||
            string.IsNullOrWhiteSpace(LicenseNumber))
        {
            await Shell.Current.DisplayAlert("Error", "Please fill in all required fields.", "OK");
            return;
        }

        var specializationsList = Specializations?
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .ToList() ?? new List<string>();

        if (IsEditMode)
        {
            var physician = _physicianService.GetPhysicianById(_physicianId);
            if (physician != null)
            {
                _physicianService.UpdatePhysician(_physicianId, FirstName, LastName, LicenseNumber, GraduationDate);
                
                // Update specializations
                physician.Specializations.Clear();
                foreach (var spec in specializationsList)
                {
                    physician.Specializations.Add(spec);
                }
            }
        }
        else
        {
            var physician = _physicianService.CreatePhysician(FirstName, LastName, LicenseNumber, GraduationDate);
            foreach (var spec in specializationsList)
            {
                physician.Specializations.Add(spec);
            }
        }

        await Shell.Current.GoToAsync("..");
    }

    private async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
