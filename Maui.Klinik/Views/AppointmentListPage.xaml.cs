using Maui.Klinik.ViewModels;

namespace Maui.Klinik.Views;

public partial class AppointmentListPage : ContentPage
{
    private readonly AppointmentListViewModel _viewModel;

    public AppointmentListPage(AppointmentListViewModel viewModel)
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
