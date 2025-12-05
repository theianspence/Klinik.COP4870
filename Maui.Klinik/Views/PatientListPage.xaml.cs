using Maui.Klinik.ViewModels;

namespace Maui.Klinik.Views;

public partial class PatientListPage : ContentPage
{
    private readonly PatientListViewModel _viewModel;

    public PatientListPage(PatientListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.RefreshCommand.Execute(null);
    }
}
