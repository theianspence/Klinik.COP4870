using System.Collections.ObjectModel;
using System.Windows.Input;
using Library.Klinik.Models;
using Library.Klinik.Services;

namespace Maui.Klinik.ViewModels;

public class PhysicianListViewModel : BaseViewModel
{
    private readonly PhysicianService _physicianService;
    private ObservableCollection<Physician> _physicians;
    private ObservableCollection<Physician> _filteredPhysicians;
    private string _searchText = string.Empty;
    private string _sortProperty = "LastName";
    private bool _sortAscending = true;

    public PhysicianListViewModel(PhysicianService physicianService)
    {
        _physicianService = physicianService;
        _physicians = new ObservableCollection<Physician>();
        _filteredPhysicians = new ObservableCollection<Physician>();

        AddPhysicianCommand = new Command(async () => await AddPhysician());
        EditPhysicianCommand = new Command<Physician>(async (physician) => await EditPhysician(physician));
        DeletePhysicianCommand = new Command<Physician>(async (physician) => await DeletePhysician(physician));
        RefreshCommand = new Command(LoadPhysicians);
        ToggleSortDirectionCommand = new Command(ToggleSortDirection);

        LoadPhysicians();
    }

    public ObservableCollection<Physician> FilteredPhysicians
    {
        get => _filteredPhysicians;
        set => SetProperty(ref _filteredPhysicians, value);
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                FilterAndSortPhysicians();
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
                FilterAndSortPhysicians();
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
                FilterAndSortPhysicians();
            }
        }
    }

    public ICommand AddPhysicianCommand { get; }
    public ICommand EditPhysicianCommand { get; }
    public ICommand DeletePhysicianCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ToggleSortDirectionCommand { get; }

    private void LoadPhysicians()
    {
        _physicians.Clear();
        var physicians = _physicianService.GetAllPhysicians();
        foreach (var physician in physicians)
        {
            _physicians.Add(physician);
        }
        FilterAndSortPhysicians();
    }

    private void FilterAndSortPhysicians()
    {
        var filtered = _physicians.AsEnumerable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(p =>
                p.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                p.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                p.LicenseNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        // Apply sorting
        filtered = SortProperty switch
        {
            "FirstName" => SortAscending
                ? filtered.OrderBy(p => p.FirstName)
                : filtered.OrderByDescending(p => p.FirstName),
            "LastName" => SortAscending
                ? filtered.OrderBy(p => p.LastName)
                : filtered.OrderByDescending(p => p.LastName),
            "GraduationDate" => SortAscending
                ? filtered.OrderBy(p => p.GraduationDate)
                : filtered.OrderByDescending(p => p.GraduationDate),
            _ => filtered
        };

        FilteredPhysicians.Clear();
        foreach (var physician in filtered)
        {
            FilteredPhysicians.Add(physician);
        }
    }

    private async Task AddPhysician()
    {
        await Shell.Current.GoToAsync("physiciandetail");
    }

    private async Task EditPhysician(Physician physician)
    {
        if (physician == null) return;
        await Shell.Current.GoToAsync($"physiciandetail?id={physician.Id}");
    }

    private async Task DeletePhysician(Physician physician)
    {
        if (physician == null) return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Confirm Delete",
            $"Are you sure you want to delete Dr. {physician.FirstName} {physician.LastName}?",
            "Yes", "No");

        if (confirm)
        {
            _physicianService.DeletePhysician(physician.Id);
            LoadPhysicians();
        }
    }

    private void ToggleSortDirection()
    {
        SortAscending = !SortAscending;
    }
}
